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
    QuantList cropList;
    public GameObject gameOverUI;

    // Awake is called before anything
    void Awake()
    {
        // PlantDatabase.Initialize();
        cropList = new QuantList(plantDataTypes);
        cropList.IncreaseCap(plantDataTypes[0], 3);
        Debug.Log("Yehaw");
        Debug.Log(cropList);
    }
    // Start is called before the first frame update
    void Start()
    {
        moneyUI = FindObjectOfType<MoneyUI>();
        seedUI = FindObjectOfType<SeedUI>();
        playerMoneyQuantity = 0;
        ChangeMoney(0);
        seedUI.GiveSeedInfo(plantDataTypes);
        turnNumber = 1;
        seedUI.UpdateData();
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

    public QuantList GetPlantQuant()
    {
        return cropList;
    }
    public PlantData[] GetSeedList()
    {
        return plantDataTypes;
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
        cropList.IncreaseCap(seedPurchased, 1);
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
    List<Quant> quantList;

    public QuantList(PlantData[] plantDatas)
    {
        quantList = new List<Quant>();
        for (int i = 0; i < plantDatas.Length; i++)
        {
            quantList.Add(new Quant(0, 0, plantDatas[i]));
        }
    }

    public void ResetValues()
    {
        quantList.ForEach(x => x.ResetCurr());
    }

    public void IncreaseCap(PlantData plant, int plusCap)
    {
        for (int i = 0; i < quantList.Count; i++)
        {
            if (quantList[i].MatchPlant(plant))
            {
                quantList[i].IncreaseCapacity(plusCap);
                return;
            }
        }
    }

    public int GetPlantCurrQuant(PlantData plant)
    {
        Quant target = quantList.Find(x => x.MatchPlant(plant));
        return target == null ? target.GetCurrQuant() : -1;
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
        Debug.Log(plant);
        Debug.Log(this.plant);
        return plant.cropName.Equals(this.plant.cropName);
    }
}