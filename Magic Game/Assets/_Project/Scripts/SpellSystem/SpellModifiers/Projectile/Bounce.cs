using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : OnCollision
{
    [SerializeField] private int bounceCount = 2;

    public override void Apply(GameObject go)
    {
        go.AddComponent<Bounce>();
    }

    public override void OnCollide(Collision collision)
    {
        if (!ready)
        {
            GetComponent<Projectile>().direction = collision.contacts[0].normal; // this changes the direction the projectile is moving
            transform.rotation = Quaternion.FromToRotation(transform.forward, collision.contacts[0].normal); // also rotate the whole thing for graphics to face the right direction
            bounceCount--;

            if (bounceCount <= 0)
            {
                ready = true;
            }
        }
    }
}
