using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoisturizeEffect : StatusEffect
{

    public GameObject waterPoolPrefab;
    public float size = 1;

    private float timeBetweenTicks = 1.0f;
    private float timeFromLastRagdoll = 0f;

    public override StatusEffect Clone()
    {
        MoisturizeEffect temp = new MoisturizeEffect(duration, graphics, waterPoolPrefab, size);
        return temp;
    }

    public MoisturizeEffect(float duration, GameObject graphics, GameObject waterPoolPrefab, float size) : base(duration, graphics)
    {
        name = "Moisturize";
        this.duration = duration;
        this.graphics = graphics;
        this.waterPoolPrefab = waterPoolPrefab;
        this.size = size;
    }

    public override void OnApply(GameObject target, List<StatusEffect> allEffectsInSpell)
    {

        base.OnApply(target, allEffectsInSpell);

        endTime = Time.time + duration;
        effectManager.AppliedEffects[StatusEffectManager.EffectType.Moisturize] = true;

        CheckForCounterEffects(allEffectsInSpell);
    }

    public override void HitNonlivingObject(Collision collision)
    {
        var water = collision.collider.GetComponent<Water>();
        if(water == null)
        {
            GameObject waterPool = GameObject.Instantiate(waterPoolPrefab, collision.contacts[0].point, Quaternion.identity);
            waterPool.transform.localScale *= size;
        }
    }

    public override void CheckForCounterEffects(List<StatusEffect> allEffectsInSpell)
    {
        var ignite = (IgniteEffect)allEffectsInSpell.Find(x => x.GetType() == typeof(IgniteEffect));
        if (effectManager.AppliedEffects[StatusEffectManager.EffectType.Ignite] || ignite != null)
        {
            if (ignite != null)
            {
                /*
                // spell contains ignite --> reduce cooldown
                endTime = Time.time + (duration * 0.5f);
                */

                // Spell contains both moisturize and ignite, nothing to cancel.
                return;
            }
            else
            {
                // spell doesn't contain ignite --> remove ignite from target
                effectManager.RemoveStatusEffect(effectManager.affectingEffects.Find(x => x.GetType() == typeof(IgniteEffect)));
                effectManager.RemoveStatusEffect(effectManager.affectingEffects.Find(x => x.GetType() == typeof(MoisturizeEffect)));
            }
        }
    }

    public override void OnLeave()
    {
        effectManager.AppliedEffects[StatusEffectManager.EffectType.Moisturize] = false;
        base.OnLeave();
    }

    public override void OnTick()
    {
        if (timeFromLastRagdoll > timeBetweenTicks)
        {
            RagdollRandom();
        }
        else
        {
            timeFromLastRagdoll += Time.deltaTime;
        }
    }

    public void RagdollRandom()
    {
        timeFromLastRagdoll = 0;
        System.Random r = new System.Random();
        int ragdollChance = r.Next(0,100);
        if (ragdollChance > 100 - size*10 )
        {
            target.GetComponent<EnemyCore>().EnableRagdoll(true);
        }
    }

}
