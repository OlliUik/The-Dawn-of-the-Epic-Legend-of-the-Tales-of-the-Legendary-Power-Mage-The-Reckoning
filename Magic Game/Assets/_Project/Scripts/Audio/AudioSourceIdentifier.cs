using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceIdentifier : MonoBehaviour
{
    public bool isPlayer = false;

    public bool madeNoise { get; private set; } = false;

    private float timer = 0.0f;
    private AudioSource source = null;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (source.isPlaying)
        {
            madeNoise = true;
            timer = 1.0f;
        }

        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            madeNoise = false;
        }
    }
}
