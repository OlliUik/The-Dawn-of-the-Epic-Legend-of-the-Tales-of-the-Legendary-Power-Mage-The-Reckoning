using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launch : SpellModifier
{

    public float force = 100f;


    public override void AoeCollide(GameObject hitObject)
    {
        var rb = hitObject.GetComponent<Rigidbody>();
        if(rb != null)
        {
            rb.AddForce(Vector3.up * force);
        }
    }

    public override void BeamCollide(RaycastHit hitInfo, Vector3 direction, float distance)
    {
        base.BeamCollide(hitInfo, direction, distance);
    }

    public override void BeamCollisionEnd()
    {
        base.BeamCollisionEnd();
    }

    public override void ProjectileCollide(Collision collision, Vector3 direction)
    {
        int magicNumber = 5;

        var rb = collision.gameObject.GetComponent<Rigidbody>();
        if(rb != null)
        {
            for (int i = 0; i < magicNumber; i++)
            {
                rb.AddForce(Vector3.up * force);
            }
        }
    }
}
