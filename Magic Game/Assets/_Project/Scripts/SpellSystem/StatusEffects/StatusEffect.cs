using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{

    public float duration = 0f;
    public ScriptableEffect effect;
    public GameObject target;

    public StatusEffect(float duration, ScriptableEffect effect, GameObject target)
    {
        this.duration = duration;
        this.effect = effect;
        this.target = target;
    }

    public bool isFinished
    {
        get { return duration <= 0f; }
    }

    public abstract void OnBeginEffect();
    public abstract void OnLeaveEffect();

    public virtual void Tick(float delta)
    {
        duration -= delta;
        if(isFinished)
        {
            OnLeaveEffect();
        }
    }

}
