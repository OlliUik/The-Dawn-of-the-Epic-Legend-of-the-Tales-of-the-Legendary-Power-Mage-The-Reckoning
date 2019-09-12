using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : SpellModifier
{
    public override void OnSpellCast(Spell spell)
    {
        if(spell.spellType == SpellType.PROJECTILE)
        {
            var rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = true;
            }
        }
    }
}
