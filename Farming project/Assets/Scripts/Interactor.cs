using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    PlayerController playerScript;
    Interactable[] interactables;
    List<GameObject> interactObjects;
    Interactable closest;
    float closestDistance;

    private void Start()
    {
        playerScript = GetComponentInParent<PlayerController>();
        interactables = GetComponents<Interactable>();
        interactObjects = new List<GameObject>();
        foreach (Interactable interactable in interactables)
        {
            interactObjects.Add(interactable.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!interactObjects.Contains(collision.gameObject))
        {
            return;
        }
        if (Vector2.Distance(gameObject.transform.position, collision.transform.position) < closestDistance)
        {
            closest.OnPlayerFar();
            closest = gameObject.GetComponent<Interactable>();
            closestDistance = Vector2.Distance(gameObject.transform.position, collision.transform.position);
            closest.OnPlayerClose();
        }
        closestDistance = Vector2.Distance(gameObject.transform.position, closest.transform.position);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == closest.gameObject)
        {
            closest.OnPlayerFar();
            closest = null;
            closestDistance = 0;
        }
    }
}
