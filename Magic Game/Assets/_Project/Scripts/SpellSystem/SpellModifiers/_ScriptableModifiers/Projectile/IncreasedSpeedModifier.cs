using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreasedSpeed", menuName = "SpellSystem/Modifiers/Projectile/IncreasedSpeed")]
public class IncreasedSpeedModifier : SpellScriptableModifier
{

    [SerializeField] private float increaseAmount = 10f;

    public override void AddSpellModifier(Spell spell)
    {
        var proj = (Projectile)spell;
        if(proj != null)
        {
            proj.ModifySpeed(increaseAmount);
        }
    }
}
