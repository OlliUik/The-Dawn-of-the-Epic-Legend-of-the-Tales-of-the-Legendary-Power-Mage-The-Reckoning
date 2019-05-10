using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Homing : SpellModifier
{

    public float maxAngle = 20f;
    public float rotationSpeed = 2.0f;
    public Transform target = null;
    Vector3 offset = Vector3.zero;

    Spellbook spellbook;
    Projectile pro;

    public override void OnSpellCast(Spell spell)
    {
        pro = GetComponent<Projectile>();
        spellbook = pro.caster.GetComponent<Spellbook>();

        target = FindClosestInFront();
        if (target != null)
        {
            offset = target.tag == "Enemy" ? target.GetComponent<CapsuleCollider>().center : target.GetComponent<CharacterController>().center;
        }
    }

    public void Start()
    {
        pro = GetComponent<Projectile>();
        spellbook = pro.caster.GetComponent<Spellbook>();

        if (target != null)
        {
            offset = target.tag == "Enemy" ? target.GetComponent<CapsuleCollider>().center : target.GetComponent<CharacterController>().center;
        }
    }

    // If projectile has target rotate it slowly towards it
    void Update()
    {
        // incase enemy dies while projectile is flying
        if (target == null) return;

        float step = rotationSpeed * Time.deltaTime;

        Vector3 targetDir = target.transform.position + offset - transform.position;
        Vector3 newDir = Vector3.RotateTowards(pro.direction, targetDir, step, 0.0f);

        pro.direction = newDir;
    }

    // Find objects which are inside maxAngle with casters forwards direction
    private Transform FindClosestInFront()
    {
        GameObject[] gos;
        if (pro.caster.tag == "Enemy")
        {
            gos = GameObject.FindGameObjectsWithTag("Player");
        }
        else
        {
            gos = GameObject.FindGameObjectsWithTag("Enemy");
        }

        List<GameObject> targetsInFront = new List<GameObject>();
        foreach (GameObject go in gos)
        {
            Vector3 heading = go.transform.position - transform.position;
            float angle = Vector3.Angle(spellbook.GetDirection(), heading);
            if (Mathf.Abs(angle) < maxAngle)
            {
                targetsInFront.Add(go);
            }
        }

        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in targetsInFront)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }

        if (closest != null)
        {
            return closest.transform;
        }
        return null;
    }

    // Returns closest from projectile // also allows split and bounce to call this after effect
    public Transform FindClosestTarget()
    {
        GameObject[] gos;
        if (pro.caster.tag == "Enemy")
        {
            gos = GameObject.FindGameObjectsWithTag("Player");
        }
        else
        {
            gos = GameObject.FindGameObjectsWithTag("Enemy");
        }

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

        if (closest != null)
        {
            return closest.transform;
        }
        return null;
    }
}
