using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnCollision : SpellModifier
{
    // base class
    public bool ready = false;
    public virtual void OnCollide(Collision collision) { }          // used to detect collision and collision info
    public virtual void Hit(GameObject go, Spellbook spellbook) { } // used to detect hits on gameObjects without collision info
}
