using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SeedUI : MonoBehaviour
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
    }
    public void PurchaseSeed()
    {
        gameManager.TryPurchaseSeed(plantSeedList[currentSeedPurch]);
        UpdateData();
    }
    public void HoldSeed()
    {
        playerController.ChangeHeld(HELD.SEED, plantSeedList[currentSeedHold]);
    }
    public void GiveSeedInfo(PlantData[] seedInfo)
    {
        plantSeedList = seedInfo;
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
        UpdateData();
    }
    public void DecrementSeedPurch()
    {
        currentSeedPurch--;
        currentSeedPurch %= plantSeedList.Length;
        UpdateData();
    }
    public void IncrementSeedHold()
    {
        currentSeedHold++;
        currentSeedHold %= plantSeedList.Length;
        UpdateData();
    }
    public void DecrementSeedHold()
    {
        currentSeedHold--;
        currentSeedHold %= plantSeedList.Length;
        UpdateData();
    }
    public void UpdateData()
    {
        seedPurchImage.sprite = plantSeedList[currentSeedPurch].seedSprite;
        cropPurchImage.sprite = plantSeedList[currentSeedPurch].cropSprite;
        holdSeedImage.sprite = plantSeedList[currentSeedHold].seedSprite;
        holdCropImage.sprite = plantSeedList[currentSeedHold].cropSprite;

        seedPrice.text = "$" + plantSeedList[currentSeedPurch].buySeedPrice.ToString();
        Debug.Log(crops.GetPlantCurrQuant(plantSeedList[currentSeedHold]));
        seedQuant.text = crops.GetPlantCurrQuant(plantSeedList[currentSeedHold]).ToString();
    }
}
