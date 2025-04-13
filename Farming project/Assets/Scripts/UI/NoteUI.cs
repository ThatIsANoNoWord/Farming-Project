using UnityEngine;

public class NoteUI : UI
{
    public GameObject mailboxUI;

    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void CloseNote()
    {
        gameObject.SetActive(false);
        mailboxUI.SetActive(true);
    }
}
