using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Electricity", menuName = "SpellSystem/StatusEffects/Electric")]
public class ElectricModifier : SpellScriptableModifier
{

    [SerializeField] private float duration = 10f;
    [SerializeField] private GameObject graphics = null;
    [SerializeField] private float extraManaCost = 0.0f;
    [SerializeField] private float extraMoistureDamage = 0.0f;

    public override void AddSpellModifier(Spell spell)
    {
        // check if electric already exist if so --> modify values only
        var electric = (ElectricEffect)spell.statusEffects.Find(x => x.GetType() == typeof(ElectricEffect));
        if (electric != null)
        {
            // modify freeze values
            electric.duration += this.duration;
            return;
        }

        ElectricEffect temp = new ElectricEffect(duration, graphics, extraManaCost, extraMoistureDamage);
        temp.SetElementParticles(projectileGraphics, beamGraphics, aoeGraphics);
        temp.SetProjectileExplosion(projectileExploionGraphics);

        // Apply as new
        spell.statusEffects.Add(temp);
    }
}
