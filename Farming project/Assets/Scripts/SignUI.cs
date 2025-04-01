using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class SignUI : UI
{
    [SerializeField] private GameObject purchaseSub;
    [SerializeField] private GameObject informationSub;
    [SerializeField] private Gradient barGradient;
    [SerializeField] private Slider qualitySlider;
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI quantText;
    [SerializeField] private TextMeshProUGUI qualityText;
    [SerializeField] private TextMeshProUGUI costText;
    public Animator needToHoldCropWarning;
    PlayerController playerController;
    Plot currPlot;
    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();

        gameObject.SetActive(false);
    }
    public void ReturnControl()
    {
        playerController.SetPlayerControl(true);
        UpdateData();
        gameObject.SetActive(false);
    }
    public void Purchase()
    {
        int code = currPlot.AttemptPurchase();
        if (code > 0) ReturnControl();
        else needToHoldCropWarning.Play("PopupText");
    }
    public void UpdateData(Plot plot)
    {
        currPlot = plot;
        if (plot.plotActive)
        {
            purchaseSub.SetActive(false);
            informationSub.SetActive(true);
            quantText.text = "$" + plot.GetPending().ToString();
            qualitySlider.value = plot.QualityRatio();
            fillImage.color = barGradient.Evaluate(plot.QualityRatio());
            string text = "";
            switch (plot.IntToSQ(plot.GetQuality()))
            {
                case SoilQuality.Unfarmable:
                    text += "Unfarmable";
                    break;
                case SoilQuality.Poor:
                    text += "Poor";
                    break;
                case SoilQuality.Average:
                    text += "Average";
                    break;
                case SoilQuality.Excellent:
                    text += "Excellent";
                    break;
                default:
                    text += "UNKOWN";
                    break;
            }
            qualityText.text = text;
        }
        else
        {
            purchaseSub.SetActive(true);
            informationSub.SetActive(false);
            costText.text = "$" + plot.buyPrice.ToString();
        }
    }
}
