using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SeedUI : MonoBehaviour
{
    public Sprite noSprite;
    public Image[] holdSeedImages;
    public Image[] holdSeedImages2;
    public Image[] holdPlantImages;
    public TextMeshProUGUI[] seedQuant;
    PlantData[] plantSeedList;
    PlayerController playerController;
    GameManager gameManager;
    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        gameManager = FindObjectOfType<GameManager>();
    }
    public void PurchaseSeed(int seedNum)
    {
        gameManager.TryPurchaseSeed(plantSeedList[seedNum]);
    }
    public void HoldSeed(int seedNum)
    {
        playerController.ChangeHeld(HELD.SEED, plantSeedList[seedNum]);
    }
    public void GiveSeedInfo(PlantData[] seedInfo)
    {
        plantSeedList = seedInfo;
    }
    public void ReturnControl()
    {
        playerController.SetPlayerControl(true);
        gameObject.SetActive(false);
    }

    public void UpdateData(Quant[] listToUpdate)
    {
        for(int i = 0; i < holdSeedImages.Length; i++)
        {
            if (i < listToUpdate.Length)
            {
                holdSeedImages[i].sprite = listToUpdate[i].plant.seedSprite;
                holdSeedImages2[i].sprite = listToUpdate[i].plant.seedSprite;
                holdPlantImages[i].sprite = listToUpdate[i].plant.growthStages[listToUpdate[i].plant.growthStages.Length - 1];
                seedQuant[i].text = listToUpdate[i].seedQuantity.ToString();
            }
            else
            {
                holdSeedImages[i].sprite = noSprite;
                holdSeedImages2[i].sprite = noSprite;
                holdPlantImages[i].sprite = noSprite;
                seedQuant[i].text = "0";
            }
        }
    }
}
