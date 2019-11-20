using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricEffect : StatusEffect
{

    public float extraMoistureDamage = 0f;

    public override StatusEffect Clone()
    {
        ElectricEffect temp = new ElectricEffect(duration, graphics, extraManaCost, extraMoistureDamage);
        return temp;
    }

    public ElectricEffect(float duration, GameObject graphics, float extraManaCost, float extraMoistureDamage) : base(duration, graphics)
    {
        this.extraManaCost = extraManaCost;
        this.extraMoistureDamage = extraMoistureDamage;
    }

    public override void HitNonlivingObject(Collision collision)
    {
        var water = collision.collider.GetComponent<Water>();
        if (water != null)
        {
            if (!water.electric)
            {
                water.SetEletric(true, duration);
                // refresh duration?
            }
        }
    }

    public override void OnApply(GameObject target, List<StatusEffect> allEffectsInSpell)
    {
        GameObject.Find("ScoreUI").GetComponent<ScoreUI>().thunderstruck = true;
        base.OnApply(target, allEffectsInSpell);
        CheckForCounterEffects(allEffectsInSpell);
        if (target.GetComponent<ThunderVariables>() != null)
        {
            target.GetComponent<ThunderVariables>().duration = duration;
        }
        else
        {
            GameObject.Destroy(graphicsCopy);
            target.AddComponent<ThunderVariables>().Init(duration, extraManaCost, playerMana, graphics, extraMoistureDamage);
        }
    }

    public override void ReApply(List<StatusEffect> allEffectsInSpell)
    {
        base.ReApply(allEffectsInSpell);
        if (target.GetComponent<ThunderVariables>() != null)
        {
            target.GetComponent<ThunderVariables>().duration = duration;
        }
        else
        {
            GameObject.Destroy(graphicsCopy);
            target.AddComponent<ThunderVariables>().Init(duration, extraManaCost, playerMana, graphics, extraMoistureDamage);
        }
    }

    public override void CheckForCounterEffects(List<StatusEffect> allEffectsInSpell)
    {
        if (effectManager == null)
        {
            Debug.Log("No StatusEffectManager found");
            return;
        }

        // check if target has moisturize applied
        var moisturize = (MoisturizeEffect)allEffectsInSpell.Find(x => x.GetType() == typeof(MoisturizeEffect));
        if (effectManager.AppliedEffects[StatusEffectManager.EffectType.Moisturize] || moisturize != null)
        {
            if (moisturize != null)
            {
                // This shouldn't do anything? They both in the spell modifier so nothing to cancel.
                return;
            }
            else
            {
                // Target is in moisture condition
                // TODO: Take extra damage
            }
        }

    }

    public override void OnLeave()
    {
        GameObject.Find("ScoreUI").GetComponent<ScoreUI>().thunderstruck = false;
        if (target.GetComponent<ThunderVariables>() != null)
        {
            GameObject.Destroy(target.GetComponent<ThunderVariables>());
        }
    }

}
