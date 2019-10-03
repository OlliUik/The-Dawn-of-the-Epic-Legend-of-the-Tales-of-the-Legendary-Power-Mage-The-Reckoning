using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launch : SpellModifier
{

    public float projectileForce = 100f;
    public float beamForce = 10f;
    public float aoeForce = 10f;
    int magicNumber = 15;


    public override void AoeCollide(GameObject hitObject)
    {
        var rb = hitObject.GetComponent<Rigidbody>();
        if(rb != null)
        {
            rb.AddForce(Vector3.up * aoeForce * rb.mass);
        }
    }

    public override void BeamCollide(RaycastHit hitInfo, Vector3 direction, float distance)
    {
        var rb = hitInfo.collider.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(Vector3.up * beamForce * rb.mass);
        }
    }

    public override void ProjectileCollide(Collision collision, Vector3 direction)
    {
        var rb = collision.gameObject.GetComponent<Rigidbody>();
        if(rb != null)
        {
            for (int i = 0; i < magicNumber; i++)
            {
                rb.AddForce(Vector3.up * projectileForce * rb.mass);
            }
        }
    }
}
