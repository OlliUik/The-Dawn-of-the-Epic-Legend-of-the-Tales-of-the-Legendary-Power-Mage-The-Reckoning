using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellBalance : ScriptableObject
{
    // override in inherited classes
    public abstract void ApplyBalance(Spellbook spellbook);
}
