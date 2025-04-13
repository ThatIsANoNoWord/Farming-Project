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
        cropList.IncreaseQuantity(plantDataTypes[0], 3);
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
        cropList.DecreaseQuantity(plant, 1);
    }

    public void IncrementSeed(PlantData plant)
    {
        cropList.IncreaseQuantity(plant, 1);
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
        cropList.IncreaseQuantity(seedPurchased, 1);
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

        dayText.text = "Day " + turnNumber.ToString();
    }
    public int Prio()
    {
        return -1;
    }
}

public class QuantList
{
    readonly Dictionary<PlantData, int> plantQuantities;

    public QuantList(PlantData[] plantDatas)
    {
        plantQuantities = new Dictionary<PlantData, int>();
        foreach (var plant in plantDatas)
        {
            plantQuantities[plant] = 0;
        }
    }

    public void DecreaseQuantity(PlantData plant, int minusCurr)
    {
        if (plantQuantities.ContainsKey(plant))
        {
            plantQuantities[plant] = Mathf.Max(plantQuantities[plant] - minusCurr, 0);
        }
    }

    public int GetPlantCurrQuant(PlantData plant)
    {
        return plantQuantities.TryGetValue(plant, out int quantity) ? quantity : -1;
    }

    public void IncreaseQuantity(PlantData plant, int plusCurr)
    {
        if (plantQuantities.ContainsKey(plant))
        {
            plantQuantities[plant] += plusCurr;
        }
    }
}
