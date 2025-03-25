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
    PlayerController playerController;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        growingSprite = gameObject.GetComponent<SpriteRenderer>();
        growingPlant = null;
        spotCurrState = SpotStates.Empty;
        parentPlot = GetComponentInParent<Plot>();
        currentGrowthStage = -1;
    }

    public override void OnInteract(HELD playerHoldState, PlantData seedData)
    {
        if (spotCurrState == SpotStates.Empty)
        {
            switch (playerHoldState)
            {
                case HELD.NOTHING:
                    // Do nothing
                    return;
                case HELD.SEED:
                    return;
                case HELD.COMPOST:
                    // Compost time
                    return;
                case HELD.CROP:
                    // Do nothing
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
        if (parentPlot.GetQuality() < (int)growingPlant.requiredSoilQuality)
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

    public int Prio()
    {
        return 0;
    }
}

enum SpotStates
{
    Empty, Growing, Harvestable
}