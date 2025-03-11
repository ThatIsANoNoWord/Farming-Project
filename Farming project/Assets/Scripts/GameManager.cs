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
    Quant[] amountOfSeeds;
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
        amountOfSeeds = new Quant[plantDataTypes.Length];
        for (int i = 0; i < plantDataTypes.Length; i++)
        {
            amountOfSeeds[i] = new Quant(0, 0, plantDataTypes[i]);
        }
        amountOfSeeds[0].seedQuantity = 6;
        turnNumber = 1;
        seedUI.GiveSeedInfo(plantDataTypes);
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

    public int TryPurchaseSeed(PlantData seedPurchased)
    {
        if (playerMoneyQuantity < seedPurchased.buySeedPrice)
        {
            return -1;
        }
        playerMoneyQuantity -= seedPurchased.buySeedPrice;
        foreach (Quant stuff in amountOfSeeds)
        {
            if (stuff.plant == seedPurchased)
            {
                stuff.seedQuantity++;
                seedUI.UpdateData(amountOfSeeds);
                return 0;
            }
        }


        return -1;
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

public class Quant
{
    public int seedQuantity;
    public int cropQuantity;
    public PlantData plant;

    public Quant(int quant, int cropQuant, PlantData plant)
    {
        seedQuantity = quant;
        cropQuantity = cropQuant;
        this.plant = plant;
    }
}