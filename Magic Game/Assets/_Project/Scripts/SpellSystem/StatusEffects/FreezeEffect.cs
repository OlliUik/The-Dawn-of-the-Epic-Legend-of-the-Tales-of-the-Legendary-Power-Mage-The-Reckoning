using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeEffect : StatusEffect
{

    public float slowAmount = 5f;               // how much slows per spell hit
    public float moistSlowMultiplier = 1.5f;    // if target is moist how much more we slow default 1.5f
    public float startSpeed;                    // what was the speed before started slowing

    public FreezeEffect(float duration, GameObject graphics, float slowAmount, float moistSlowMultiplier) : base(duration, graphics)
    {
        name = "Freeze";
        this.duration = duration;
        this.slowAmount = slowAmount;
        this.moistSlowMultiplier = moistSlowMultiplier;
        this.graphics = graphics;
    }


    private void Slow()
    {
        Debug.Log("Slow");

        // change this for enemy, currently not working yet
        var movement = target.GetComponent<PlayerMovement>();
        // Debug.Log("Movement found: " + (movement != null));
        if (movement != null)
        {
            Debug.Log("Moisturize spell effect before freeze: "+effectManager.AppliedEffects[StatusEffectManager.EffectType.Moisturize]);
            if (effectManager.AppliedEffects[StatusEffectManager.EffectType.Moisturize])
            {
                /*
                movement.accelerationMultiplier -= 1/(slowAmount * moistSlowMultiplier);
                */

                // stun for a duration
                movement.accelerationMultiplier = -1;
                return;
            }
            else
            {
                movement.accelerationMultiplier -= 1/slowAmount;
            }

            Debug.Log("Acc multiplier: " + movement.accelerationMultiplier);

            if(movement.accelerationMultiplier <= 0f)
            {
                // stun for a duration
                Debug.Log("Stun " + target.name);
                effectManager.RemoveStatusEffect(this);
                movement.Stun(3f);
            }

        }
    }


    public override void OnApply(GameObject target, List<StatusEffect> allEffectsInSpell)
    {
        Debug.Log("Apply new ");

        base.OnApply(target, allEffectsInSpell);
        effectManager.AppliedEffects[StatusEffectManager.EffectType.Freeze] = true;
        endTime = Time.time + duration;
        Slow();

        CheckForCounterEffects(allEffectsInSpell);
    }

    public override void ReApply(List<StatusEffect> allEffectsInSpell)
    {
        // slow target again
        Debug.Log("Reapply");

        base.ReApply(allEffectsInSpell);
        Slow();
    }

    public override void OnLeave()
    {
        if(target.CompareTag("Player"))
        {
            var movement = target.GetComponent<PlayerMovement>();
            if(movement != null)
            {
                movement.accelerationMultiplier = 1f;
            }
        }
        else if(target.CompareTag("Enemy"))
        {
            // reset speed
        }

        effectManager.AppliedEffects[StatusEffectManager.EffectType.Freeze] = false;
        base.OnLeave();
    }

}
