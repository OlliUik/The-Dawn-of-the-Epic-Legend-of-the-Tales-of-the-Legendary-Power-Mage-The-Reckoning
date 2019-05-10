using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformation : MonoBehaviour
{

    public float Duration { get; set; } = 5f;
    public GameObject TransformedObject { get; set; } = null;


    protected virtual void Start()
    {
        timer = Duration;
    }

    private float timer;
    protected virtual void Update()
    {
        timer -= Time.deltaTime;

        if(timer <= 0f)
        {
            if(TransformedObject != null)
            {
                TransformedObject.transform.position = transform.position;
                TransformedObject.transform.rotation = transform.rotation;
                TransformedObject.SetActive(true);
            }
            // particles before destroying etc
            Destroy(gameObject);
        }
    }
}
