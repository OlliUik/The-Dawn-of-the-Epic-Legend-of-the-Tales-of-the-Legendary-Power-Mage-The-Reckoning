using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManagerBase : MonoBehaviour
{

    public List<StatusEffectBase> statusEffects = new List<StatusEffectBase>();


    public void AddStatusEffect(StatusEffectBase effect)
    {
        if(!statusEffects.Contains(effect))
        {
            statusEffects.Add(effect);
            effect.OnApplyEffect(gameObject);
        }
        else
        {
            // entity already has StatusEffect

            // stack it ?

            // refresh duration ?

        }
    }

    public void RemoveStatusEffect(StatusEffectBase effect)
    {
        statusEffects.Remove(effect);
        effect.OnRemoveEffect();
    }

}
