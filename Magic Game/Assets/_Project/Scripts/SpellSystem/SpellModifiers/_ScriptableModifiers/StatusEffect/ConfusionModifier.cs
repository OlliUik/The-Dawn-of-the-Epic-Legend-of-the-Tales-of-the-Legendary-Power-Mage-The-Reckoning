using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Confusion", menuName = "SpellSystem/StatusEffects/Confusion")]
public class ConfusionModifier : SpellScriptableModifier
{

    [SerializeField] private float duration = 7f;
    [SerializeField] private GameObject graphics = null;

    public override void AddSpellModifier(Spell spell)
    {
        // check if confusion already exist if so --> modify values only
        var confuse = (ConfusionEffect)spell.statusEffects.Find(x => x.GetType() == typeof(ConfusionEffect));
        if (confuse != null)
        {
            // modify confusion values
            confuse.duration += this.duration;
            return;
        }

        spell.statusEffects.Add(new ConfusionEffect(duration, graphics));
    }
}
