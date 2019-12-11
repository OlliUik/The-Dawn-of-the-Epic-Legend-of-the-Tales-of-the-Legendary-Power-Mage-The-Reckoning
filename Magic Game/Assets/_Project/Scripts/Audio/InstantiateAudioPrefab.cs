using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateAudioPrefab : MonoBehaviour
{
    public GameObject audioprefab;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(audioprefab, new Vector3(0, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
