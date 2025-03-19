using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, ITurnable
{

    int playerMoneyQuantity;
    int turnNumber;
    public PlantData[] plantDataTypes;
    MoneyUI moneyUI;
    SeedUI seedUI;
    QuantList amountOfSeeds;
    public GameObject gameOverUI;

    // Awake is called before anything
    void Awake()
    {
        // PlantDatabase.Initialize();
    }
    // Start is called before the first frame update
    void Start()
    {
        moneyUI = FindObjectOfType<MoneyUI>();
        seedUI = FindObjectOfType<SeedUI>();
        playerMoneyQuantity = 0;
        ChangeMoney(0);
        seedUI.GiveSeedInfo(plantDataTypes);
        amountOfSeeds = new QuantList(plantDataTypes);
        amountOfSeeds.IncreaseCap(plantDataTypes[0], 3);
        turnNumber = 1;
        seedUI.UpdateData(amountOfSeeds);
    }

    public void ChangeMoney(int newMoney)
    {
        playerMoneyQuantity = newMoney;
        moneyUI.DisplayMoney(playerMoneyQuantity);
        if(playerMoneyQuantity < 0)
        {
            gameOverUI.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public int GetPlantQuant()
    {
        return plantDataTypes.Length;
    }

    // Tries to purchase the seed for the player.
    // Returns 0 on success and -1 on failure.
    public int TryPurchaseSeed(PlantData seedPurchased)
    {
        if (playerMoneyQuantity < seedPurchased.buySeedPrice)
        {
            return -1;
        }
        playerMoneyQuantity -= seedPurchased.buySeedPrice;
        amountOfSeeds.IncreaseCap(seedPurchased, 1);
        seedUI.UpdateData(amountOfSeeds);
        return 0;
    }

    public void Turn()
    {
        turnNumber++;
        if(turnNumber % 6 == 1)
        {
            // Lose some money
        }
    }
}

public class QuantList
{
    Quant[] quantList;

    public QuantList(PlantData[] plantDatas)
    {
        quantList = new Quant[plantDatas.Length];
        for (int i = 0; i < quantList.Length; i++)
        {
            quantList[i] = new Quant(0, 0, plantDatas[i]);
        }
    }

    public void ResetValues()
    {
        for (int i = 0; i < quantList.Length; i++)
        {
            quantList[i].ResetCurr();
        }
    }

    public void IncreaseCap(PlantData plant, int plusCap)
    {
        for (int i = 0; i < quantList.Length; i++)
        {
            if (quantList[i].MatchPlant(plant))
            {
                quantList[i].IncreaseCapacity(plusCap);
                return;
            }
        }
    }
}

class Quant
{
    int seedTotQuantity;
    int seedCurrQuantity;
    int cropQuantity;
    PlantData plant;

    public Quant(int quant, int cropQuant, PlantData plant)
    {
        seedTotQuantity = quant;
        seedCurrQuantity = quant;
        cropQuantity = cropQuant;
        this.plant = plant;
    }

    public void ResetCurr()
    {
        seedCurrQuantity = seedTotQuantity;
    }

    public int GetCurrQuant()
    {
        return seedCurrQuantity;
    }

    public void IncreaseCapacity(int plusCapacity)
    {
        seedTotQuantity += plusCapacity;
        seedCurrQuantity += plusCapacity;
    }

    public bool MatchPlant(PlantData plant)
    {
        return plant == this.plant;
    }
}