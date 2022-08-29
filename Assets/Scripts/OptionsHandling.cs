using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsHandling : MonoBehaviour
{
    [Header("Configuration")] 
    [SerializeField] private OptionsInputType type;

    private void Start()
    {
        switch (type)
        {
            case OptionsInputType.ScanLines:
                GetComponent<Toggle>().isOn = GameManager.instance.ScanLinesOn;
                break;
            case OptionsInputType.Music:
                GetComponent<Slider>().value = MusicManager.instance.Volume;
                break;
            case OptionsInputType.SFX:
                GetComponent<Slider>().value = SFXManager.instance.Volume;
                break;
            case OptionsInputType.FullScreen:
                GetComponent<Toggle>().isOn = Screen.fullScreen;
                break;
        }
    }

    public void HandleChange()
    {
        switch (type)
        {
            case OptionsInputType.ScanLines:
                GameManager.instance.ToggleScanLines(GetComponent<Toggle>().isOn);
                break;
            case OptionsInputType.Music:
                MusicManager.instance.SetVolume(GetComponent<Slider>().value);
                return; // Skip button sfx for this one
            case OptionsInputType.SFX:
                SFXManager.instance.SetVolume(GetComponent<Slider>().value);
                break;
            case OptionsInputType.FullScreen:
                var value = GetComponent<Toggle>().isOn;
                Screen.fullScreen = value;
                PlayerPrefs.SetString("Fullscreen", value.ToString());
                break;
        }
        SFXManager.instance.PlaySound(SFXType.ButtonClick);
    }
}
