using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantingSpot : Interactable, ITurnable
{
    public static Sprite emptyPlantableSprite;
    SpriteRenderer growingSprite;
    PlantData growingPlant;
    SpotStates spotCurrState;
    Plot parentPlot;
    int currentGrowthStage;

    void Start()
    {
        growingSprite = gameObject.GetComponent<SpriteRenderer>();
        growingPlant = null;
        spotCurrState = SpotStates.Empty;
        parentPlot = GetComponentInParent<Plot>();
        currentGrowthStage = -1;
    }

    public override void OnInteract(HELD playerHoldState, PlantData seedData)
    {
        if (playerHoldState == HELD.COMPOST)
        {
            // That's stuff with the plot. Call the plot.
        }
        if (spotCurrState == SpotStates.Empty)
        {
            switch (playerHoldState)
            {
                case HELD.NOTHING:
                    // Make a message that the player needs seeds to plant
                    return;
                case HELD.SEED:
                    return;
                default:
                    Debug.LogError("Impossible to reach here");
                    return;
            }
        }
    }

    void SeedPlanted(PlantData plant)
    {
        spotCurrState = SpotStates.Growing;
        growingPlant = plant;
        currentGrowthStage = 0;
        growingSprite.sprite = plant.growthStages[currentGrowthStage];
    }

    void EmptyPlot()
    {
        growingSprite.sprite = emptyPlantableSprite;
        growingPlant = null;
        spotCurrState = SpotStates.Empty;
    }

    void GrowPlant()
    {
        if ((int)parentPlot.GetQuality() < (int)growingPlant.requiredSoilQuality)
        {
            EmptyPlot();
            return;
        }
        currentGrowthStage++;
        if (currentGrowthStage == growingPlant.growthStages.Length - 1)
        {

        }
    }

    public void Turn()
    {
        switch (spotCurrState)
        {
            case SpotStates.Empty:
                return;
            case SpotStates.Growing:
                GrowPlant();
                return;
            case SpotStates.Harvestable:
                EmptyPlot();
                return;
            default:
                Debug.LogError("Impossible to reach here");
                return;
        }
    }
}

enum SpotStates
{
    Empty, Growing, Harvestable
}