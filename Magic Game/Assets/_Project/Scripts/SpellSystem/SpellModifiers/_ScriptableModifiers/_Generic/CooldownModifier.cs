using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CooldownDecrease", menuName = "SpellSystem/Modifiers/CooldownDecrease")]
public class CooldownModifier : SpellScriptableModifier
{
    [SerializeField] private float cooldownDecrease = 1f;

    public override void AddSpellModifier(Spell spell)
    {
        spell.ModifyCooldown(cooldownDecrease);
    }
}
