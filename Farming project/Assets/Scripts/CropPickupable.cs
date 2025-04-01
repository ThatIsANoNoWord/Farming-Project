using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropPickupable : Interactable, ITurnable
{
    public Sprite noSprite;
    PlantData cropPlant;
    int cropCount;
    SpriteRenderer thisSpriteRenderer;
    PlayerController playerController;
    void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        thisSpriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        thisSpriteRenderer.sprite = cropPlant == null ? noSprite : cropPlant.cropSprite;
    }
    public void SetPlantType(PlantData plant, int count)
    {
        cropPlant = plant;
        cropCount = count;
        thisSpriteRenderer.sprite = plant.cropSprite;
        if (cropCount <= 0)
        {
            Destroy(gameObject);
        }
    }
    public override void OnInteract(HELD playerHoldState, PlantData seedData)
    {
        base.OnInteract(playerHoldState, seedData);
        playerController.ChangeHeld(HELD.CROP, cropPlant, cropPlant.cropSprite, cropCount);
        Destroy(gameObject);
    }

    public void Turn()
    {
        Destroy(gameObject);
    }

    public int Prio()
    {
        return 0;
    }
}
