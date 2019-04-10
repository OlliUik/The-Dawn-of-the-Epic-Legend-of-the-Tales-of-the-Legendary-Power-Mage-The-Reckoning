using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : SpellModifier
{

    public float rotationSpeed = 2.0f;
    public Transform target = null;
    float height = 1f;

    Projectile pro;


    void Start()
    {
        pro = GetComponent<Projectile>();

        if (target == null)
        {
            target = FindClosestTarget();
        }
    }

    void Update()
    {
        // incase enemy dies while projectile is flying
        if(target == null)
        {
            target = FindClosestTarget();
        }

        float step = rotationSpeed * Time.deltaTime;

        Vector3 targetDir = (target.transform.position + Vector3.up * height) - transform.position;
        Vector3 newDir = Vector3.RotateTowards(pro.direction, targetDir, step, 0.0f);

        pro.direction = newDir;
    }

    private Transform FindClosestTarget()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        
        if(closest != null)
        {
            height = closest.GetComponent<CapsuleCollider>().center.y;
        }

        return closest.transform;
    }
}
