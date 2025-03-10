using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Plant : MonoBehaviour, ITurnable
{
    public Sprite[] growthStages;  // List of sprites corresponding to the growth stages
    public int produceCount;
    public int sellPrice;
    public int buySeedPrice;
    public SoilQuality requiredSoilQuality;
    public float growthTime;  // Turns it takes to grow to the next stage
    private int currentGrowthStage;
    private SpriteRenderer spriteRenderer;

    public void Awake()
    {
        
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (!spriteRenderer) spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        SetStage(0);
    }

    public void SetStage(int stage) {
        if (growthStages.Length == 0) return;

        currentGrowthStage = Mathf.Min(stage, growthStages.Length - 1);
        spriteRenderer.sprite = growthStages[currentGrowthStage];
    }

    public void Turn() {
        SetStage(currentGrowthStage + 1);
    }

    public bool isFullyGrown() {
        return currentGrowthStage == growthStages.Length - 1;
    }
}
