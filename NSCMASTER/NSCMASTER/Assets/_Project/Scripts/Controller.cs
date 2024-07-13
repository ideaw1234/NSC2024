using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;
    private float curentRefreshRate;
    private int currentResolutionIndex;

    public Slider masterVol;
    public AudioMixer mainaudioMixer;

    private void Start()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        curentRefreshRate = Screen.currentResolution.refreshRate;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRate == curentRefreshRate)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }
        List<string> options = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + " x " + filteredResolutions[i].height + " " + filteredResolutions[i].refreshRate + " Hz";
            options.Add(resolutionOption);

            if (filteredResolutions[i].width == Screen.width &&
                               filteredResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 0.75f); // Default to 0.75 if no value is saved
        masterVol.value = savedVolume;
        mainaudioMixer.SetFloat("MasterVolume", savedVolume);
        masterVol.onValueChanged.AddListener(delegate { ChangeMasterVolume(); });
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height,Screen.fullScreen);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void ChangeMasterVolume()
    {
        Debug.Log(masterVol.value);
        mainaudioMixer.SetFloat("MasterVolume", masterVol.value);
        PlayerPrefs.SetFloat("MasterVolume", masterVol.value);
    }
}
