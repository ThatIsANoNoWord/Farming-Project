using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnEndInteractable : Interactable
{
    List<ITurnable> allTurnEnd;
    public AnimationClip screenAnim;
    public Animator screenAnimator;
    PlayerController playerController;

    private void Start()
    {
        allTurnEnd = new List<ITurnable>();
        var tempVar = FindObjectsOfType<MonoBehaviour>().OfType<ITurnable>();
        foreach (ITurnable temp in tempVar)
        {
            allTurnEnd.Add(temp);
        }
        playerController = FindObjectOfType<PlayerController>();
    }
    public override void OnInteract(HELD playerHoldState, PlantData seedData)
    {
        foreach(ITurnable runable in allTurnEnd)
        {
            runable.Turn();
            screenAnimator.Play("FadeAndReturn");
            playerController.SetPlayerControl(false);
        }
    }

    IEnumerator WaitAndGiveBack()
    {
        yield return new WaitForSeconds(screenAnim.length);
        playerController.SetPlayerControl(true);
    }
}
