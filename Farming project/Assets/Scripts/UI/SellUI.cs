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
    List<PlantData> plantsToBeSold = new List<PlantData>();
    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        gameManager = FindObjectOfType<GameManager>();

        plantsToBeSold = new List<PlantData>();

        UpdateData();

        gameObject.SetActive(false);
    }
    public void AllocateCrop()
    {
        if (playerController.GetHeld() != HELD.CROP)
        {
            needToHoldCropWarning.Play("PopupText");
            return;
        }
        plantsToBeSold.Add(playerController.GetHeldPlantData());
        UpdateData();
        playerController.DecrementHeld();
    }
    public void ReturnControl()
    {
        playerController.SetPlayerControl(true);
        gameObject.SetActive(false);
    }
    public override void UpdateData()
    {
        cropAllocImage.sprite = plantsToBeSold.Count == 0 ? null : plantsToBeSold.ToArray()[plantsToBeSold.Count - 1].cropSprite;
        cropAllocImage.color = new Color(1, 1, 1, plantsToBeSold.Count == 0 ? 0 : 1);

        allocatedQuant.text = plantsToBeSold.Count.ToString();
    }

    public void Turn()
    {
        int gains = 0;
        if (plantsToBeSold.Count > 0)
        {
            plantsToBeSold.ForEach(x => gains += x.sellPrice);
            plantsToBeSold.RemoveAll(x => true);
        }
        gameManager.ChangeMoney(gains);
        UpdateData();
    }
    public int Prio()
    {
        return 0;
    }
}
