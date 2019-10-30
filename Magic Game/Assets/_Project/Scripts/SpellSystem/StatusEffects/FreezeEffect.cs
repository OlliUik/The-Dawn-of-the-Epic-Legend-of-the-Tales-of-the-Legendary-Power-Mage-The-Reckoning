using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FreezeEffect : StatusEffect
{

    public float slowAmount = 5f;               // how much slows per spell hit
    public float moistSlowMultiplier = 1.5f;    // if target is moist how much more we slow default 1.5f
    public GameObject iceStunParticle;

    /*
     * Moved to FreezeVariables
     * 
    private bool isAlreadySlowed = false;
    private bool isStun = false;
    private EnemyVision enemyVision;
    private GameObject copyIceStunParticle;

    public float startRunSpeed;                    // what was the speed before started slowing
    public float startWalkSpeed;                    // what was the speed before started slowing
    public float startPanicSpeed;                    // what was the speed before started slowing
    */

    public int cardAmount = 1;

    #region Cloning
    public override StatusEffect Clone()
    {
        FreezeEffect temp = new FreezeEffect(duration, graphics, slowAmount, moistSlowMultiplier, iceStunParticle);
        temp.slowAmount = slowAmount;
        temp.moistSlowMultiplier = moistSlowMultiplier;
        temp.iceStunParticle = iceStunParticle;
        temp.cardAmount = cardAmount;
        return temp;
    }
    #endregion

    public FreezeEffect(float duration, GameObject graphics, float slowAmount, float moistSlowMultiplier, GameObject iceStunParticle) : base(duration, graphics)
    {
        name = "Freeze";
        this.duration = duration;
        this.slowAmount = slowAmount;
        this.moistSlowMultiplier = moistSlowMultiplier;
        this.graphics = graphics;
        this.iceStunParticle = iceStunParticle;
    }


    private void Slow()
    {

        // This is for player's
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
                movement.accelerationMultiplier = 0;
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
        Debug.Log("Reapply Freeze");
        base.OnApply(target, allEffectsInSpell);
        effectManager.AppliedEffects[StatusEffectManager.EffectType.Freeze] = true;
        endTime = Time.time + duration;
        Slow();
        if (target.GetComponent<EnemyCore>() != null && target.GetComponent<FreezeVariables>() == null)
        {
            FreezeVariables temp = target.AddComponent<FreezeVariables>();
            temp.freeze(slowAmount, iceStunParticle, cardAmount);
        }
    }

    public override void ReApply(List<StatusEffect> allEffectsInSpell)
    {
        // slow target again
        Debug.Log("Reapply");
        Slow();
        if (target.GetComponent<EnemyCore>() != null && target.GetComponent<FreezeVariables>() == null)
        {
            FreezeVariables temp = target.AddComponent<FreezeVariables>();
            temp.freeze(slowAmount, iceStunParticle, cardAmount);
        }
        base.ReApply(allEffectsInSpell);
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
        if(target.GetComponent<FreezeVariables>() != null)
        {
            GameObject.Destroy(target.GetComponent<FreezeVariables>());
        }
        effectManager.AppliedEffects[StatusEffectManager.EffectType.Freeze] = false;
        base.OnLeave();
    }

}
