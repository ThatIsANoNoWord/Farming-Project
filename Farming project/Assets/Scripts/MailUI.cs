using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MailUI : UI, ITurnable
{
    public TextMeshProUGUI mailContents;
    public GameObject interactor;
    PlayerController playerController;
    Queue<string> mailQueue;
    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        mailQueue = new Queue<string>();

        gameObject.SetActive(false);
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
    }
    public override void UpdateData()
    {
        if (mailQueue.Count == 0) return;
        mailContents.text = mailQueue.Dequeue();
        if (mailQueue.Count == 0) interactor.SetActive(false);
    }

    public void Turn()
    {
        mailQueue.Clear();
        interactor.SetActive(false);
    }

    public int Prio()
    {
        return 1;
    }
}
