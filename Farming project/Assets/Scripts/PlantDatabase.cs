using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class PlantDatabase
{
    // This dictionary will store the plant GameObjects by their plant name
    private static Dictionary<string, Plant> allPlants = new();

    public static void Initialize()
    {
        // Load all plant data
        Plant[] plants = Resources.LoadAll<Plant>("Plants");

        // Turn data into gameobjects
        foreach (Plant plant in plants)
        {
            Debug.Log("Loaded plant" + plant.name);
            allPlants.Add(plant.name, plant);
        }
    }

    // Get the GameObject for the specified plant
    public static Plant GetPlant(string plantName)
    {
        return allPlants.TryGetValue(plantName, out var plantGO) ? plantGO : null;
    }

    /// <summary>
    /// Get a list of all plant scripts.
    /// To use the gameobject, create a new instance of it using Instantiate(plant.gameObject);
    /// </summary>
    /// <returns>A list of all plants script types.</returns>
    public static Plant[] GetAllPlants()
    {
        return new List<Plant>(allPlants.Values).ToArray();
    }
}
