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
    [SerializeField] private Gradient barGradient;
    [SerializeField] private Slider qualitySlider;
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI costText;
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

        UpdatePlotQuality(0);
        UpdateSign();
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

    public void NoConuco()
    {
        if (pendingOrganicMatter == 0) return;
        moundObject.SetActive(false);
        pendingOrganicMatter = 0;
        plantingSpots.ForEach(x => x.EmptyPlot());
    }

    public void AttemptPurchase()
    {
        if (gameManager.GetMoney() >= buyPrice)
        {
            gameManager.ChangeMoney(-buyPrice);
            plotActive = true;
            inactiveObject.SetActive(false);
            UpdateSign();
        }
        else
        {
            // Make an error or smthn idk you do you
        }
    }

    public void UpdateSign()
    {
        if (plotActive)
        {
            costText.gameObject.SetActive(false);
            qualitySlider.gameObject.SetActive(true);
            qualitySlider.value = quality / 100f;
            fillImage.color = barGradient.Evaluate(quality / 100f);
        }
        else
        {
            costText.gameObject.SetActive(true);
            qualitySlider.gameObject.SetActive(false);
            costText.text = "Cost: \n$" + buyPrice.ToString();
        }
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
            case >= 40:
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
        UpdateSign();
    }
    public int Prio()
    {
        return 0;
    }
}