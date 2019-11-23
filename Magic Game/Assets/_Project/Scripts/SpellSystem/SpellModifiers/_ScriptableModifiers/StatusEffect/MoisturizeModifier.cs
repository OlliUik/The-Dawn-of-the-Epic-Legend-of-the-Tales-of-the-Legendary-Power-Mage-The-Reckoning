using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Moisturize", menuName = "SpellSystem/StatusEffects/Moisturize")]
public class MoisturizeModifier : SpellScriptableModifier
{

    [SerializeField] private float duration = 10f;
    [SerializeField] private GameObject graphics = null;
    [SerializeField] private GameObject waterPoolPrefab = null;
    [SerializeField] private float size = 1.0f;

    // When spell is casted this gets called --> Add this effect to the spell
    public override void AddSpellModifier(Spell spell)
    {
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
        MoisturizeEffect temp = new MoisturizeEffect(duration, graphics, waterPoolPrefab, size);
        temp.SetElementParticles(projectileGraphics, beamGraphics, aoeGraphics);
        spell.statusEffects.Add(temp);
        
    }
}
