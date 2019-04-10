using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BlackHoleVariables
{
    public float radius;
    public float duration;
    public float pullForce;
}

public class BlackHole : MonoBehaviour
{

    public BlackHoleVariables variables;
    public bool destroySelf = false;

    private void Start()
    {
        if(destroySelf)
        {
            Destroy(gameObject, variables.duration);
        }
    }

    private void FixedUpdate()
    {
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, variables.radius);
        foreach (Collider collider in hitObjects)
        {
            var rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 direction = (transform.position - rb.transform.position);
                float gravityIntensity = Vector3.Distance(transform.position, rb.transform.position) / variables.radius;
                rb.AddForce(direction.normalized * variables.pullForce * gravityIntensity * rb.mass * Time.fixedDeltaTime);
            }
        }
    }

    // debug stuff
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, variables.radius);
    }

}
