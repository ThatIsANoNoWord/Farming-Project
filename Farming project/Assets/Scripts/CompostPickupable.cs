using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompostPickupable : Interactable, ITurnable
{
    public Sprite[] compostSprites;
    SpriteRenderer thisSpriteRenderer;
    GameManager gameManager;
    PlayerController playerController;
    int compostCount;
    void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        thisSpriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();
    }
    void Start()
    {
        thisSpriteRenderer.sprite = compostSprites[Random.Range(0, compostSprites.Length)];
        compostCount = 1;
    }
    public void Initial(int count)
    {
        compostCount = count;
        if(compostCount <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void InitialSpecial(int count)
    {
        compostCount = count;
        if (compostCount <= 0)
        {
            Destroy(gameObject);
        }
        thisSpriteRenderer.sprite = gameManager.compostSprite;
    }
    public override void OnInteract(HELD playerHoldState, PlantData seedData)
    {
        base.OnInteract(playerHoldState, seedData);
        playerController.ChangeHeld(HELD.COMPOST, null, gameManager.compostSprite, 1);
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
