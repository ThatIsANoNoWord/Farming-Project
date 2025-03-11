using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeldUI : MonoBehaviour
{
    public TextMeshProUGUI quantText;
    public Image Image;
    public void ChangeHeld(Sprite data, int count)
    {
        Image.sprite = data;
        quantText.text = count.ToString();
    }
}
