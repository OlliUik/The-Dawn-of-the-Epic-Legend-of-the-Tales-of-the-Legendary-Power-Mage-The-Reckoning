using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformTo : SpellModifier
{

    public GameObject transformPrefab { get; set; }
    public float duration;

    public override void AoeCollide(GameObject hitObject)
    {
        var rb = hitObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            hitObject.SetActive(false);
            Transformation tempTransform = Instantiate(transformPrefab, hitObject.transform.position, hitObject.transform.rotation).GetComponent<Transformation>();
            tempTransform.TransformedObject = hitObject;
            tempTransform.Duration = duration;
        }
    }

    public override void BeamCollide(RaycastHit hitInfo, Vector3 direction, float distance)
    {
        
    }

    public override void ProjectileCollide(Collision collision, Vector3 direction)
    {
        var rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.gameObject.SetActive(false);
            Transformation tempTransform = Instantiate(transformPrefab, rb.transform.position, rb.transform.rotation).GetComponent<Transformation>();
            tempTransform.TransformedObject = collision.gameObject;
            tempTransform.Duration = duration;
        }
    }
}
