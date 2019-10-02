using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    [SerializeField] private GameObject destructible = null; //solid prefab
    [SerializeField] private GameObject destroyed = null; //cracked prefab

    private void OnCollisionEnter(Collision collision)
    {
        if (destructible.gameObject.tag == "DestructibleWall")
        {
            if (collision.gameObject.tag == "Throwable")
            {
                Instantiate(destroyed, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (destructible.gameObject.tag == "DestructibleFloor")
        {
            if (other.gameObject.tag == "Player")
            {
                Instantiate(destroyed, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}