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
        if (data == null) Image.color = new Color(Image.color.r, Image.color.g, Image.color.b, 0);
        else Image.color = new Color(Image.color.r, Image.color.g, Image.color.b, 1);
        quantText.text = count.ToString();
    }
    public void ChangeHeldDefault()
    {
        ChangeHeld(null, 0);
    }
    public Sprite CurrentSprite()
    {
        return Image.sprite;
    }
}
    