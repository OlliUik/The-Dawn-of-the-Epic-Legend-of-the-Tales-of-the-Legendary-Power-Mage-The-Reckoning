using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Split : OnCollision
{

    private bool splitted = false;
    public int splitCount = 4;


    public override void OnCollide(Collision collision, Vector3 direction)
    {
        
        if(!splitted)
        {
            for (int i = 0; i < splitCount; i++)
            {
                GameObject copy = Instantiate(gameObject, transform.position, Quaternion.identity);
                copy.transform.rotation = Quaternion.FromToRotation(copy.transform.forward, collision.contacts[0].normal);  // also rotate the whole thing for graphics to face the right direction
                copy.GetComponent<Projectile>().direction = collision.contacts[0].normal;                                   // this changes the direction the projectile is moving
                Destroy(copy.GetComponent<Split>());

                copy.transform.Rotate(copy.transform.right, Random.Range(-45f, 45f));
                copy.transform.Rotate(copy.transform.up, Random.Range(-45f, 45f));
            }
        }

    }

}
