using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformation : MonoBehaviour
{

    public float duration = 5f;
    public GameObject TransformedObject { get; set; } = null;
    public GameObject transformationParticles { get; set; } = null;
    private float timer;


    protected virtual void Start()
    {
        timer = duration;
    }

    // wait the duration of the transformation and reactivate the orginal object if we still have it
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
