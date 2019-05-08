using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Moisturize", menuName = "SpellSystem/StatusEffects/Moisturize")]
public class MoisturizeModifier : SpellScriptableModifier
{

    [SerializeField] private float duration = 10f;
    [SerializeField] private GameObject graphics;
    [SerializeField] private GameObject waterPoolPrefab;
    [SerializeField] private float size = 1.0f;

    // When spell is casted this gets called --> Add this effect to the spell
    public override void AddSpellModifier(GameObject spellObject)
    {
        Spell spell = spellObject.GetComponent<Spell>();

        // check if freeze already exist if so --> modify values only
        var moisturize = (MoisturizeEffect)spell.statusEffects.Find(x => x.GetType() == typeof(MoisturizeEffect));
        if (moisturize != null)
        {
            // modify freeze values
            moisturize.duration += this.duration;
            moisturize.size += this.size;
            return;
        }

        // Apply as new
        spell.statusEffects.Add(new MoisturizeEffect(duration, graphics, waterPoolPrefab, size));
    }
}
