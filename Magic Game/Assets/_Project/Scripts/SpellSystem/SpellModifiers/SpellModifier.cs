using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellModifier : MonoBehaviour
{

    public GameObject projecttileElementGraphic;
    public GameObject beamElementGraphic;
    public GameObject aoeElementGraphic;

    // base cast modfiers 
    public virtual void OnSpellCast(Spell spell) { }

    // base collision modifiers
    public virtual void ProjectileCollide(Collision collision, Vector3 direction) { }                       // used to detect collision and collision info

    public virtual void BeamCollide(RaycastHit hitInfo, Vector3 direction, float distance) { }              // when beam hits something
    public virtual void BeamCollisionEnd() { }
    public virtual void BeamCastingEnd() { }

    public virtual void AoeCollide(GameObject hitObject) { }

}
