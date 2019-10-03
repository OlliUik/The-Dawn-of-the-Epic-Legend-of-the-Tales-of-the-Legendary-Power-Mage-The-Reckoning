using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Deduction", menuName = "SpellSystem/Modifiers/Deduction")]
public class Deduction : SpellScriptableModifier
{

    [SerializeField] private float decreaseManaMultiplier = 0.25f;

    public override void AddSpellModifier(Spell spell)
    {
        spell.ModifyManaCostMultiplier(decreaseManaMultiplier);
    }
}
