using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WhirlwindVariables
{
    public float rotateSpeed;
    public float pullInSpeed;
    public float strength;
    public float size;
    public float growSpeed;
    public float duration;
}

public class Tornado : MonoBehaviour
{

    public WhirlwindVariables variables;
    public List<Rigidbody> hitObjects = new List<Rigidbody>();
    private bool fullGrown = false;
    private float timer = 0f;


    void Start()
    {
        timer = variables.duration;
    }

    void FixedUpdate()
    {
        GrowAndShrink();

        // move the objects on a circular trajectory
        foreach (Rigidbody rb in hitObjects)
        {
            if(rb != null)
            {
                if ( rb.gameObject.GetComponent<EnemyCore>() != null && rb.gameObject.GetComponent<BossLizardKing>() == null)
                {
                    if (rb.gameObject.GetComponent<EnemyCore>().currentState != EnemyCore.EState.RAGDOLLED)
                        rb.gameObject.GetComponent<EnemyCore>().EnableRagdoll(true);
                    else
                        rb.gameObject.GetComponent<EnemyCore>().SetRagdollSleepTimer(2f);
                }
                var difference = transform.position - rb.transform.position;

                if(difference.magnitude > variables.size * 0.25)
                {
                    rb.AddForce(difference.normalized * variables.pullInSpeed * rb.mass);
                }

                rb.transform.RotateAround(transform.position, transform.up, variables.rotateSpeed * Time.deltaTime);
                rb.AddForce(Vector3.up * variables.strength * rb.mass);
                rb.AddTorque(Vector3.up * variables.rotateSpeed);
            }
        }

        // also check if tornados strength is stronger than enemies will and ragdoll them

    }

    // Handles tornados size changing and destroying it after
    private void GrowAndShrink()
    {
        if (!fullGrown)
        {
            // Grow
            if (transform.parent.localScale.y < variables.size)
            {
                Vector3 newScale = transform.parent.localScale + Vector3.one * variables.growSpeed * Time.deltaTime;
                transform.parent.localScale = newScale;
            }
            else
            {
                fullGrown = true;
            }
        }

        if (fullGrown)
        {
            timer -= Time.fixedDeltaTime;
        }

        if (timer <= 0f)
        {
            // Shrink
            if (transform.parent.localScale.y > 0f)
            {
                Vector3 newScale = transform.parent.localScale - Vector3.one * variables.growSpeed * Time.deltaTime;
                transform.parent.localScale = newScale;
            }
            else
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var rb = other.gameObject.GetComponent<Rigidbody>();
        if(rb != null && !hitObjects.Contains(rb))
        {
            hitObjects.Add(rb);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var rb = other.gameObject.GetComponent<Rigidbody>();
        if (hitObjects.Contains(rb))
        {
            hitObjects.Remove(rb);
        }
    }

}
