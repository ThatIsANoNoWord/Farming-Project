using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class PlantDatabase
{
    // This dictionary will store the plant GameObjects by their plant name
    private static Dictionary<string, PlantData> allPlants = new();
    
    public static void Initialize()
    {
        // Load all plant data
        PlantData[] plants = Resources.LoadAll<PlantData>("Plants");

        // Turn data into gameobjects
        foreach (PlantData plant in plants)
        {
            Debug.Log("Loaded plant" + plant.name);
            allPlants.Add(plant.name, plant);
        }
    }
    
    // Get the specified plant data
    public static PlantData GetPlant(string plantName)
    {
        return allPlants.TryGetValue(plantName, out var plant) ? plant : null;
    }

    /// <summary>
    /// Get a list of all plant scripts.
    /// To use the gameobject, create a new instance of it using Instantiate(plant.gameObject);
    /// </summary>
    /// <returns>A list of all plants script types.</returns>
    public static PlantData[] GetAllPlants()
    {
        return new List<PlantData>(allPlants.Values).ToArray();
    }
}
