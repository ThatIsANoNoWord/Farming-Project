using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Collections;

public class CompostUI : UI, ITurnable
{
    public Sprite noSprite;
    public Image cropAllocImage;
    public TextMeshProUGUI allocatedQuant;
    public TextMeshProUGUI compostQuant;
    public Animator needToHoldCropWarning;
    PlayerController playerController;
    GameManager gameManager;
    int compostContained = 0;
    private Stack<PlantData> allocatedPlants = new();

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        gameManager = FindObjectOfType<GameManager>();

        UpdateData();

        gameObject.SetActive(false);
    }
    public void AllocateCrop()
    {
        if (playerController.GetHeld() == HELD.NOTHING) {
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

    public void HoldCompost()
    {
        if (playerController.GetHeld() != HELD.NOTHING && playerController.GetHeld() != HELD.COMPOST)
        {
            return;
        }
        if (compostContained == 0)
        {
            compostContained = playerController.GetHeldQuant();
            playerController.ChangeHeld(HELD.COMPOST, null, null, -playerController.GetHeldQuant());
            playerController.HoldNothing();
            UpdateData();
            return;
        }
        playerController.ChangeHeld(HELD.COMPOST, null, gameManager.compostSprite, compostContained);
        compostContained = 0;
        UpdateData();
    }
    public void ReturnControl()
    {
        playerController.SetPlayerControl(true);
        gameObject.SetActive(false);
    }
    public override void UpdateData()
    {
        if (allocatedPlants.Count > 0) UpdateData(allocatedPlants.Peek());
        else UpdateData(null);
    }
    public void UpdateData(PlantData plant)
    {
        cropAllocImage.sprite = plant != null ? plant.cropSprite : null;
        cropAllocImage.color = new Color(1, 1, 1, plant != null ? 1 : 0);

        allocatedQuant.text = allocatedPlants.Count.ToString();
        compostQuant.text = compostContained.ToString();
    }

    public void Turn()
    {
        compostContained = 0;
        while (allocatedPlants.Count > 0) {
            compostContained += allocatedPlants.Pop().compostCount;
        }

        UpdateData();
    }

    public int Prio()
    {
        return 0;
    }
}
