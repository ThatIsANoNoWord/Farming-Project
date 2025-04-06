using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MailUI : UI, ITurnable
{
    public TextMeshProUGUI mailContents;
    public GameObject interactor;
    public Sprite mailPresent;
    public Sprite mailAbsent;
    PlayerController playerController;
    Queue<string> mailQueue;
    Collider2D interactorCollider;
    SpriteRenderer interactorRenderer;
    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        mailQueue = new Queue<string>();

        gameObject.SetActive(false);
        interactorCollider = interactor.GetComponent<Collider2D>();
        interactorRenderer = interactor.GetComponent<SpriteRenderer>();
    }
    public void ReturnControl()
    {
        playerController.SetPlayerControl(true);
        UpdateData();
        gameObject.SetActive(false);
    }
    public void QueueNewLetter(string content)
    {
        mailQueue.Enqueue(content);
        interactor.SetActive(true);
        interactorCollider.enabled = true;
        interactorRenderer.sprite = mailPresent;
    }
    public override void UpdateData()
    {
        if (mailQueue.Count == 0) return;
        mailContents.text = mailQueue.Dequeue();
        if (mailQueue.Count == 0)
        {
            interactorCollider.enabled = false;
            interactorRenderer.sprite = mailAbsent;
        }
    }

    public void Turn()
    {
        mailQueue.Clear();
        interactorCollider.enabled = false;
        interactorRenderer.sprite = mailAbsent;
    }

    public int Prio()
    {
        return 1;
    }
}
