using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{

    public List<StatusEffect> appliedEffects = new List<StatusEffect>();


    void Update()
    {

        List<StatusEffect> copy = new List<StatusEffect>(appliedEffects);
        foreach (StatusEffect effect in copy)
        {
            effect.Tick(Time.deltaTime);
            if(effect.isFinished)
            {
                RemoveStatusEffect(effect);
            }
        }

    }

    public void AddStatusEffect(StatusEffect effect)
    {
        if(!appliedEffects.Contains(effect))
        {
            appliedEffects.Add(effect);
            effect.OnBeginEffect();
            return;
        }

    }

    private void RemoveStatusEffect(StatusEffect effect)
    {
        for (int i = 0; i < appliedEffects.Count; i++)
        {
            if(appliedEffects[i].Equals(effect))
            {
                effect.OnLeaveEffect();
                appliedEffects.Remove(appliedEffects[i]);
                return;
            }
        }
        print("Error in removing effect");
    }

}
