using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Moisturize", menuName = "SpellSystem/StatusEffects/Moisturize")]
public class MoisturizeModifier : SpellScriptableModifier
{

    [SerializeField] private float duration = 10f;
    [SerializeField] private GameObject graphics;

    public override void AddSpellModifier(GameObject spellObject)
    {
        Spell spell = spellObject.GetComponent<Spell>();

        // check if freeze already exist if so --> modify values only
        var moisturize = (MoisturizeEffect)spell.statusEffects.Find(x => x.GetType() == typeof(MoisturizeEffect));
        if (moisturize != null)
        {
            // modify freeze values
            moisturize.duration += this.duration;
            return;
        }

        spell.statusEffects.Add(new MoisturizeEffect(duration, graphics));
    }
}
