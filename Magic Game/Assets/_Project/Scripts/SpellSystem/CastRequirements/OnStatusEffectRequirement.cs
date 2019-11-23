using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "On StatusEffect", menuName = "SpellSystem/CastRequirements/OnStatusEffect")]
public class OnStatusEffectRequirement : CastRequirement
{

    [SerializeField] private StatusEffectManager.EffectType type = 0;

    public override bool isMet(Spellbook spellbook)
    {
        StatusEffectManager effectManager = spellbook.GetComponent<StatusEffectManager>();
        if(effectManager != null)
        {
            if(effectManager.AppliedEffects[type] == true)
            {
                return true;
            }
        }
        return false;
    }
}
