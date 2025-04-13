using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SeedUI : UI
{
    public Sprite noSprite;
    public Image seedPurchImage;
    public Image cropPurchImage;
    public Image holdSeedImage;
    public Image holdCropImage;
    public TextMeshProUGUI seedQuant;
    public TextMeshProUGUI seedPrice;
    QuantList crops;
    PlantData[] plantSeedList;
    PlayerController playerController;
    GameManager gameManager;
    int currentSeedPurch = 0;
    int currentSeedHold = 0;
    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        gameManager = FindObjectOfType<GameManager>();

        crops = gameManager.GetPlantQuant();
        plantSeedList = gameManager.GetSeedList();
        UpdateData();

        gameObject.SetActive(false);
    }
    public void PurchaseSeed()
    {
        gameManager.TryPurchaseSeed(plantSeedList[currentSeedPurch]);
        UpdateData();
    }

    // Player holds nothing if they are already holding the same seed type
    // Otherwise hold the seed type
    public void HoldSeed()
    {
        if (crops.GetPlantCurrQuant(plantSeedList[currentSeedHold]) <= 0) return;
        if (playerController.GetHeld() == HELD.SEED && playerController.GetHeldPlantData() == plantSeedList[currentSeedHold].seedSprite)
        {
            playerController.HoldNothing();
            return;
        }
        playerController.ChangeHeld(HELD.SEED, plantSeedList[currentSeedHold], plantSeedList[currentSeedHold].seedSprite, crops.GetPlantCurrQuant(plantSeedList[currentSeedHold]));
    }
    public void ReturnControl()
    {
        playerController.SetPlayerControl(true);
        currentSeedPurch = 0;
        currentSeedHold = 0;
        UpdateData();
        gameObject.SetActive(false);
    }
    public void IncrementSeedPurch()
    {
        
        currentSeedPurch++;
        currentSeedPurch %= plantSeedList.Length;
        currentSeedPurch = currentSeedPurch < 0 ? currentSeedPurch + plantSeedList.Length : currentSeedPurch;

        UpdateData();
    }
    public void DecrementSeedPurch()
    {
        currentSeedPurch--;
        currentSeedPurch %= plantSeedList.Length;
        currentSeedPurch = currentSeedPurch < 0 ? currentSeedPurch + plantSeedList.Length : currentSeedPurch;
        UpdateData();
    }
    public void IncrementSeedHold()
    {
        int originalIndex = currentSeedHold;

        do {
            currentSeedHold = (currentSeedHold + 1) % plantSeedList.Length;
            if (crops.GetPlantCurrQuant(plantSeedList[currentSeedHold]) > 0)
                break;
        }
        while (currentSeedHold != originalIndex);

        UpdateData();
    }

    public void DecrementSeedHold()
    {
        int originalIndex = currentSeedHold;

        do {
            currentSeedHold = (currentSeedHold - 1) % plantSeedList.Length;
            if (crops.GetPlantCurrQuant(plantSeedList[currentSeedHold]) > 0)
                break;
        }
        while (currentSeedHold != originalIndex);

        UpdateData();
    }

    public override void UpdateData()
    {
        seedPurchImage.sprite = plantSeedList[currentSeedPurch].seedSprite;
        cropPurchImage.sprite = plantSeedList[currentSeedPurch].cropSprite;
        holdSeedImage.sprite = plantSeedList[currentSeedHold].seedSprite;
        holdCropImage.sprite = plantSeedList[currentSeedHold].cropSprite;

        seedPrice.text = "$" + plantSeedList[currentSeedPurch].buySeedPrice.ToString();
        seedQuant.text = crops.GetPlantCurrQuant(plantSeedList[currentSeedHold]).ToString();
    }
}
