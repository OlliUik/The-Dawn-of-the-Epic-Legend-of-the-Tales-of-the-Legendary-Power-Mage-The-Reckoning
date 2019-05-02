﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeEffect : StatusEffect
{

    public float slowAmount = 5f;   // how much slows per spell hit
    public float startSpeed;        // what was the speed before started slowing

    public FreezeEffect(float duration, GameObject graphics, float slowAmount) : base(duration, graphics)
    {
        name = "Freeze";
        this.duration = duration;
        this.slowAmount = slowAmount;
        this.graphics = graphics;
    }


    private void Slow()
    {
        var movement = target.GetComponent<PlayerMovement>();
        if (movement != null)
        {
            // TODO:: CHECK IF PLAYER IS MOIST --> slow more
            movement.accelerationMultiplier -= (slowAmount * 0.1f);
        }
    }


    public override void OnApply(GameObject target, List<StatusEffect> allEffectsInSpell)
    {

        base.OnApply(target, allEffectsInSpell);

        effectManager.AppliedEffects[StatusEffectManager.EffectType.Freeze] = true;
        endTime = Time.time + duration;

        CheckForCounterEffects(allEffectsInSpell);

        Slow();
    }

    public override void ReApply(List<StatusEffect> allEffectsInSpell)
    {
        base.ReApply(allEffectsInSpell);
        // slow target again
        Slow();
    }

    public override void CheckForCounterEffects(List<StatusEffect> allEffectsInSpell)
    {
        return;
    }

    public override void OnLeave()
    {
        if(target.CompareTag("Player"))
        {
            // reset speed
        }
        else if(target.CompareTag("Enemy"))
        {
            // reset speed
        }

        effectManager.AppliedEffects[StatusEffectManager.EffectType.Freeze] = false;
        base.OnLeave();
    }

}