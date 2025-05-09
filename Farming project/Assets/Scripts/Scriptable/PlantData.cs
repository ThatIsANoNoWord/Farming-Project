using UnityEngine;

[CreateAssetMenu(fileName = "New Fruit")]
public class PlantData : ScriptableObject
{
    public string cropName;
    public Sprite seedSprite;
    public Sprite cropSprite;
    public Sprite[] growthStages;  // List of sprites corresponding to the growth stages
    // We can use the list of sprites to determine how many rounds to grow.
    public int produceCount;
    public int sellPrice;
    public int buySeedPrice;
    public int compostCount;
    public SoilQuality requiredSoilQuality;
    [TextArea]
    public string description;


    public override string ToString()
    {
        return cropName + ": sell for " + sellPrice + " and buys seeds for " + buySeedPrice;
        // return base.ToString();
    }
}
