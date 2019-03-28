using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{

    public List<StatusEffect> appliedEffects = new List<StatusEffect>();
    public List<OnSelf> appliedOnSelfSpells = new List<OnSelf>();

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

        // stack and other options
    }

    private void RemoveStatusEffect(StatusEffect effect)
    {
        for (int i = 0; i < appliedEffects.Count; i++)
        {
            if(appliedEffects[i].Equals(effect))
            {
                effect.OnLeaveEffect();
                appliedEffects.Remove(appliedEffects[i]);
                // validate onselfs
                return;
            }
        }
        print("Error in removing effect");
    }

}
