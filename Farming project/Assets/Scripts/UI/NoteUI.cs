using UnityEngine;

public class NoteUI : UI
{
    public GameObject mailboxUI;
    private PlayerController playerController;

    private bool isInitialMail = true;

    private void Start()
    {
        gameObject.SetActive(true); // Open on start so we can display the intro mail
        playerController = FindObjectOfType<PlayerController>();
        playerController.SetPlayerControl(false);
    }
    
    public void CloseNote()
    {
        gameObject.SetActive(false);
        if (isInitialMail) playerController.SetPlayerControl(true);
        else mailboxUI.SetActive(true);
        isInitialMail = false;
    }
}
