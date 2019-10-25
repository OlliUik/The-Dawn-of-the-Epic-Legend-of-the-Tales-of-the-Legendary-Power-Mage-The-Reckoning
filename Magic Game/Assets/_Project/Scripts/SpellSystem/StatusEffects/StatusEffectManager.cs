using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{

    #region Variables

    public enum EffectType
    {
        Freeze,
        Ignite,
        Moisturize,
        Confuse,
        StackingDamage,
        LeechLife,
        Stun,
    };

    public List<StatusEffect> affectingEffects = new List<StatusEffect>();
    public Dictionary<EffectType, bool> AppliedEffects = new Dictionary<EffectType, bool>();

    #endregion

    #region Unity_Methods

    private void Awake()
    {
        AppliedEffects.Add(EffectType.Freeze, false);
        AppliedEffects.Add(EffectType.Ignite, false);
        AppliedEffects.Add(EffectType.Moisturize, false);
        AppliedEffects.Add(EffectType.Confuse, false);
        AppliedEffects.Add(EffectType.StackingDamage, false);
        AppliedEffects.Add(EffectType.LeechLife, false);
        AppliedEffects.Add(EffectType.Stun, false);
    }

    // Loop through all the affecting StatusEffects and call OnTick()
    private void Update()
    {
        for (int i = 0; i < affectingEffects.Count; i++)
        {
            affectingEffects[i].OnTick();

            if (affectingEffects[i].IsFinished)
            {
                RemoveStatusEffect(affectingEffects[i]);
            }
        }
    }

    #endregion

    #region Custom_Methods

    // Spells call this if they contain StatusEffects as cards
    // if entity already has current effect call ReApply() on it
    public void ApplyStatusEffect(StatusEffect effect, List<StatusEffect> allEffectsInSpell)
    {
        for (int i = 0; i < affectingEffects.Count; i++)
        {
            if (affectingEffects[i].GetType() == effect.GetType())
            {
                // same effect already exisit
                affectingEffects[i].ReApply(allEffectsInSpell);
                return;
            }
        }

        // if we get this far the effect is new       
        affectingEffects.Add(effect);
        effect.OnApply(gameObject, allEffectsInSpell);
    }

    // Update calls this when StatusEffects duration ends or when countering effect is applied
    public void RemoveStatusEffect(StatusEffect effect)
    {
        for (int i = 0; i < affectingEffects.Count; i++)
        {
            if (affectingEffects[i].GetType() == effect.GetType())
            {
                // found the effect we want to remove
                Debug.Log("Removed " + affectingEffects[i].name);
                affectingEffects[i].OnLeave();
                affectingEffects.RemoveAt(i);
                return;
            }
        }
    }

    #endregion

}
