using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnEndInteractable : Interactable
{
    List<ITurnable> allTurnEnd;
    public float waitWorkTime;
    public float waitTime;
    public float delayBeforeNextSleep;
    public Animator screenAnimator;
    PlayerController playerController;
    private DateTime sleepDelay = DateTime.Now;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }
    public override void OnInteract(HELD playerHoldState, PlantData seedData)
    {
        if (DateTime.Now < sleepDelay) return;
        sleepDelay = DateTime.Now.AddSeconds(delayBeforeNextSleep);

        base.OnInteract(playerHoldState, seedData);
        StartCoroutine(WaitAndWork());
        screenAnimator.Play("FadeAndReturn");
        playerController.SetPlayerControl(false);
        StartCoroutine(WaitAndGiveBack());
    }


    IEnumerator WaitAndWork()
    {
        yield return new WaitForSeconds(waitWorkTime);
        allTurnEnd = new List<ITurnable>();
        var tempVar = FindObjectsOfType<MonoBehaviour>(true).OfType<ITurnable>();
        foreach (ITurnable temp in tempVar)
        {
            allTurnEnd.Add(temp);
        }
        allTurnEnd.Sort((x, y) => y.Prio() - x.Prio());
        foreach (ITurnable runable in allTurnEnd)
        {
            runable.Turn();
        }
    }
    IEnumerator WaitAndGiveBack()
    {
        yield return new WaitForSeconds(waitTime);
        playerController.SetPlayerControl(true);
    }
}
