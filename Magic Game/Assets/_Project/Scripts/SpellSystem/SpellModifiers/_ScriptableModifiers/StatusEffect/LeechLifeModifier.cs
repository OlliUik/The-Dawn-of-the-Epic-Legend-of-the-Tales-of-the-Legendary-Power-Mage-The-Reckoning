using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LeechLife", menuName = "SpellSystem/StatusEffects/LeechLife")]
public class LeechLifeModifier : SpellScriptableModifier
{

    [SerializeField] private float duration = 5f;
    [SerializeField] private float healthPerTick = 1f;
    [SerializeField] private float timeBetweenTicks = 1f;
    [SerializeField] GameObject graphics = null;

    public override void AddSpellModifier(Spell spell)
    {
        // check if LeechLife already exist if so --> modify values only
        var leechLife = (LeechLifeEffect)spell.statusEffects.Find(x => x.GetType() == typeof(LeechLifeEffect));
        if (leechLife != null)
        {
            // modify freeze values
            leechLife.healthPerTick += this.healthPerTick;
            leechLife.duration += this.duration;
            return;
        }

        // add to spell as new
        spell.statusEffects.Add(new LeechLifeEffect(duration, graphics, healthPerTick, timeBetweenTicks, spell.caster));
    }
}
