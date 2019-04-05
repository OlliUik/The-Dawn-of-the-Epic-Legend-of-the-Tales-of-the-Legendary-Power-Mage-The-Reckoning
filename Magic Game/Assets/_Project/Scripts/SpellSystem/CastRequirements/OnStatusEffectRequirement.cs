using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "On StatusEffect", menuName = "SpellSystem/CastRequirements/OnStatusEffect")]
public class OnStatusEffectRequirement : CastRequirement
{

    [SerializeField] private ScriptableEffect requiredStatusEffect = null;

    public override bool isMet(Spellbook spellbook)
    {
        StatusEffectManager manager = spellbook.GetComponent<StatusEffectManager>();

        for (int i = 0; i < manager.appliedEffects.Count; i++)
        {
            if(manager.appliedEffects[i].effect == requiredStatusEffect)
            {
                return true;
            }
        }
        return false;
    }
}
