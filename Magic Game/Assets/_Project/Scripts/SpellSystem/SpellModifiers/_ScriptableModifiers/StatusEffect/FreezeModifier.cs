using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Freeze", menuName = "SpellSystem/StatusEffects/Freeze")]
public class FreezeModifier : SpellScriptableModifier
{

    [SerializeField] private float duration             = 5f;
    [SerializeField] private float slowAmount           = 5f;
    [SerializeField] private float moistSlowMultiplier  = 1.5f;
    [SerializeField] private GameObject graphics        = null;

    public override void AddSpellModifier(Spell spell)
    {
        // check if freeze already exist if so --> modify values only
        var freeze = (FreezeEffect)spell.statusEffects.Find(x => x.GetType() == typeof(FreezeEffect));
        if (freeze != null)
        {
            // modify freeze values
            freeze.slowAmount           += this.slowAmount;
            freeze.duration             += this.duration;
            freeze.moistSlowMultiplier  += moistSlowMultiplier;
            return;
        }

        spell.statusEffects.Add(new FreezeEffect(duration, graphics, slowAmount, moistSlowMultiplier));
    }
}
