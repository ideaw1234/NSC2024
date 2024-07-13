using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer instance = null;
    private AudioSource audioSource;
    public AudioMixer mainaudioMixer;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 0.75f); // Default to 0.75 if no value is saved
    }

    void Start()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
