using UnityEngine;

[CreateAssetMenu(fileName = "New Mail")]
public class MailData : ScriptableObject
{
    [TextArea(1, 1)]
    public string title;
    [TextArea(13, 13)]
    public string bodyContent;
    [HideInInspector]
    public bool isRead;
    public int dayAppear;
}