using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantingSpot : Interactable
{
    public Sprite emptyPlantableSprite;
    public Animator warnAnimator;
    SpriteRenderer growingSprite;
    PlantData growingPlant;
    SpotStates spotCurrState;
    Plot parentPlot;
    int currentGrowthStage;
    PlayerController playerController;
    GameManager gameManager;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        growingSprite = gameObject.GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();
        growingPlant = null;
        spotCurrState = SpotStates.Empty;
        parentPlot = GetComponentInParent<Plot>();
        currentGrowthStage = -1;
    }

    public override void OnInteract(HELD playerHoldState, PlantData seedData)
    {
        base.OnInteract(playerHoldState, seedData);
        if (!parentPlot.plotActive)
        {
            parentPlot.AttemptPurchase();
            return;
        }

        if (spotCurrState == SpotStates.Empty)
        {
            switch (playerHoldState)
            {
                case HELD.NOTHING:
                    // Do nothing
                    return;
                case HELD.SEED:
                    if (!parentPlot.GoodEnoughSoil(playerController.GetHeldPlantData().requiredSoilQuality)) return;
                    SeedPlanted(playerController.GetHeldPlantData());
                    playerController.DecrementHeld();
                    return;
                case HELD.COMPOST:
                    if (!parentPlot.EveryoneEmpty())
                    {
                        // Warning
                        return;
                    }
                    parentPlot.MakeConuco(1);
                    playerController.DecrementHeld();
                    return;
                case HELD.CROP:
                    // Do nothing
                    return;
                default:
                    Debug.LogError("Impossible to reach here");
                    return;
            }
        }
        if (spotCurrState == SpotStates.Harvestable)
        {
            Harvest();
            return;
        }
        if (spotCurrState == SpotStates.Growing && playerHoldState == HELD.SEED)
        {
            return;
        }
        if (spotCurrState == SpotStates.Composting && playerHoldState == HELD.SEED)
        {
            // If the soil is not high enough quality for the plant, do nothing.
            if (!parentPlot.GoodEnoughSoil(playerController.GetHeldPlantData().requiredSoilQuality)) return;

            SeedPlanted(playerController.GetHeldPlantData());
            playerController.DecrementHeld();
            return;
        }
        if (playerHoldState == HELD.COMPOST)
        {
            parentPlot.MakeConuco(1);
            AudioManager.PlaySFX("Dig", 0.1f);

            playerController.DecrementHeld();
            return;
        }
    }

    void SeedPlanted(PlantData plant)
    {
        AudioManager.PlaySFX("Plant", 0.1f);
        parentPlot.NoConuco();
        spotCurrState = SpotStates.Growing;
        growingPlant = plant;
        currentGrowthStage = 0;
        growingSprite.sprite = plant.growthStages[currentGrowthStage];
        gameManager.DecrementSeed(plant);
    }

    public void Harvest()
    {
        AudioManager.PlaySFX("Dig", 0.1f);
        playerController.ChangeHeld(HELD.CROP, growingPlant, growingPlant.cropSprite, growingPlant.produceCount);
        gameManager.IncrementSeed(growingPlant);
        EmptyPlot();

        parentPlot.UpdatePlotQuality(0);
    }

    public void EmptyPlot()
    {
        growingSprite.sprite = emptyPlantableSprite;
        growingPlant = null;
        spotCurrState = SpotStates.Empty;
    }

    void GrowPlant()
    {
        if (!parentPlot.GoodEnoughSoil(growingPlant.requiredSoilQuality))
        {
            EmptyPlot();
            return;
        }
        currentGrowthStage++;
        growingSprite.sprite = growingPlant.growthStages[currentGrowthStage];
        if (currentGrowthStage == growingPlant.growthStages.Length - 1)
        {
            spotCurrState = SpotStates.Harvestable;
        }
    }

    public void CompostMode()
    {
        growingSprite.sprite = null;
        growingPlant = null;
        spotCurrState = SpotStates.Composting;
    }

    public void ProcessTurn()
    {
        switch (spotCurrState)
        {
            case SpotStates.Empty:
                return;
            case SpotStates.Composting:
                return;
            case SpotStates.Growing:
                GrowPlant();
                return;
            case SpotStates.Harvestable:
                EmptyPlot();
                parentPlot.UpdatePlotQuality(5); // Deceased plants are actually good for the soil! Rip seeds though :)
                return;
            default:
                Debug.LogError("Impossible to reach here");
                return;
        }
    }

    public bool IsEmpty()
    {
        return spotCurrState == SpotStates.Empty || spotCurrState == SpotStates.Composting;
    }
}

enum SpotStates
{
    Empty, Growing, Harvestable, Composting
}