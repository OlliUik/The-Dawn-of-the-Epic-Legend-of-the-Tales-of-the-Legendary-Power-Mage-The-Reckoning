using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EndTrigger : MonoBehaviour
{
    public GameObject audioObject = null;

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
            Instantiate(audioObject, new Vector3(0, 0, 0), Quaternion.identity);
            loop.isGenerating = true;
            Destroy(gameObject);
        }
    }
}