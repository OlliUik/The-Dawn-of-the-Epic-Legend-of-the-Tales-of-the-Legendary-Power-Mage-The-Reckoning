using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Throwable", menuName = "SpellSystem/Modifiers/Projectile/Throwable")]
public class ThrowableModifier : SpellScriptableModifier
{
    public override void AddSpellModifier(Spell spell)
    {
        var throwable = spell.GetComponent<Throwable>();
        if(throwable == null && spell.spellType == SpellType.PROJECTILE)
        {
            spell.gameObject.AddComponent<Throwable>();
        }
    }
}
