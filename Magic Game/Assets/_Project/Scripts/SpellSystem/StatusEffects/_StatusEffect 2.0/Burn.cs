using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : StatusEffectBase
{

    [SerializeField] private float damagePerTick = 1f;
    private Health health;

    public override void OnApplyEffect(GameObject target)
    {
        this.target = target;
        instance = Instantiate(gameObject, target.transform.position, target.transform.rotation);
        instance.transform.parent = target.transform;
        health = target.GetComponent<Health>();

        if (tick)
        {
            InvokeRepeating("OnTick", startDelay, timeBetweenTicks);
        }

        Invoke("OnRemoveEffect", duration);
    }

    public override void OnTick()
    {
        health.Hurt(damagePerTick, true);
    }
}
