using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellModifier : MonoBehaviour
{

    public GameObject projectileElementGraphic;
    public Beam.ElementType beamElementGraphic;
    public GameObject aoeElementGraphic;

    public GameObject projectileExplosionGraphic;

    // base cast modfiers 
    public virtual void OnSpellCast(Spell spell) { }

    // base collision modifiers
    public virtual void ProjectileCollide(Collision collision, Vector3 direction) { }                       // used to detect collision and collision info

    public virtual void BeamCollide(RaycastHit hitInfo, Vector3 direction, float distance) { }              // when beam hits something
    public virtual void BeamCollisionEnd() { }
    public virtual void BeamCastingEnd() { }

    public virtual void AoeCollide(GameObject hitObject) { }

    public void SetElementParticles(GameObject projectileParticle, Beam.ElementType beamParticle, GameObject aoeParticle)
    {
        projectileElementGraphic = projectileParticle;
        beamElementGraphic = beamParticle;
        aoeElementGraphic = aoeParticle;
    }

    public void SetProjectileExplosion(GameObject projectileParticle)
    {
        projectileExplosionGraphic = projectileParticle;
    }

}
