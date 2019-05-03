using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoisturizeEffect : StatusEffect
{



    public MoisturizeEffect(float duration, GameObject graphics) : base(duration, graphics)
    {
        name = "Moisturize";
        this.duration = duration;
        this.graphics = graphics;
    }

    public override void OnApply(GameObject target, List<StatusEffect> allEffectsInSpell)
    {

        base.OnApply(target, allEffectsInSpell);

        endTime = Time.time + duration;
        effectManager.AppliedEffects[StatusEffectManager.EffectType.Moisturize] = true;

        CheckForCounterEffects(allEffectsInSpell);
    }

    public override void CheckForCounterEffects(List<StatusEffect> allEffectsInSpell)
    {
        var ignite = (IgniteEffect)allEffectsInSpell.Find(x => x.GetType() == typeof(IgniteEffect));
        if (effectManager.AppliedEffects[StatusEffectManager.EffectType.Ignite] || ignite != null)
        {
            if (ignite != null)
            {
                // spell contains ignite --> reduce cooldown
                endTime = Time.time + (duration * 0.5f);
                return;
            }
            else
            {
                // spell doesn't contain ignite --> remove ignite from target
                effectManager.RemoveStatusEffect(effectManager.affectingEffects.Find(x => x.GetType() == typeof(IgniteEffect)));
                effectManager.RemoveStatusEffect(effectManager.affectingEffects.Find(x => x.GetType() == typeof(MoisturizeEffect)));
            }
        }
    }

    public override void OnLeave()
    {
        effectManager.AppliedEffects[StatusEffectManager.EffectType.Moisturize] = false;
        base.OnLeave();
    }
}
