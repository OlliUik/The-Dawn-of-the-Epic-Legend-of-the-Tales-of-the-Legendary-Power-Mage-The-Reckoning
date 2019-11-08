using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ExplosionPush : MonoBehaviour
{
    [SerializeField] private float pushRadius = 10.0f;
    [SerializeField] private float pushForce = 0.2f;
    [SerializeField] private float destroyDelay = 0.2f;

    private void Start()
    {
        GetComponent<SphereCollider>().radius = pushRadius;
        StartCoroutine(DestroyDelay());
    }

    IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rigid = other.GetComponent<Rigidbody>();
        if (rigid != null)
        {
            rigid.AddExplosionForce(pushForce, transform.position, pushRadius, 1.0f, ForceMode.Impulse);
        }
    }
}
