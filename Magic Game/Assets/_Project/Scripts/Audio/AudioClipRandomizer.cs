//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioClipRandomizer : MonoBehaviour
{
    [SerializeField] private SoundList soundList = null;
    [SerializeField] private bool randomizeOnStart = false;

    private AudioSource source;
    
    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (randomizeOnStart)
        {
            Randomize();
            source.Play();
        }
    }

    public void Randomize()
    {
        //Do a while loop to prevent the same clip from playing twice in a row.
        AudioClip nextClip = soundList.clips[Random.Range(0, soundList.clips.Length)];

        if (source.clip != null)
        {
            nextClip = source.clip;

            if (soundList.clips.Length > 1)
            {
                int i = 0;
                while (nextClip == source.clip)
                {
                    i = Random.Range(0, soundList.clips.Length);
                    nextClip = soundList.clips[i];
                }
            }
        }

        source.clip = nextClip;
    }
}
