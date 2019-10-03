using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SpellSystem/StatusEffects/Ignite", fileName = "Ignite")]
public class IgniteModifier : SpellScriptableModifier
{

    [SerializeField] private float duration         = 5f;
    [SerializeField] private float damagePerTick    = 1f;
    [SerializeField] private float timeBetweenTicks = 1.0f;
    [SerializeField] private GameObject graphics    = null;

    public override void AddSpellModifier(Spell spell)
    {
        // check if ignite already exist if so --> modify values only
        var ignite = (IgniteEffect)spell.statusEffects.Find(x => x.GetType() == typeof(IgniteEffect));
        if (ignite != null)
        {
            // modify freeze values
            ignite.damagePerTick    += this.damagePerTick;
            ignite.duration         += this.duration;
            return;
        }

        spell.statusEffects.Add(new IgniteEffect(duration, graphics, damagePerTick, timeBetweenTicks));
    }
}
