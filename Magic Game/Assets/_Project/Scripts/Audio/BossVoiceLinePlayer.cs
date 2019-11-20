using System.Collections.Generic;
using UnityEngine;

public class BossVoiceLinePlayer : MonoBehaviour
{
    public float Timer = 10;
    public GameObject audioPrefab;
    GameObject audioClone;

    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0f)
        {
            audioClone = Instantiate(audioPrefab, new Vector3(Random.Range(-9, 9), 5f, 0f), transform.rotation) as GameObject;
            Timer = 10f;
        }
    }
}