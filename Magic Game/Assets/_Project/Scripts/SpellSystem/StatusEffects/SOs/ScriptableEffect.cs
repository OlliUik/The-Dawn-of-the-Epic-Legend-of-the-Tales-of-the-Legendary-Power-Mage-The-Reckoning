using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableEffect : ScriptableObject
{

    public float duration = 0f;

    public abstract StatusEffect InitializeEffect(GameObject go);

}
