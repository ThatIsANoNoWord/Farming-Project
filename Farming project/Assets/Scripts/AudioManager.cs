using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    static AudioSource musicSource;
    static AudioSource sfxSource;
    static Dictionary<string, AudioClip> clips = new();

    public List<string> randomSoundNames = new List<string>();  // Add your sound names here
    public float randomSoundInterval = 7.5f;  // Interval in seconds to play random sound

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        musicSource = gameObject.AddComponent<AudioSource>(); // Background music stuff
        musicSource.loop = true;
        
        sfxSource = gameObject.AddComponent<AudioSource>(); // One time sounds
        
        var loadedClips = Resources.LoadAll<AudioClip>("Audio");
        foreach (var clip in loadedClips) {
            clips.Add(clip.name, clip);
        }

        // Random sounds lol
        InvokeRepeating(nameof(PlayRandomSound), randomSoundInterval, randomSoundInterval);
    }

    void PlayRandomSound()
    {
        // Pick a random sound from the list
        if (randomSoundNames.Count > 0) {
            int randomIndex = Random.Range(0, randomSoundNames.Count);
            string randomSound = randomSoundNames[randomIndex];
            
            PlaySFX(randomSound, 0.1f);
        }
    }

    public static void PlayMusic(string name)
    {
        if (clips.TryGetValue(name, out var clip))
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    public static void StopMusic()
    {
        if (musicSource != null) musicSource.Stop();
    }

    public static void PlaySFX(string name, float volume = 1f, float pitch = 1f)
    {
        if (clips.TryGetValue(name, out var clip))
        {
            sfxSource.pitch = Mathf.Clamp(pitch, -3f, 3f); // Unity supports pitch between -3 and 3
            sfxSource.PlayOneShot(clip, Mathf.Clamp01(volume));
        }
    }

    public static bool IsMusicPlaying()
    {
        return musicSource != null && musicSource.isPlaying;
    }

    public static void SetMusicVolume(float volume)
    {
        if (musicSource != null)
            musicSource.volume = Mathf.Clamp01(volume); // keeps it between 0 and 1
    }
}
