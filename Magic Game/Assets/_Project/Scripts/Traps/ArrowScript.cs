using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    [SerializeField] private ArrowSpawner spawner = null;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameObject.transform.position = spawner.transform.position;
            spawner.CollisionDetected(this);
        }
    }
}