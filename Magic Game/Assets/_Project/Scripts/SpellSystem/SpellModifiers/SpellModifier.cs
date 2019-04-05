using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellModifier : MonoBehaviour
{
    // base cast modfiers 
    public virtual void OnSpellCast(Spell spell) { }

    // base collision modifiers
    public virtual void ProjectileCollide(Collision collision, Vector3 direction) { }                       // used to detect collision and collision info
    public virtual void BeamCollide(RaycastHit hitInfo, Vector3 direction) { }                              // used to detect hits on gameObjects without collision info
    public virtual void AoeCollide(GameObject hitObject) { }

    public virtual void BeamCollisionEnd() { }
}
