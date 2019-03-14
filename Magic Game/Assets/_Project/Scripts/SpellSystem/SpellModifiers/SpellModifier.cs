using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellModifier : MonoBehaviour
{
    // base class
    public abstract void Apply(GameObject go);
}
