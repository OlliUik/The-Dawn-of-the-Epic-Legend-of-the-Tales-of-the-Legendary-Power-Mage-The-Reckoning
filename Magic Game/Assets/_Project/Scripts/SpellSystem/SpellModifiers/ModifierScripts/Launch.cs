using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launch : SpellModifier
{

    public float force = 100f;
    int magicNumber = 15;


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
        var rb = hitInfo.collider.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(Vector3.up * force);
        }
    }

    public override void ProjectileCollide(Collision collision, Vector3 direction)
    {

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
