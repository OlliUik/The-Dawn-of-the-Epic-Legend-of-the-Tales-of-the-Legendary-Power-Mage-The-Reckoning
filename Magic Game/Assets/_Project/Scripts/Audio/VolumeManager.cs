using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public AudioMixer mixer = null;
    public Slider sliderMaster = null;
    public Slider sliderMusic = null;
    public Slider sliderSfx = null;

    private void Start()
    {
        sliderMaster.value = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        sliderMusic.value = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        sliderSfx.value = PlayerPrefs.GetFloat("SfxVolume", 1.0f);
    }

    public void SetMasterLevel (float value)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MasterVolume", sliderMaster.value);
    }

    public void SetMusicLevel (float value)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderMusic.value);
    }

    public void SetSfxLevel (float value)
    {
        mixer.SetFloat("SfxVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SfxVolume", sliderSfx.value);
    }
}
