using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformation : MonoBehaviour
{

    public float Duration { get; set; } = 5f;
    public GameObject TransformedObject { get; set; } = null;
    public GameObject transformationParticles = null;


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
            if(transformationParticles != null)
            {
                Instantiate(transformationParticles, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}
