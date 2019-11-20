using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EndTrigger : MonoBehaviour
{
    private GenerationLoop loop = null;

    private void Start()
    {
        loop = FindObjectOfType<GenerationLoop>();
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            loop.isGenerating = true;
            Destroy(gameObject);
        }
    }
}