using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KnockBack : SpellModifier
{

    public float aoeForce = 50f;
    public float beamForce = 50f;
    public float projectileForce = 100f;

    public int magicNumber = 4;


    public override void BeamCollide(RaycastHit hitInfo, Vector3 direction, float distance)
    {
        var rb = hitInfo.collider.GetComponent<Rigidbody>();
        if(rb != null)
        {
            rb.AddForce(direction.normalized * beamForce * rb.mass * Time.deltaTime);
            //rb.gameObject.transform.position += direction.normalized * beamForce * Time.deltaTime;
        }
    }

    public override void ProjectileCollide(Collision collision, Vector3 direction)
    {
        var rb = collision.collider.GetComponent<Rigidbody>();
        if(rb != null)
        {
            for (int i = 0; i < magicNumber; i++)
            {
                rb.AddForce(direction * projectileForce * rb.mass);
            }
        }
    }

    public override void AoeCollide(GameObject hitObject)
    {
        var rb = hitObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce((hitObject.transform.position - transform.position).normalized * aoeForce * rb.mass * Time.deltaTime);
        }
    }

}
