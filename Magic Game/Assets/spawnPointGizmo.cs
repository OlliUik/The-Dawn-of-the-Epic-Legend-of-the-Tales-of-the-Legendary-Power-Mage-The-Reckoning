using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnPointGizmo : MonoBehaviour
{
    [SerializeField]
    protected float debugDrawRadius = 1.0F;


    public virtual void OnDrawGizmos()
    {


        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, debugDrawRadius);

    }
}
