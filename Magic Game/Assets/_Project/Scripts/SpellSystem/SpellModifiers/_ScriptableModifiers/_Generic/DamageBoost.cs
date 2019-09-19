using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage Boost", menuName = "SpellSystem/Modifiers/Damage Boost")]
public class DamageBoost : SpellScriptableModifier
{

    [SerializeField] private float increaseDamageMultiplier = 0.2f;

    public override void AddSpellModifier(Spell spell)
    {
        spell.ModifyDamageMultiplier(increaseDamageMultiplier);
    }
}
