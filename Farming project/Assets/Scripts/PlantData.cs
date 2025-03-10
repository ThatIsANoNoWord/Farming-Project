using UnityEngine;

[CreateAssetMenu(fileName = "New Fruit")]
public class PlantData : ScriptableObject
{
    public Sprite[] growthStages;  // List of sprites corresponding to the growth stages
    // We can use the list of sprites to determine how many rounds to grow.
    public int produceCount;
    public int sellPrice;
    public int buySeedPrice;
    public SoilQuality requiredSoilQuality;
}
