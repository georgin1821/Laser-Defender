using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] Slider musicSlider;

    private void Start()
    {
        InitializeSettings();
    }

    private void InitializeSettings()
    {
        musicSlider.value = GameDataManager.Instance.musicVolume;
    }

    public void OnMusicSliderChange()
    {
        float volume = musicSlider.value;
        AudioController.Instance.ChangeMusicVolume(volume);
        GameDataManager.Instance.musicVolume = musicSlider.value;
    }

    
}
