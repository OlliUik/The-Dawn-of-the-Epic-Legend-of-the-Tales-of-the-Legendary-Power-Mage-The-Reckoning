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

    void Start()
    {
        StartCoroutine(Grow());
    }

    private IEnumerator Grow()
    {
        while(transform.parent.localScale.x < variables.size)
        {
            Vector3 newScale = transform.parent.localScale + Vector3.one * variables.growSpeed * Time.deltaTime;
            transform.parent.localScale = newScale;
            yield return null;
        }

        Invoke("StartShrink", variables.duration);
    }
    private void StartShrink()
    {
        StartCoroutine(Shrink());
    }
    private IEnumerator Shrink()
    {
        while(transform.parent.localScale.x > 0.1f)
        {
            Vector3 newScale = transform.parent.localScale - Vector3.one * variables.growSpeed * Time.deltaTime;
            transform.parent.localScale = newScale;
            yield return null;
        }

        Destroy(transform.parent.gameObject);
    }

    void Update()
    {

        // move the objects on a circular trajectory
        foreach (Rigidbody rb in hitObjects)
        {
            if(rb != null)
            {
                var difference = transform.position - rb.transform.position;

                if(difference.magnitude > variables.size * 0.5)
                {
                    rb.AddForce(difference.normalized * variables.pullInSpeed * rb.mass);
                }

                rb.transform.RotateAround(transform.position, transform.up, variables.rotateSpeed * Time.deltaTime);
                rb.AddForce(Vector3.up * variables.strength * rb.mass);
                rb.AddTorque(Vector3.up * variables.rotateSpeed * 0.2f);
            }
        }

        // also check if tornados strength is stronger than enemies will and ragdoll them

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
