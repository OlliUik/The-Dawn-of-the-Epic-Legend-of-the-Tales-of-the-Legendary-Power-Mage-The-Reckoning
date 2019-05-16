using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreasedRadius", menuName = "SpellSystem/Modifiers/Beam/IncreasedRadius")]
public class IncreasedRadius : SpellScriptableModifier
{
    [SerializeField] private float increaseAmount = 1f;

    public override void AddSpellModifier(Spell spell)
    {
        Beam beam = (Beam)spell;
        beam.ModifyRadius(increaseAmount);
    }
}
