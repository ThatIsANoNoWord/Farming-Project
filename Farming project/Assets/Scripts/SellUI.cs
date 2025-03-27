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
    List<PlantData> plantsToBeSold;
    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        gameManager = FindObjectOfType<GameManager>();

        plantsToBeSold = new List<PlantData>();

        UpdateData();
    }
    public void AllocateCrop()
    {
        if (playerController.GetHeld() != HELD.CROP)
        {
            needToHoldCropWarning.Play("PopupText");
            return;
        }
        plantsToBeSold.Add(playerController.GetHeldPlantData());
        UpdateData(playerController.GetHeldPlantData());
        playerController.DecrementHeld();
        Debug.Log(plantsToBeSold);
    }
    public void ReturnControl()
    {
        playerController.SetPlayerControl(true);
        gameObject.SetActive(false);
    }
    public override void UpdateData()
    {
        UpdateData(null);
    }
    public void UpdateData(PlantData plant)
    {
        cropAllocImage.sprite = plant != null ? plant.cropSprite : null;
        cropAllocImage.color = new Color(1, 1, 1, plant != null ? 1 : 0);

        allocatedQuant.text = plantsToBeSold.Count.ToString();
    }

    public void Turn()
    {
        int gains = 0;
        plantsToBeSold.ForEach(x => gains += x.sellPrice);
        plantsToBeSold.RemoveAll(x => true);
        gameManager.ChangeMoney(gains);
        UpdateData();
    }
    public int Prio()
    {
        return 0;
    }
}
