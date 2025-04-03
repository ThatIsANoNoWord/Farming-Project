using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPlayerMove : Interactable
{
    public GameObject player;
    public Transform newPosition;
    public override void OnInteract(HELD playerHoldState, PlantData seedData)
    {
        player.transform.position = newPosition.position;
    }
}
