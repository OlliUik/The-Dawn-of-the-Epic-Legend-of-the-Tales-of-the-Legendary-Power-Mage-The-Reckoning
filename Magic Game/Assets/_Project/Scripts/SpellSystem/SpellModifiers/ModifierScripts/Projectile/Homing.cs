﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : SpellModifier
{

    public float rotationSpeed = 2.0f;
    public Transform target = null;

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
        float step = rotationSpeed * Time.deltaTime;

        Vector3 targetDir = target.transform.position - transform.position;
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
        return closest.transform;
    }
}