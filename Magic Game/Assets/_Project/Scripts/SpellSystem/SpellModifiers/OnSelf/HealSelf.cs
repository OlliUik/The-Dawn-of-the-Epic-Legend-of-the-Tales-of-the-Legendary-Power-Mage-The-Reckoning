using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSelf : OnSelfModifier
{

    [SerializeField] private float healPerTick = 1.0f;
    [SerializeField] private float timeBetweenTicks = 1.0f;

    private Health health;

    private void Start()
    {
        health = GetComponentInParent<Health>();
    }

    public override void Apply(GameObject go)
    {
        go.AddComponent<HealSelf>();
    }

    float timer = 0f;
    private void Update()
    {
        if(timer <= 0f)
        {
            Heal();
            timer += timeBetweenTicks;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    private void Heal()
    {
        health.Heal(healPerTick);
    }
}
