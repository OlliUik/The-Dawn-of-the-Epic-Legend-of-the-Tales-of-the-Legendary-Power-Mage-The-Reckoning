using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StackingDamage", menuName = "SpellSystem/StatusEffects/StackingDamage")]
public class StackingDamageModifier : SpellScriptableModifier
{

    [SerializeField] private float duration         = 5f;
    [SerializeField] private float extraDamage      = 2.5f;
    [SerializeField] private GameObject graphics    = null;

    public override void AddSpellModifier(Spell spell)
    {
        // check if stackingDamage already exist if so --> modify values only
        var stackingDamage = (StackingDamageEffect)spell.statusEffects.Find(x => x.GetType() == typeof(StackingDamageEffect));
        if (stackingDamage != null)
        {
            // modify stackingDamage values
            stackingDamage.extraDamage  += this.extraDamage;
            stackingDamage.duration     += this.duration;
            return;
        }

        // add as new
        spell.statusEffects.Add(new StackingDamageEffect(duration, graphics, extraDamage));
    }
}
