using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections.Generic;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("Ses")]
    public AudioMixer mainMixer; 

    [Header("UI Elemanları")]
    public Slider musicSlider;
    public Slider sfxSlider;


    void Start()
    {
        LoadSettings();
    }


    public void SetMusicVolume(float volume)
    {
        mainMixer.SetFloat("MusicVol", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVol", volume);
    }

    public void SetSFXVolume(float volume)
    {
        mainMixer.SetFloat("SFXVol", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVol", volume);
    }

    void LoadSettings()
    {
        float mVol = PlayerPrefs.GetFloat("MusicVol", 0.75f);
        musicSlider.value = mVol;
        SetMusicVolume(mVol);

        float sVol = PlayerPrefs.GetFloat("SFXVol", 0.75f);
        sfxSlider.value = sVol;
        SetSFXVolume(sVol);
    }
}   