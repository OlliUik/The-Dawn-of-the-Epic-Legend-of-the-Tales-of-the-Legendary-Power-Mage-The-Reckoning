using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnCollision : SpellModifier
{
    // base class
    public bool ready = false;
    public virtual void OnCollide(Collision collision) { }
    public virtual void Hit(GameObject go, Spellbook spellbook) { }
}
