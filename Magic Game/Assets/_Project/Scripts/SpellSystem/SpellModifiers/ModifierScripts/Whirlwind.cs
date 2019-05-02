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
        base.BeamCollide(hitInfo, direction, distance);
    }

    public override void ProjectileCollide(Collision collision, Vector3 direction)
    {
        GameObject tornado = Instantiate(tornadoPrefab, collision.contacts[0].point, Quaternion.identity);
        Tornado script = tornado.GetComponentInChildren<Tornado>();
        script.variables = variables;
    }
}
