using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : OnCollision
{

    public float knockbackForce = 10.0f;


    public override void BeamCollide(RaycastHit hitInfo, Vector3 direction) // fix this and pushback
    {
        hitInfo.collider.gameObject.transform.position += direction.normalized * knockbackForce * Time.deltaTime;
    }

    public override void ProjectileCollide(Collision collision, Vector3 direction)
    {
        if(collision.gameObject.GetComponent<Health>() != null)
        {
            collision.gameObject.transform.position += direction.normalized * knockbackForce;
        }
    }

    public override void AoeCollide(GameObject hitObject)
    {
        hitObject.transform.position += (hitObject.transform.position - transform.position).normalized * knockbackForce * Time.deltaTime;
    }

}
