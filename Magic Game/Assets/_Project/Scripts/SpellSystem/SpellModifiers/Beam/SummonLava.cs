using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonLava : OnCollision
{

    public GameObject spawnPrefab = null;


    public override void BeamCollide(RaycastHit hitInfo, Vector3 direction)
    {
        if(!hitInfo.collider.gameObject.CompareTag("Spell"))
        {
            GameObject temp = Instantiate(spawnPrefab, hitInfo.point + Vector3.up * 0.1f, Quaternion.FromToRotation(Vector3.up, hitInfo.normal));
        }
    }

    public override void ProjectileCollide(Collision collision, Vector3 direction)
    {
        GameObject temp = Instantiate(spawnPrefab, collision.contacts[0].point, Quaternion.FromToRotation(Vector3.up, collision.contacts[0].normal));
    }
}
