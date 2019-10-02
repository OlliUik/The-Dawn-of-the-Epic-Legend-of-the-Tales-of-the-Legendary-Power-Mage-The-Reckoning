using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{

    public float damageAmount = 20;
    public float miniAoeRadius = 3f;
    public float projectileForce = 10f;

    public GameObject[] explosionParticles;

    private void OnCollisionEnter(Collision collision)
    {
        bool hitLiving = false;

        // DEAL DAMAGE
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, miniAoeRadius);
        foreach (Collider go in hitObjects)
        {
            var health = go.gameObject.GetComponent<Health>();
            if (health != null)
            {
                hitLiving = true;
                health.Hurt(damageAmount, true);
            }
        }

        // Pushes everything away (hopefully)
        ExplosionForce();

        // DESTROY ORGINAL
        DestroyMeteor();
    }

    private void DestroyMeteor()
    {

        foreach (GameObject explosionParticle in explosionParticles)
        {
            if (explosionParticle != null)
            {
                GameObject particle = Instantiate(explosionParticle, transform.position, transform.rotation);
                particle.transform.parent = null;
            }
        }
        StartCoroutine(WaitForParticleAndDestroy());
    }
    
    private void ExplosionForce()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, miniAoeRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(MeteorManager.Instance.GetExplosionForce(), explosionPos, miniAoeRadius * 2 * MeteorManager.Instance.GetMeteorScale(), 3.0F);
        }
    }

    IEnumerator WaitForParticleAndDestroy()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject.transform.parent.gameObject);
    }

}
