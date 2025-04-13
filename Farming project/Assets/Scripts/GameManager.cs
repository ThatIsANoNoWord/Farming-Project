using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Xml.Linq;

public class GameManager : MonoBehaviour, ITurnable
{

    [SerializeField] int playerMoneyQuantity;
    int turnNumber;
    public Sprite compostSprite;
    public PlantData[] plantDataTypes;
    MoneyUI moneyUI;
    QuantList cropList;
    public GameObject gameOverUI;
    public TextMeshProUGUI taxText;
    public TextMeshProUGUI dayText;
    public int initialLoss;
    public GameObject compostPickupPrefab;
    public Vector2 trCompSpawn;
    public Vector2 blCompSpawn;
    public Animator winAnim;
    List<Plot> plots;
    int winCon;

    // Awake is called before anything
    void Awake()
    {
        // PlantDatabase.Initialize();
        cropList = new QuantList(plantDataTypes);
        cropList.IncreaseCap(plantDataTypes[0], 3);
        moneyUI = FindObjectOfType<MoneyUI>();
        plots = new List<Plot>();
        plots.AddRange(FindObjectsOfType<Plot>());
    }
    // Start is called before the first frame update
    void Start()
    {
        ChangeMoney(100000);
        turnNumber = 1;
        winCon = 0;
    }

    public void ChangeMoney(int newMoney)
    {
        playerMoneyQuantity += newMoney;
        moneyUI.DisplayMoney(playerMoneyQuantity);
        if(playerMoneyQuantity < 0)
        {
            gameOverUI.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public int GetMoney()
    {
        return playerMoneyQuantity;
    }

    public QuantList GetPlantQuant()
    {
        return cropList;
    }
    public PlantData[] GetSeedList()
    {
        return plantDataTypes;
    }

    public void DecrementSeed(PlantData plant)
    {
        cropList.DecreaseCurr(plant, 1);
    }

    // Tries to purchase the seed for the player.
    // Returns 0 on success and -1 on failure.
    public int TryPurchaseSeed(PlantData seedPurchased)
    {
        if (playerMoneyQuantity < seedPurchased.buySeedPrice)
        {
            return -1;
        }
        ChangeMoney(-seedPurchased.buySeedPrice);
        cropList.IncreaseCap(seedPurchased, 1);
        return 0;
    }

    public void Turn()
    {
        if(turnNumber % 6 == 0)
        {
            ChangeMoney(-initialLoss);
            initialLoss *= 2;
            taxText.gameObject.SetActive(false);
            bool allUnlocked = true;
            plots.ForEach(x => allUnlocked = allUnlocked && x.plotActive);
            if (allUnlocked) winCon++;
            if (winCon == 2)
            {
                winAnim.Play("YouWin");
            }
        }
        turnNumber++;
        if (turnNumber % 6 == 0)
        {
            taxText.text = "-$" + initialLoss;
            taxText.gameObject.SetActive(true);
        }

        int compQuant = Random.Range(5,9);
        for(int i = 0; i < compQuant; i++)
        {
            Vector2 newPos = new Vector2(Random.Range(blCompSpawn.x, trCompSpawn.x), Random.Range(blCompSpawn.y, trCompSpawn.y));
            GameObject droppedCrop = Instantiate(compostPickupPrefab, newPos, Quaternion.identity);
            droppedCrop.GetComponent<CompostPickupable>().Initial(1);
        }
        cropList.ResetValues();
        dayText.text = "Day " + turnNumber.ToString();
    }
    public int Prio()
    {
        return -1;
    }
}

public class QuantList
{
    readonly List<Quant> quantList;

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
    public void DecreaseCurr(PlantData plant, int minusCurr)
    {
        for (int i = 0; i < quantList.Count; i++)
        {
            if (quantList[i].MatchPlant(plant))
            {
                quantList[i].DecreaseCurr(minusCurr);
                return;
            }
        }
    }

    public int GetPlantCurrQuant(PlantData plant)
    {
        Quant target = quantList.Find(x => x.MatchPlant(plant));
        return target != null ? target.GetCurrQuant() : -1;
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
    public void DecreaseCurr(int minusCurr)
    {
        seedCurrQuantity -= minusCurr;
    }

    public bool MatchPlant(PlantData plant)
    {
        return plant.cropName.Equals(this.plant.cropName);
    }
}