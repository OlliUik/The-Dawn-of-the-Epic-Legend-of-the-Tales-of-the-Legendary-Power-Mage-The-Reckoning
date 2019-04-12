using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KnockBack : SpellModifier
{

    public float knockbackForce = 10.0f;
    public float knockbackRadius = 10.0f;

    public override void BeamCollide(RaycastHit hitInfo, Vector3 direction) // fix this and pushback
    {
        var rb = hitInfo.collider.GetComponent<Rigidbody>();
        if(rb != null)
        {
            rb.gameObject.transform.position += direction.normalized * knockbackForce * Time.deltaTime;
        }
    }

    public override void ProjectileCollide(Collision collision, Vector3 direction)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<NavMeshAgent>().enabled = false;
        }

        Collider[] physicsObjects = Physics.OverlapSphere(collision.contacts[0].point, knockbackRadius);

        foreach (Collider coll in physicsObjects)
        {
            var rb = coll.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(knockbackForce * 100, collision.contacts[0].point + collision.contacts[0].normal, knockbackRadius, 2.0f);
            }
        }
    }

    public override void AoeCollide(GameObject hitObject)
    {
        if(hitObject.CompareTag("Enemy"))
        {
            hitObject.GetComponent<NavMeshAgent>().enabled = false;
        }

        var rb = hitObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce((hitObject.transform.position - transform.position).normalized * knockbackForce * 100 * Time.deltaTime);
        }
    }

}
