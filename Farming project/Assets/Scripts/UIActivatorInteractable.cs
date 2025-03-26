using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIActivatorInteractable : Interactable
{
    public GameObject seedUI;
    PlayerController playerController;
    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }
    public override void OnInteract(HELD playerHoldState, PlantData seedData)
    {
        seedUI.SetActive(true);
        playerController.SetPlayerControl(false);
        seedUI.GetComponent<UI>().UpdateData();
    }
}
