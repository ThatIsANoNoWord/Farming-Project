using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class CompendiumUI : UI
{
    public Sprite noSprite;
    public Image cropImage;
    public Image seedImage;
    public TextMeshProUGUI cropName;
    public TextMeshProUGUI harvestQuant;
    public TextMeshProUGUI harvestTime;
    public TextMeshProUGUI sellPrice;
    public TextMeshProUGUI seedPrice;
    public TextMeshProUGUI landQualityNeed;
    public TextMeshProUGUI description;
    PlayerController playerController;
    GameManager gameManager;
    PlantData[] plantSeedList;
    int index;
    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        gameManager = FindObjectOfType<GameManager>();

        plantSeedList = gameManager.GetSeedList();

        UpdateData();
        index = 0;

        gameObject.SetActive(false);
    }
    public void IncrementIndex()
    {
        index++;
        index %= plantSeedList.Length;
        index = index < 0 ? index + plantSeedList.Length : index;
        UpdateData();
    }
    public void DecrementIndex()
    {
        index--;
        index %= plantSeedList.Length;
        index = index < 0 ? index + plantSeedList.Length : index;
        UpdateData();
    }
    public void ReturnControl()
    {
        playerController.SetPlayerControl(true);
        index = 0;
        UpdateData();
        gameObject.SetActive(false);
    }
    public override void UpdateData()
    {
        cropImage.sprite = plantSeedList[index].seedSprite;
        seedImage.sprite = plantSeedList[index].cropSprite;

        harvestQuant.text = "Crops per Harvest: " + plantSeedList[index].produceCount.ToString() + (plantSeedList[index].produceCount == 1 ? " Crop." : " Crops.");
        harvestTime.text = "Days until Harvest: " + (plantSeedList[index].growthStages.Length - 1).ToString() + 
            ((plantSeedList[index].growthStages.Length - 1) == 1 ? " Day." : " Days.");
        seedPrice.text = "Seed Buy Price: $" + plantSeedList[index].buySeedPrice.ToString();
        sellPrice.text = "Crop Sell Price: $" + plantSeedList[index].sellPrice.ToString();
        cropName.text = plantSeedList[index].cropName;
        description.text = plantSeedList[index].description;

        string qualityText = "Land Requirement: ";
        switch (plantSeedList[index].requiredSoilQuality)
        {
            case SoilQuality.Unfarmable:
                qualityText += "Any Land.";
                break;
            case SoilQuality.Poor:
                qualityText += "Farmable Land.";
                break;
            case SoilQuality.Average:
                qualityText += "Average Land.";
                break;
            case SoilQuality.Excellent:
                qualityText += "Excellent Land.";
                break;
            default:
                qualityText += "UNKOWN.";
                break;
        }
        landQualityNeed.text = qualityText;
    }
}
