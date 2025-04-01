using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotSignActivator : Interactable
{
    SignUI UI;
    Plot parentPlot;
    PlayerController playerController;
    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        parentPlot = GetComponentInParent<Plot>();
        UI = FindObjectOfType<SignUI>();
    }
    public override void OnInteract(HELD playerHoldState, PlantData seedData)
    {
        base.OnInteract(playerHoldState, seedData);
        UI.gameObject.SetActive(true);
        playerController.SetPlayerControl(false);
        UI.UpdateData(parentPlot);
    }
}
