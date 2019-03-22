using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : SpellModifier
{

    public override void Apply(GameObject go)
    {
        go.AddComponent<Homing>();
    }

    public float rotationSpeed = 2.0f;
    private Transform target = null;

    void Start()
    {
        if (target == null)
        {
            target = FindClosestTarget();
        }
    }

    void Update()
    {
        Vector3 targetDir = target.transform.position - transform.position;
        float step = rotationSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
        Debug.DrawRay(transform.position, newDir, Color.red);
        transform.rotation = Quaternion.LookRotation(newDir);
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
        return closest.transform;
    }
}
