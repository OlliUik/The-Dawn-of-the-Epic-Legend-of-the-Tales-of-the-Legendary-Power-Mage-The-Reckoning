using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whirlwind : SpellModifier
{




    public float size = 10f;
    public float strenght = 10f;


    public override void AoeCollide(GameObject hitObject)
    {
        base.AoeCollide(hitObject);
    }

    public override void BeamCastingEnd()
    {
        base.BeamCastingEnd();
    }

    public override void BeamCollide(RaycastHit hitInfo, Vector3 direction, float distance)
    {
        base.BeamCollide(hitInfo, direction, distance);
    }

    public override void BeamCollisionEnd()
    {
        base.BeamCollisionEnd();
    }

    public override void OnSpellCast(Spell spell)
    {
        base.OnSpellCast(spell);
    }

    public override void ProjectileCollide(Collision collision, Vector3 direction)
    {
        base.ProjectileCollide(collision, direction);
    }
}
