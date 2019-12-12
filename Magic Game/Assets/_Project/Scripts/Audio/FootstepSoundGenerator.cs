using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class FootstepSoundGenerator : MonoBehaviour
{
    [SerializeField] private float animationWeightThreshold = 0.5f;
    [SerializeField] private GameObject audioPrefab = null;

    private GameObject audioObject = null;
    private AudioSource audioSource = null;
    private AudioClipRandomizer randomizer = null;

    void Awake()
    {
        if (audioPrefab != null)
        {
            audioObject = Instantiate(audioPrefab);
            audioObject.transform.parent = transform;
            audioObject.transform.localPosition = Vector3.zero;

            audioSource = audioObject.GetComponent<AudioSource>();
            randomizer = audioObject.GetComponent<AudioClipRandomizer>();
        }
        else
        {
            Debug.LogWarning(this.gameObject + " is missing an audio prefab!");
        }
    }

    public void Footstep(AnimationEvent evt)
    {
        if (audioSource != null && audioSource.enabled)
        {
            if (evt.animatorClipInfo.weight > animationWeightThreshold)
            {
                audioSource.Play();
                StartCoroutine(WaitAudio());
            }
        }
    }

    private IEnumerator WaitAudio()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        randomizer.Randomize();
    }
}
