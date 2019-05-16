using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveCask : Transformation
{

    public bool activated;

    public float explosionRadius = 10f;
    public float explosionForce = 1000f;
    public GameObject explosionParticles;
    private Material mat;

    [SerializeField] private float health = 100f;
    [SerializeField] private float timeToExplode = 2f;

    protected override void Start()
    {
        base.Start();
        mat = GetComponent<MeshRenderer>().material;
    }

    protected override void Update()
    {
        base.Update();

        if(activated)
        {
            if(timeToExplode > 0)
            {
                mat.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time, 0.5f));
                timeToExplode -= Time.deltaTime;
            }
            else
            {
                Explode();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        health -= collision.relativeVelocity.magnitude;

        if(collision.gameObject.GetComponent<Spell>() != null || health <= 0f)
        {
            activated = true;
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