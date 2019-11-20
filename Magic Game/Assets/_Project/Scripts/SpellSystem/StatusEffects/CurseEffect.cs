using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CurseEffect : StatusEffect
{

    public float amountPerStack = 1f;               // how much slows per spell hit

    #region Cloning
    public override StatusEffect Clone()
    {
        CurseEffect temp = new CurseEffect(duration, graphics, amountPerStack);
        return temp;
    }
    #endregion

    public CurseEffect(float duration, GameObject graphics, float amountPerStack) : base(duration, graphics)
    {
        name = "Curse";
        this.duration = duration;
        this.amountPerStack = amountPerStack;
        this.graphics = graphics;
    }

    public override void OnApply(GameObject target, List<StatusEffect> allEffectsInSpell)
    {
        Debug.Log("Reapply Freeze");
        base.OnApply(target, allEffectsInSpell);
        effectManager.AppliedEffects[StatusEffectManager.EffectType.Curse] = true;
        //TODO: Add curse component?
        endTime = Time.time + duration;
    }

    public override void ReApply(List<StatusEffect> allEffectsInSpell)
    {
        Debug.Log("Reapply");
        //This reset the duration
        base.ReApply(allEffectsInSpell);
    }

    public override void OnLeave()
    {
        //TODO: Remove curse component?
        effectManager.AppliedEffects[StatusEffectManager.EffectType.Curse] = false;
        base.OnLeave();
    }

}
