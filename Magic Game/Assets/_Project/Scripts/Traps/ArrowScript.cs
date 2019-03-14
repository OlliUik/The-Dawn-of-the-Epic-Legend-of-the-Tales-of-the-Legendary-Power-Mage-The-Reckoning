using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    [SerializeField]
    private GameObject spawner;

    void Start()
    {
        spawner = transform.parent.GetComponent<ArrowSpawner>().gameObject;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameObject.transform.position = spawner.transform.position;
            transform.parent.GetComponent<ArrowSpawner>().CollisionDetected(this);
        }
    }
}