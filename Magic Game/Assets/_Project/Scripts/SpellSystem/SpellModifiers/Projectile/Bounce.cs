using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : OnCollision
{

    public int bounceCount = 2;

    public override void ProjectileCollide(Collision collision, Vector3 direction)
    {
        if(bounceCount > 0)
        {
            GameObject copy = Instantiate(gameObject, transform.position, Quaternion.identity);
            copy.transform.rotation = Quaternion.FromToRotation(copy.transform.forward, collision.contacts[0].normal);  // also rotate the whole thing for graphics to face the right direction
            copy.GetComponent<Projectile>().direction = collision.contacts[0].normal;                                   // this changes the direction the projectile is moving
            copy.GetComponent<Bounce>().bounceCount--;
        }
    }
}
