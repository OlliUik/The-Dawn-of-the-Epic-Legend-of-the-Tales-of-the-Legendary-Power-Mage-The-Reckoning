using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dot", menuName = "SpellSystem/StatusEffects/Dot")]
public class ScriptableDot : ScriptableEffect
{

    public float damagePerTick;
    public float timeBetweenTicks;

    public override StatusEffect InitializeEffect(GameObject go)
    {
        return new DamageOverTime(duration, this, go); 
    }

}
