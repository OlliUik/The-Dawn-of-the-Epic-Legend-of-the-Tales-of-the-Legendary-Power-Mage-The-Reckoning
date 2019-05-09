using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Increased Range", menuName = "SpellSystem/Modifiers/Increased Range")]
public class IncreasedRangeModifier : SpellScriptableModifier
{

    [SerializeField] private float increaseAmount = 5f;

    public override void AddSpellModifier(Spell spell)
    {
        spell.ModifyRange(increaseAmount);
    }
}
