using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveCask : Transformation
{

    public float explosionRadius = 10f;
    public float explosionForce = 1000f;
    public GameObject explosionParticles;


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Spell>() != null)
        {
            Explode();
        }
    }

    public void Explode()
    {
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider coll in hitObjects)
        {
            var rb = coll.GetComponent<Rigidbody>();
            if(rb != null)
            {
                // also deal damage to them
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        // Spawn explosion particles
        if(explosionParticles != null)
        {
            Instantiate(explosionParticles, transform.position, transform.rotation);
        }
        Destroy(gameObject, 0.2f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

}
