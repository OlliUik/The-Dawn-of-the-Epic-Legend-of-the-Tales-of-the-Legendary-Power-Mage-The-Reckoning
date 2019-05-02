using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whirlwind : SpellModifier
{

    public GameObject tornadoPrefab = null;
    public WhirlwindVariables variables;


    public override void AoeCollide(GameObject hitObject)
    {
        base.AoeCollide(hitObject);
    }

    public override void BeamCollide(RaycastHit hitInfo, Vector3 direction, float distance)
    {

        // sphereCastAll() get every physics obbject all the way to the hitInfo
        // make them all move towards the beam itself

        //if (Physics.SphereCast(startPos, baseRadius, direction, out hit, baseRange))

        Beam beam = gameObject.GetComponent<Beam>();
        RaycastHit[] hitObject = Physics.SphereCastAll(beam.startPos, variables.size, direction);

        foreach (RaycastHit hit in hitObject)
        {
            var rb = hit.collider.GetComponent<Rigidbody>();
            if(rb != null)
            {
                // pull towards the center
                Debug.Log("beam pulling " + hit.collider.gameObject.name);  
                Vector3 difference = (transform.position - rb.transform.position).normalized;
                rb.AddForce(difference * variables.pullInSpeed);
            }
        }

    }

    public override void ProjectileCollide(Collision collision, Vector3 direction)
    {
        GameObject tornado = Instantiate(tornadoPrefab, collision.contacts[0].point, Quaternion.identity);
        Tornado script = tornado.GetComponentInChildren<Tornado>();
        script.variables = variables;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < 64; i++)
        {
            //Gizmos.DrawWireSphere(())
        }
    }
}
