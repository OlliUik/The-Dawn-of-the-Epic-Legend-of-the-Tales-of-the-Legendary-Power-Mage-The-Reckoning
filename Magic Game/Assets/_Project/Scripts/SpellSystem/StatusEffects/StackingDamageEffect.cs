using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackingDamageEffect : StatusEffect
{

    public int increaseRate     = 1;
    public float extraDamage    = 2.5f;

    public override StatusEffect Clone()
    {
        StackingDamageEffect temp = new StackingDamageEffect(duration, graphics, extraDamage);
        temp.increaseRate = increaseRate;
        temp.extraDamage = extraDamage;
        return temp;
    }

    // health script calls this when target has StackingDamage on it
    public float ModifyDamage(float amount)
    {
        amount += (extraDamage * increaseRate);
        increaseRate++;
        return amount;
    }

    public StackingDamageEffect(float duration, GameObject graphics, float extraDamage) : base(duration, graphics)
    {
        name = "StackingDamage";
        this.duration = duration;
        this.graphics = graphics;
        this.extraDamage = extraDamage;
    }

    // when stackin damage gets added the 1st time it resets increaseRate and time
    public override void OnApply(GameObject target, List<StatusEffect> allEffectsInSpell)
    {
        base.OnApply(target, allEffectsInSpell);
        effectManager.AppliedEffects[StatusEffectManager.EffectType.StackingDamage] = true;
        increaseRate = 1;
        endTime = Time.time + duration;
    }

    // resets only time when applied again
    public override void ReApply(List<StatusEffect> allEffectsInSpell)
    {
        endTime = Time.time + duration;
    }

    public override void OnLeave()
    {
        effectManager.AppliedEffects[StatusEffectManager.EffectType.StackingDamage] = false;
        base.OnLeave();
    }
}
