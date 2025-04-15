using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Collections;

public class SellUI : UI, ITurnable
{
    public Sprite noSprite;
    public Image cropAllocImage;
    public TextMeshProUGUI allocatedQuant;
    public Animator needToHoldCropWarning;
    PlayerController playerController;
    GameManager gameManager;
    private Stack<PlantData> allocatedPlants = new();
    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        gameManager = FindObjectOfType<GameManager>();

        UpdateData();

        gameObject.SetActive(false);
    }
    public void AllocateCrop()
    {
        // Remove from compost instead of put in
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
            PlantData plant = allocatedPlants.Pop();

            playerController.ChangeHeld(HELD.CROP, plant, plant.cropSprite, 1);
        } else {
            if (playerController.GetHeld() != HELD.CROP) needToHoldCropWarning.Play("PopupText");
            else {
                allocatedPlants.Push(playerController.GetHeldPlantData());
                playerController.DecrementHeld();
            }
        }
        
        UpdateData();
    }

    public void ReturnControl()
    {
        playerController.SetPlayerControl(true);
        gameObject.SetActive(false);
    }
    public override void UpdateData()
    {
        cropAllocImage.sprite = allocatedPlants.Count == 0 ? null : allocatedPlants.Peek().cropSprite;
        cropAllocImage.color = new Color(1, 1, 1, allocatedPlants.Count == 0 ? 0 : 1);

        allocatedQuant.text = allocatedPlants.Count.ToString();
    }


    public void Turn()
    {
        int profit = 0;
        while (allocatedPlants.Count > 0) {
            profit += allocatedPlants.Pop().sellPrice;
        }
        
        gameManager.ChangeMoney(profit);

        UpdateData();
    }

    public int Prio()
    {
        return 0;
    }
}
