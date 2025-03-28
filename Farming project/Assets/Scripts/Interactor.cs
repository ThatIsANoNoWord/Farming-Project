using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public GameObject interactIndicator;
    public float indicatorUpDist;
    PlayerController playerScript;
    Interactable[] interactables;
    List<GameObject> interactObjects;
    Interactable closest;
    float closestDistance;

    private void Start()
    {
        playerScript = GetComponentInParent<PlayerController>();
        interactables = FindObjectsByType<Interactable>(FindObjectsSortMode.None);
        interactObjects = new List<GameObject>();
        foreach (Interactable interactable in interactables)
        {
            interactObjects.Add(interactable.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Interactable>())
        {
            interactObjects.Add(collision.gameObject);
        }
        if (!interactObjects.Contains(collision.gameObject))
        {
            return;
        }
        if (closest == null)
        {
            closest = collision.gameObject.GetComponent<Interactable>();
            closest.OnPlayerClose();
            interactIndicator.SetActive(true);
            interactIndicator.transform.position = closest.transform.position + Vector3.up * indicatorUpDist;
            playerScript.ChangeClosestInteract(closest);
            return;
        }
        if (Vector2.Distance(gameObject.transform.position, collision.transform.position) < closestDistance)
        {
            closest.OnPlayerFar();
            closest = collision.gameObject.GetComponent<Interactable>();
            closestDistance = Vector2.Distance(gameObject.transform.position, collision.transform.position);
            closest.OnPlayerClose();
            interactIndicator.SetActive(true);
            interactIndicator.transform.position = closest.transform.position + Vector3.up * indicatorUpDist;
            playerScript.ChangeClosestInteract(closest);
            return;
        }
        closestDistance = Vector2.Distance(gameObject.transform.position, closest.transform.position);
        interactIndicator.SetActive(true);
        interactIndicator.transform.position = closest.transform.position + Vector3.up * indicatorUpDist;
        playerScript.ChangeClosestInteract(closest);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {   
        if (closest == null)
        {
            closest = null;
            closestDistance = 0;
            interactIndicator.SetActive(false);
            playerScript.ChangeClosestInteract(null);
            return;
        }
        if (closest.gameObject == null)
        {
            return;
        }
        if (collision.gameObject == null)
        {
            return;
        }
        if (collision.gameObject == closest.gameObject)
        {
            closest.OnPlayerFar();
            closest = null;
            closestDistance = 0;
            interactIndicator.SetActive(false);
            playerScript.ChangeClosestInteract(null);
        }
    }
}
