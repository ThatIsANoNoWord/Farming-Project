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

        if (!plotActive)
        {
            plantingSpots.ForEach(x => x.gameObject.GetComponent<Collider2D>().enabled = false);
        }

        // Randomize the starting quality of the land except for Plot #2
        // Randomize with a curve: higher values are more rare
        float randomValue = Mathf.Pow(Random.value, 6);
        quality = Mathf.FloorToInt(randomValue * 100);
        if (gameObject.name == "Plot-02") quality = 30;

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
    }

    public SoilQuality IntToSQ(int quality)
    {
        return quality switch
        {
            >= 80 => SoilQuality.Excellent,
            >= 50 => SoilQuality.Average,
            >= 20 => SoilQuality.Poor,
            _ => SoilQuality.Unfarmable,
        };
    }
    public bool GoodEnoughSoil(SoilQuality requirement)
    {
        return (int) requirement <= (int) plotQuality;
    }

    public bool EveryoneEmpty()
    {
        foreach(PlantingSpot planting in plantingSpots)
        {
            if (!planting.IsEmpty())
            {
                return false;
            }
        }
        return true;
    }

    public void Turn()
    {
        plantingSpots.ForEach(x => x.ProcessTurn());
        if (pendingOrganicMatter > 0 || !plotActive)
        {
            UpdatePlotQuality(pendingOrganicMatter);
        } else
        {
            int plants = plantingSpots.Count(spot => !spot.IsEmpty());

            plants = Mathf.Clamp(plants - 2, 0, int.MaxValue);
            UpdatePlotQuality((-plants * 3) - 2); // -1 by default for all land over time
        }
    }

    public int Prio()
    {
        return 0;
    }
}