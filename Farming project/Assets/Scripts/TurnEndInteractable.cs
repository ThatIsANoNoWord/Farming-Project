using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnEndInteractable : Interactable
{
    List<ITurnable> allTurnEnd;
    public float waitTime;
    public Animator screenAnimator;
    PlayerController playerController;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }
    public override void OnInteract(HELD playerHoldState, PlantData seedData)
    {
        allTurnEnd = new List<ITurnable>();
        var tempVar = FindObjectsOfType<MonoBehaviour>(true).OfType<ITurnable>();
        foreach (ITurnable temp in tempVar)
        {
            allTurnEnd.Add(temp);
            Debug.Log(temp);
        }
        allTurnEnd.Sort((x, y) => x.Prio() - y.Prio());
        foreach (ITurnable runable in allTurnEnd)
        {
            runable.Turn();
        }
        screenAnimator.Play("FadeAndReturn");
        playerController.SetPlayerControl(false);
        StartCoroutine(WaitAndGiveBack());
    }

    IEnumerator WaitAndGiveBack()
    {
        yield return new WaitForSeconds(waitTime);
        playerController.SetPlayerControl(true);
    }
}
