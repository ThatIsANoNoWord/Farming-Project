using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompostPickupable : Interactable
{
    public Sprite[] compostSprites;
    SpriteRenderer thisSpriteRenderer;
    GameManager gameManager;
    PlayerController playerController;
    void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        thisSpriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();
    }
    void Start()
    {
        thisSpriteRenderer.sprite = compostSprites[Random.Range(0, compostSprites.Length)];
    }
    public override void OnInteract(HELD playerHoldState, PlantData seedData)
    {
        playerController.ChangeHeld(HELD.COMPOST, null, gameManager.compostSprite, 1);
    }
}
