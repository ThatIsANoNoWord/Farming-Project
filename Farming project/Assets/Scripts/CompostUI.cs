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
    int allocated = 0;
    int compostContained = 0;
    PlantData lastPlant;
    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        gameManager = FindObjectOfType<GameManager>();

        lastPlant = null;
        UpdateData();

        gameObject.SetActive(false);
    }
    public void AllocateCrop()
    {
        if(playerController.GetHeld() != HELD.CROP)
        {
            needToHoldCropWarning.Play("PopupText");
            return;
        }
        allocated++;
        lastPlant = playerController.GetHeldPlantData();
        UpdateData();
        playerController.DecrementHeld();
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
            playerController.ChangeHeld(HELD.NOTHING, null, null, 0, false);
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
        UpdateData(lastPlant);
    }
    public void UpdateData(PlantData plant)
    {
        cropAllocImage.sprite = plant != null ? plant.cropSprite : null;
        cropAllocImage.color = new Color(1, 1, 1, plant != null ? 1 : 0);

        allocatedQuant.text = allocated.ToString();
        compostQuant.text = compostContained.ToString();
    }

    public void Turn()
    {
        compostContained = allocated * 3;
        allocated = 0;
        lastPlant = null;
        UpdateData();
    }
    public int Prio()
    {
        return 0;
    }
}
