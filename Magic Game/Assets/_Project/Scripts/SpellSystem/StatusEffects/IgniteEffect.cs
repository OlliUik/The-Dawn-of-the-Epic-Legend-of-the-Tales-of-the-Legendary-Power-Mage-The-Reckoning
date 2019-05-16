using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgniteEffect : StatusEffect
{

    public float damagePerTick      = 1f;
    public float timeBetweenTicks   = 1.0f;
    private float timer;

    public IgniteEffect(float duration, GameObject graphics, float damagePerTick, float timeBetweenTicks) : base(duration, graphics)
    {
        name = "Ignite";
        this.duration = duration;
        this.graphics = graphics;
        this.damagePerTick = damagePerTick;
        this.timeBetweenTicks = timeBetweenTicks;
    }


    private void Burn()
    {
        // deal damage to target
        timer = 0f;

        if (target != null)
        {
            var health = target.GetComponent<Health>();
            health.Hurt(damagePerTick, true);
        }
        else
        {
            Debug.Log("Ignite has no health to damage");
        }
    }

    public override void OnApply(GameObject target, List<StatusEffect> allEffectsInSpell)
    {

        base.OnApply(target, allEffectsInSpell);

        effectManager.AppliedEffects[StatusEffectManager.EffectType.Ignite] = true;
        endTime = Time.time + duration;

        CheckForCounterEffects(allEffectsInSpell);
    }

    public override void OnTick()
    {
        if (timer > timeBetweenTicks)
        {
            Burn();
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    public override void CheckForCounterEffects(List<StatusEffect> allEffectsInSpell)
    {
        if (effectManager == null)
        {
            Debug.Log("No StatusEffectManager found");
            return;
        }

        // check if target has moisturize applied
        var moisturize = (MoisturizeEffect)allEffectsInSpell.Find(x => x.GetType() == typeof(MoisturizeEffect));
        if (effectManager.AppliedEffects[StatusEffectManager.EffectType.Moisturize] || moisturize != null)
        {
            if (moisturize != null)
            {
                // spell contains moisturize --> reduce duration
                endTime = Time.time + (duration * 0.5f);
                return;
            }
            else
            {
                // spell doesn't contain moisturize --> remove moisturize from target
                effectManager.RemoveStatusEffect(effectManager.affectingEffects.Find(x => x.GetType() == typeof(MoisturizeEffect)));
                effectManager.RemoveStatusEffect(effectManager.affectingEffects.Find(x => x.GetType() == typeof(IgniteEffect)));
            }
        }

    }

    public override void OnLeave()
    {
        effectManager.AppliedEffects[StatusEffectManager.EffectType.Ignite] = false;
        base.OnLeave();
    }

}
