using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NoteUI : UI
{
    public TextMeshProUGUI mailContents;
    PlayerController playerController;
    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();

        gameObject.SetActive(false);
    }
    public void ReturnControl()
    {
        playerController.SetPlayerControl(true);
        UpdateData();
        gameObject.SetActive(false);
    }
    public override void UpdateData()
    {

    }
}
