using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTime : StatusEffect
{

    private ScriptableDot dot;
    private Health health;

    public DamageOverTime(float duration, ScriptableEffect effect, GameObject target) : base(duration, effect, target)
    {
        health = target.GetComponent<Health>();
        dot = (ScriptableDot)effect;
    }

    public override void OnBeginEffect()
    {
        ScriptableDot dot = (ScriptableDot)effect;
        health.Hurt(dot.damagePerTick, true);
    }

    private float timer = 1f;
    public override void Tick(float delta)
    {
        base.Tick(delta);
        if(timer <= 0f)
        {
            OnBeginEffect();
            timer += dot.timeBetweenTicks;
        }
        else
        {
            timer -= delta;
        }
    }

    public override void OnLeaveEffect()
    {

    }
}
