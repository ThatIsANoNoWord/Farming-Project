using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEndInteractable : Interactable
{
    List<ITurnable> allTurnEnd;

    private void Start()
    {
        allTurnEnd.AddRange(GetComponents<ITurnable>());
    }
    public override void OnInteract(HELD playerHoldState, PlantData seedData)
    {
        foreach(ITurnable runable in allTurnEnd)
        {
            runable.Turn();
        }
    }
}
