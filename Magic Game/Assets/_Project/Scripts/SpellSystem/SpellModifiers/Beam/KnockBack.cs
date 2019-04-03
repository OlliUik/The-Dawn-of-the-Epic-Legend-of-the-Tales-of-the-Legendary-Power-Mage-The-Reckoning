using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : OnCollision
{

    public float knockbackForce = 10.0f;


    public override void BeamHit(RaycastHit hitInfo, Vector3 direction) // fix this and pushback
    {
        hitInfo.collider.gameObject.transform.position += direction.normalized * knockbackForce * Time.deltaTime;
    }

    public override void OnCollide(Collision collision, Vector3 direction)
    {
        if(collision.gameObject.GetComponent<Health>() != null)
        {
            collision.gameObject.transform.position += direction.normalized * knockbackForce;
        }
    }

}
