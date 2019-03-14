using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    //destructible = solid prefab
    //destroyed = cracked prefab

    [SerializeField]
    private GameObject destructible, destroyed;

    void OnCollisionEnter(Collision collision)
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
    
    void OnTriggerEnter(Collider other)
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