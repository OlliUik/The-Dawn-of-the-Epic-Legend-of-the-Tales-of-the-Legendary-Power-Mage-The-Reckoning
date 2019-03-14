using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CastRequirement : ScriptableObject
{
    public abstract bool isMet(Spellbook spellbook);
}
