using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SfxManager : MonoBehaviour
{
    //PUT THIS AND VOLUME MANAGER TO THE SAME OBJECT!!
    //Main Menu-scene has AudioManager-gameobject, use that in builds!

    public static SfxManager sfxManager;

    public List<SoundList> sfxLists = new List<SoundList>();

    private void Start()
    {
        sfxManager = this;
    }

    public void Play(int list, int number)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        AudioMixer mixer = gameObject.GetComponent<VolumeManager>().mixer;
        source.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];

        AudioClip clip = sfxLists[list].clips[number];
        source.clip = clip;
        float length = clip.length;
        source.Play();

        /*
        if (source.isPlaying)
        {
            //Make IEnumerator?
            //When clip has ended Destroy(source)
        }
        */
    }
}