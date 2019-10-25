using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    #region VARIABLES

    public AudioMixer mixer = null;
    public Slider sliderMaster = null;
    public Slider sliderMusic = null;
    public Slider sliderSfx = null;

    #endregion

    #region UNITY_FUNCTIONS

    private void Start()
    {
        sliderMaster.value = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        sliderMusic.value = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        sliderSfx.value = PlayerPrefs.GetFloat("SfxVolume", 1.0f);
    }

    #endregion

    #region CUSTOM_FUNCTIONS

    public void SetMasterLevel (float value)
    {
        mixer.SetFloat("MasterVolume", value);
        PlayerPrefs.SetFloat("MasterVolume", sliderMaster.value);
    }

    public void SetMusicLevel (float value)
    {
        mixer.SetFloat("MusicVolume", value);
        PlayerPrefs.SetFloat("MusicVolume", sliderMusic.value);
    }

    public void SetSfxLevel (float value)
    {
        mixer.SetFloat("SfxVolume", value);
        PlayerPrefs.SetFloat("SfxVolume", sliderSfx.value);
    }

    #endregion
}