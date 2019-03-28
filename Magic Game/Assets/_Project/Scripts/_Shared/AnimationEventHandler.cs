using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private float animationWeightThreshold = 0.25f;
    [SerializeField] private bool randomizeSound = false;

    private AudioClipRandomizer randomizer = null;

    void Start()
    {
        if (source != null)
        {
            if (source.GetComponent<AudioClipRandomizer>() != null)
            {
                randomizer = source.GetComponent<AudioClipRandomizer>();
            }
        }
    }

    public void Footstep(AnimationEvent evt)
    {
        if (evt.animatorClipInfo.weight > animationWeightThreshold)
        {
            source.Play();
            if (randomizeSound)
            {
                StartCoroutine(WaitAudio());
            }
        }
    }

    private IEnumerator WaitAudio()
    {
        yield return new WaitForSeconds(source.clip.length);
        randomizer.Randomize();
    }
}
