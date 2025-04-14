using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : Interactable
{
    public void Start()
    {
        AudioManager.PlayMusic("Background");
        AudioManager.SetMusicVolume(0.25f);
    }

    public override void OnInteract(HELD playerHoldState, PlantData seedData) {
        if (AudioManager.IsMusicPlaying()) AudioManager.StopMusic();
        else AudioManager.PlayMusic("Background");
        
    }
}
