using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfusionEffect : StatusEffect
{


    public ConfusionEffect(float duration, GameObject graphics) : base(duration, graphics)
    {
        name = "Confused";
        this.duration = duration;
        this.graphics = graphics;
    }

    public override void OnApply(GameObject target, List<StatusEffect> allEffectsInSpell)
    {
        base.OnApply(target, allEffectsInSpell);

        effectManager.AppliedEffects[StatusEffectManager.EffectType.Confuse] = true;
        endTime = Time.time + duration;

        // make player/enemy confused
        var enemyCore = target.GetComponent<EnemyCore>();
        if (enemyCore != null)
        {
            //Set enemy to confusion stage
        }
    }

    public override void ReApply(List<StatusEffect> allEffectsInSpell)
    {
        base.ReApply(allEffectsInSpell);
    }

    public override void CheckForCounterEffects(List<StatusEffect> allEffectsInSpell)
    {
        return;
    }

    public override void OnLeave()
    {
        // remove player/enemy confusion

        effectManager.AppliedEffects[StatusEffectManager.EffectType.Confuse] = false;
        base.OnLeave();
    }
}
