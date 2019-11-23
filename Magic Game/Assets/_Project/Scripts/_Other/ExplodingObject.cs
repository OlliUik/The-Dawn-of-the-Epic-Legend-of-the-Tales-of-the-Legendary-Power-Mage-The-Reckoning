//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class ExplodingObject : BreakableObject
{
    public float explosionRadius = 10.0f;
    public float explosionDamage = 25.0f;

    protected override void OnDestroy()
    {
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider go in hitObjects)
        {
            var health = go.gameObject.GetComponent<Health>();
            if (health != null)
            {
                float distance = (transform.position - go.transform.position).magnitude + 0.01f;
                float scaledDamage = Mathf.Ceil(Mathf.Lerp(explosionDamage, 0.0f, distance / explosionRadius));
                health.Hurt(scaledDamage, true);
            }
            else if (go.gameObject.GetComponent<BreakableObject>() != null)
            {
                Destroy(go.gameObject);
            }
        }
        base.OnDestroy();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
