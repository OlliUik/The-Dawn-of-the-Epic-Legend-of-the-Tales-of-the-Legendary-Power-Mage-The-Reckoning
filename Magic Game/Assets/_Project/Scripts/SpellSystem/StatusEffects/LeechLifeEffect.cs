using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeechLifeEffect : StatusEffect
{

    // variables
    public GameObject caster;
    public float healthPerTick = 1f;
    public float timeBetweenTicks = 1f;
    private float timer;

    private Health giving, resiving;

    public LeechLifeEffect(float duration, GameObject graphics, float healthPerTick, float timeBetweenTicks, GameObject caster) : base(duration, graphics)
    {
        name = "LeechLife";
        this.duration = duration;
        this.graphics = graphics;
        this.healthPerTick = healthPerTick;
        this.timeBetweenTicks = timeBetweenTicks;
        this.caster = caster;
    }

    public override void OnApply(GameObject target, List<StatusEffect> allEffectsInSpell)
    {
        //graphicsCopy = GameObject.Instantiate(graphics, target.transform.position + (Vector3.up * 1f), Quaternion.FromToRotation(-graphics.transform.up, Vector3.up));
        //graphicsCopy.transform.SetParent(target.transform);
        this.target = target;
        giving = target.GetComponent<Health>();
        resiving = caster.GetComponent<Health>();
        effectManager = target.GetComponent<StatusEffectManager>();
        effectManager.AppliedEffects[StatusEffectManager.EffectType.LeechLife] = true;

        endTime = Time.time + duration;
    }

    public override void OnTick()
    {
        if (timer > timeBetweenTicks)
        {
            Tick();
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    private void Tick()
    {
        timer = 0f;

        if (target != null)
        {
            if(giving != null)
            {
                giving.Hurt(healthPerTick, true);
            }

            if(resiving != null)
            {
                resiving.Heal(healthPerTick);
            }
        }
        else
        {
            Debug.Log("LeechLife has no health to damage");
        }
    }

    public override void OnLeave()
    {
        effectManager.AppliedEffects[StatusEffectManager.EffectType.LeechLife] = false;
        base.OnLeave();
    }
}
