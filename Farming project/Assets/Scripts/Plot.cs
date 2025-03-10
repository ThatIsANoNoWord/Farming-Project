using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Plot : MonoBehaviour, ITurnable
{
    private SoilQuality plotQuality;
    [SerializeField] private int quality = 0;
    public List<Sprite> qualitySprites = new();
    private SpriteRenderer spriteRenderer;
    private Transform[] spots;
    private List<Plant> plants = new();
    [SerializeField] private int pendingOrganicMatter;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = transform.Find("Plot").GetComponent<SpriteRenderer>();
        spots = GetComponentsInChildren<Transform>()
            .Where(child => child.gameObject.name.StartsWith("Spot"))
            .ToArray();

        foreach (Transform spot in spots)
        {
            spot.gameObject.SetActive(false);
        }

        UpdatePlotQuality(0);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.gameObject.CompareTag("Player")) return;

        if (plants.Count == 0)
        {
            
            Plant randomPlant = PlantDatabase.GetAllPlants()[Random.Range(0, PlantDatabase.GetAllPlants().Length)];
            PlantCrops(randomPlant);
        }
        else if (plants[0].isFullyGrown()) {
            HarvestCrops();
        }
        else {
            Turn();
            plants.ForEach(plant => plant.Turn());
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (!collider.gameObject.CompareTag("Player")) return;

        

        UpdatePlotQuality(3);
    }


    public void UpdatePlotQuality(int number)
    {
        quality = Mathf.Clamp(quality + number, 0, 100);

        SoilQuality newQuality = quality < 20 ? SoilQuality.Unfarmable :
                                 quality < 40 ? SoilQuality.Poor :
                                 quality < 80 ? SoilQuality.Average :
                                 SoilQuality.Excellent;

        int changeInQuality = newQuality - plotQuality;

        plotQuality = newQuality;
        spriteRenderer.sprite = qualitySprites[(int)plotQuality];

        // Handle spots
        int spotsChange = changeInQuality * 3;

        // Enable spots that are disabled, or disable those that are enabled based on spotsChange
        List<Transform> spotsToChange = spots
            .Where(spot => spotsChange > 0 ? !spot.gameObject.activeSelf : spot.gameObject.activeSelf)
            .ToList();

        for (int i = 0; i < Mathf.Abs(spotsChange); i++)
        {
            int randomIndex = Random.Range(0, spotsToChange.Count);
            spotsToChange[randomIndex].gameObject.SetActive(spotsChange > 0);
            spotsToChange.RemoveAt(randomIndex);
        }
    }

    public void HarvestCrops()
    {
        if (plants.Count == 0) return;

        if (!plants[0].isFullyGrown()) return;

        plants.ForEach(plant =>
        {
            PlayerInventory.AddItem(plant.name, plant.produceCount);
            Destroy(plant.gameObject);
        });

        plants.Clear();
    }

    /// <summary>
    /// Only works if there are currently no plants. Creates a plant object in each active spot and adds it to the plant list.
    /// </summary>
    /// <param name="plant">The type of crop to plant.</param>
    public void PlantCrops(Plant plant)
    {
        if (plants.Count != 0) return;

        foreach (Transform spot in spots)
        {
            if (!spot.gameObject.activeSelf) continue;

            Plant newPlant = Instantiate(plant, spot.position, spot.rotation);
            plants.Add(newPlant);
        }
    }

    public void Turn()
    {
        UpdatePlotQuality(pendingOrganicMatter);
        pendingOrganicMatter = 0;
    }
}