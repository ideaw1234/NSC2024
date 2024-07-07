using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class AudioVolume : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private TextMeshProUGUI volumeText;
    [SerializeField] private AudioMixMode audioMixMode;
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        float initialVolume = 0f;

        switch (audioMixMode)
        {
            case AudioMixMode.LinearAudioSourceVolume:
                initialVolume = audioSource.volume;
                break;
            case AudioMixMode.LinearMixerVolume:
            case AudioMixMode.LogarithmicMixerVolume:
                audioMixer.GetFloat("Volume", out float mixerVolume);
                initialVolume = audioMixMode == AudioMixMode.LinearMixerVolume ?
                    (mixerVolume + 80f) / 100f :
                    Mathf.Pow(10f, mixerVolume / 20f);
                break;
        }

        volumeSlider.value = initialVolume;
        volumeText.SetText($"{(int)(initialVolume * 100)}%");
    }

    public void OnChangeVolume(float volume)
    {
        // Ensure volume is within 0 to 1 range
        volume = Mathf.Clamp(volume, 0f, 1f);
        volumeText.SetText($"{(int)(volume * 100)}%");

        switch (audioMixMode)
        {
            case AudioMixMode.LinearAudioSourceVolume:
                audioSource.volume = volume;
                break;
            case AudioMixMode.LinearMixerVolume:
                audioMixer.SetFloat("Volume", -80f + volume * 100f);
                break;
            case AudioMixMode.LogarithmicMixerVolume:
                audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20f);
                break;
        }
    }

    public void OnBackButton()
    {
        SceneManager.LoadScene("Menu");
    }

    public enum AudioMixMode
    {
        LinearAudioSourceVolume,
        LinearMixerVolume,
        LogarithmicMixerVolume
    }
}
