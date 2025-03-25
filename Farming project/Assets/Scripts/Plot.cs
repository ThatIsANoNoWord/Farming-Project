using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Plot : MonoBehaviour, ITurnable
{
    private SoilQuality plotQuality;
    [SerializeField] private int quality = 0;
    public Sprite[] qualitySprites;
    private SpriteRenderer spriteRenderer;
    private int pendingOrganicMatter;
    public bool active;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = transform.Find("Plot").GetComponent<SpriteRenderer>();

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
    }
    public int Prio()
    {
        return 0;
    }
}