using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Aoe Duration ", menuName = "SpellSystem/Modifiers/Aoe/Duration")]
public class AoeDurationModifier : SpellScriptableModifier
{

    [SerializeField] private float extraDuration = 0f;

    public override void AddSpellModifier(Spell spell)
    {
        if(spell.spellType == SpellType.AOE)
        {
            Aoe aoe = (Aoe)spell;
            aoe.ModifyDuration(extraDuration);
        }
    }

}
