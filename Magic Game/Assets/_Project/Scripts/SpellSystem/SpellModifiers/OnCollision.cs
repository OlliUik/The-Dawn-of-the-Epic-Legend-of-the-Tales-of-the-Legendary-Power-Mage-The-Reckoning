using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnCollision : SpellModifier
{
    // base class
    public virtual void OnCollide(Collision collision, Vector3 direction) { }               // used to detect collision and collision info
    public virtual void BeamHit(RaycastHit hitInfo, Vector3 direction) { }                       // used to detect hits on gameObjects without collision info
}
