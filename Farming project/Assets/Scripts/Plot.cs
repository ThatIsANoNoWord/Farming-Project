using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Plot : MonoBehaviour, ITurnable
{
    private SoilQuality plotQuality;
    [SerializeField] private int quality;
    [SerializeField] private GameObject moundObject;
    [SerializeField] private GameObject inactiveObject;
    public Sprite[] qualitySprites;
    private SpriteRenderer spriteRenderer;
    private int pendingOrganicMatter;
    private List<PlantingSpot> plantingSpots;
    public bool plotActive;
    public int buyPrice;
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        spriteRenderer = transform.Find("Plot").GetComponent<SpriteRenderer>();
        moundObject.SetActive(false);
        inactiveObject.SetActive(!plotActive);
        plantingSpots = new List<PlantingSpot>();
        plantingSpots.AddRange(GetComponentsInChildren<PlantingSpot>(true));

        // Randomize the list of plantingSpots
        plantingSpots = plantingSpots.OrderBy(x => Random.value).ToList();

        // Randomize the starting quality of the land except for Plot #2
        // Randomize with a curve: higher values are more rare
        float randomValue = Mathf.Pow(Random.value, 7);
        quality = Mathf.FloorToInt(randomValue * 100);
        if (gameObject.name == "Plot-02") quality = 30;

        if (!plotActive) plantingSpots.ForEach(x => x.gameObject.GetComponent<Collider2D>().enabled = false);

        UpdatePlotQuality(0);
    }

    public int GetQuality()
    {
        return quality;
    }
    public SoilQuality GetSQ()
    {
        return plotQuality;
    }

    public void MakeConuco(int increase)
    {
        pendingOrganicMatter += increase;
        plantingSpots.ForEach(x => x.CompostMode());
        moundObject.SetActive(true);
    }

    public int GetPending()
    {
        return pendingOrganicMatter;
    }

    public void NoConuco()
    {
        if (pendingOrganicMatter == 0) return;
        moundObject.SetActive(false);
        pendingOrganicMatter = 0;
        plantingSpots.ForEach(x => x.EmptyPlot());
    }

    public int AttemptPurchase()
    {
        if (gameManager.GetMoney() >= buyPrice)
        {
            gameManager.ChangeMoney(-buyPrice);
            plotActive = true;
            inactiveObject.SetActive(false);
            plantingSpots.ForEach(x => x.gameObject.GetComponent<Collider2D>().enabled = true);
            return 1;
        }
        return -1;
    }

    public float QualityRatio()
    {
        return quality / 100f;
    }

    public void UpdatePlotQuality(int number)
    {
        quality = Mathf.Clamp(quality + number, 0, 100);

        plotQuality = IntToSQ(quality);

        spriteRenderer.sprite = qualitySprites[(int)plotQuality];

        // Activate spots or de-activate spots based on soil quality
        // If a spot is empty, keep it active until harvested. This will be called whenever a plant is harvested.
        // Min is 1 so they can still put stuff on it
        int targetCount = Mathf.Clamp(3 * (int) plotQuality, 1, 9);
        int activated = 0;

        for (int i = 0; i < plantingSpots.Count; i++) {
            PlantingSpot spot = plantingSpots[i];

            if (activated < targetCount) spot.gameObject.SetActive(true);
            else if (spot.IsEmpty()) spot.gameObject.SetActive(false);

            activated++;
        }
    }

    public SoilQuality IntToSQ(int quality)
    {
        switch (quality)
        {
            case >= 80:
                return SoilQuality.Excellent;
            case >= 50:
                return SoilQuality.Average;
            case >= 20:
                return SoilQuality.Poor;
            default:
                return SoilQuality.Unfarmable;
        }
    }
    public bool GoodEnoughSoil(SoilQuality requirement)
    {
        int minReq = SQToMinInt(requirement);
        return quality >= minReq;
    }

    public int SQToMinInt(SoilQuality quality)
    {
        switch (quality)
        {
            case SoilQuality.Excellent:
                return 80;
            case SoilQuality.Average:
                return 50;
            case SoilQuality.Poor:
                return 20;
            default:
                return 0;
        }
    }

    public void Turn()
    {
        UpdatePlotQuality(pendingOrganicMatter);
        plantingSpots.ForEach(x => x.ProcessTurn());

        int plants = plantingSpots.Count(spot => !spot.IsEmpty());

        plants = Mathf.Clamp(plants - 1, 0, int.MaxValue);
        UpdatePlotQuality(-plants - 1); // -1 by default for all land over time
    }

    public int Prio()
    {
        return 0;
    }
}