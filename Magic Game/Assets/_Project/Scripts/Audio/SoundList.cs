using UnityEngine;

[CreateAssetMenu(fileName = "Sound List", menuName = "Audio/Sound List", order = 1)]
public class SoundList : ScriptableObject
{
    public AudioClip[] clips = null;
}
