using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class SoilBookUI : UI
{
    public GameObject[] pages;
    PlayerController playerController;
    int index;
    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();

        UpdateData();
        index = 0;

        gameObject.SetActive(false);
    }
    public void IncrementIndex()
    {
        index++;
        index %= pages.Length;
        index = index < 0 ? index + pages.Length : index;
        UpdateData();
    }
    public void DecrementIndex()
    {
        index--;
        index %= pages.Length;
        index = index < 0 ? index + pages.Length : index;
        UpdateData();
    }
    public void ReturnControl()
    {
        playerController.SetPlayerControl(true);
        index = 0;
        UpdateData();
        gameObject.SetActive(false);
    }
    public override void UpdateData()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(index == i);
        }
    }
}
