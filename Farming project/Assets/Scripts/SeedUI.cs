using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SeedUI : MonoBehaviour
{
    public Sprite noSprite;
    public Image[] holdSeedImages;
    public TextMeshProUGUI[] seedQuant;
    public GameObject purchasePrefab;
    public GameObject equipSeedPrefab;
    GameObject[] purchaseInstances;
    GameObject[] equipSeedInstances;
    PlantData[] plantSeedList;
    PlayerController playerController;
    GameManager gameManager;
    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        gameManager = FindObjectOfType<GameManager>();

        purchaseInstances = new GameObject[gameManager.GetPlantQuant()];
        equipSeedInstances = new GameObject[gameManager.GetPlantQuant()];
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

    public void UpdateData(QuantList listToUpdate)
    {
        for(int i = 0; i < holdSeedImages.Length; i++)
        {
            // Do stuff
        }
    }
}
