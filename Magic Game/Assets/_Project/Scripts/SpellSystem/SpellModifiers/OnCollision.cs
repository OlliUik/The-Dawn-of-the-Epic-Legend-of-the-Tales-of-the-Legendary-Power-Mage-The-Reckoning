using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnCollision : SpellModifier
{
    // base class
    public virtual void ProjectileCollide(Collision collision, Vector3 direction) { }                       // used to detect collision and collision info
    public virtual void BeamCollide(RaycastHit hitInfo, Vector3 direction) { }                              // used to detect hits on gameObjects without collision info
    public virtual void AoeCollide(GameObject hitObject) { }

}
