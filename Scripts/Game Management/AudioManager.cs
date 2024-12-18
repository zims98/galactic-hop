using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds) // Stores audiosource variables for each sound in array
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    #region AudioControls
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name); // Checks the array for the requested sound name
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found.");
            return;
        }

        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name); // Checks the array for the requested sound name
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found.");
            return;
        }

        s.source.Stop();
    }
    #endregion

    #region AudioCallings
    public static void PlayStatic(string name)
    {
        if (instance == null)
        {
            Debug.LogWarning("No Audio Manager Found!");
            return;
        }

        instance.Play(name);
    }

    public static void StopStatic(string name)
    {
        if (instance == null)
        {
            Debug.LogWarning("No Audio Manager Found!");
            return;
        }

        instance.Stop(name);
    }
    #endregion
}