using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public void Turn()
    {
        UpdatePlotQuality(pendingOrganicMatter);
        plantingSpots.ForEach(x => x.ProcessTurn());
        int plants = 0;
        foreach (PlantingSpot planting in plantingSpots)
        {
            if (!planting.IsEmpty())
            {
                plants++;
            }
        }
        plants = Mathf.Clamp(plants - 2, 0, int.MaxValue);
        UpdatePlotQuality(-plants);
    }
    public int Prio()
    {
        return 0;
    }
}