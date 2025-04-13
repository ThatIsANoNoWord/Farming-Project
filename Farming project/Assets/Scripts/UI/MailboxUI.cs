using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;
using System;


public class MailboxUI : UI, ITurnable 
{
    public GameObject buttonTemplate; // Assign the original button from the Content in the inspector
    public Transform contentParent;   // The Content object inside the ScrollView
    private int currentTurn = 1;
    private PlayerController playerController;
    public SpriteRenderer interactorRenderer;
    public Sprite newMail;
    public Sprite noNewMail;
    private Dictionary<MailData, GameObject> mailButtonMap = new();
    public GameObject noteUI;


    void Start() {
        LoadAllMail();

        gameObject.SetActive(false);

        playerController = FindObjectOfType<PlayerController>();
    }

    public void CloseMailbox() {
        playerController.SetPlayerControl(true);
        gameObject.SetActive(false);

        interactorRenderer.sprite = UnopenedMailExists() ? newMail : noNewMail;
    }

    void OpenMail(MailData mail, GameObject buttonObj) {
        // Change mail to be "read"
        Button button = buttonObj.GetComponent<Button>();

        ColorBlock colors = button.colors;
        colors.normalColor = Color.gray;
        button.colors = colors;
        mail.isRead = true;

        // Set the note text and open the note
        Transform compendiumPageTransform = noteUI.transform.Find("Compendium Page");
        TextMeshProUGUI noteText = compendiumPageTransform.GetComponentInChildren<TextMeshProUGUI>();

        noteText.text = mail.bodyContent;

        // Enable the NoteUI so it becomes visible
        gameObject.SetActive(false);
        noteUI.SetActive(true);
    }


    void LoadAllMail() {
        MailData[] mailArray = Resources.LoadAll<MailData>("Mail");

        // Sort mail by dayAppear in ascending order
        System.Array.Sort(mailArray, (a, b) => b.dayAppear.CompareTo(a.dayAppear));

        foreach (var mail in mailArray) {
            mail.isRead = false;
            GameObject buttonObj = Instantiate(buttonTemplate, contentParent);
            buttonObj.SetActive(mail.dayAppear == currentTurn);

            TextMeshProUGUI text = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            text.text = mail.title;

            mailButtonMap.Add(mail, buttonObj);

            buttonObj.GetComponent<Button>().onClick.AddListener(() => OpenMail(mail, buttonObj));
        }
    }

    bool UnopenedMailExists() {
        return mailButtonMap.Any(pair => !pair.Key.isRead && pair.Value.activeSelf);
    }

    void DisplayNewMail() {
        foreach (var pair in mailButtonMap) {
            MailData mail = pair.Key;
            GameObject buttonObj = pair.Value;

            // Check if the mail is set to appear on the current turn and if it's not already visible
            if (mail.dayAppear == currentTurn && !buttonObj.activeSelf) {
                buttonObj.SetActive(true);
                interactorRenderer.sprite = UnopenedMailExists() ? newMail : noNewMail;
            }
        }
    }

    public void Turn()
    {
        currentTurn++;
        DisplayNewMail();
    }

    public int Prio()
    {
        return 1;
    }
}
