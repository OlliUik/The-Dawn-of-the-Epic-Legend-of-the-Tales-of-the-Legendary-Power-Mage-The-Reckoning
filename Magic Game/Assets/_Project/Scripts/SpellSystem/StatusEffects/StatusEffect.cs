using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusEffect
{

    [HideInInspector] public string name;
    [HideInInspector] public GameObject target;
    [HideInInspector] public StatusEffectManager effectManager;
    [HideInInspector] public float endTime;

    public GameObject graphics;
    public float duration;

    protected GameObject graphicsCopy;

    // StatusEffectManager uses this
    public bool IsFinished
    {
        get { return Time.time > endTime; }
    }

    // Will be inherited with more parameters
    public StatusEffect(float duration, GameObject graphics) 
    {
        this.duration = duration;
        this.graphics = graphics;
    }

    // This will be called from each entitys own StatusEffectManager when the effect is about to be applied
    public virtual void OnApply(GameObject target, List<StatusEffect> allEffectsInSpell)
    {

        graphicsCopy = GameObject.Instantiate(graphics, target.transform.position, Quaternion.FromToRotation(-graphics.transform.up, Vector3.up));
        graphicsCopy.transform.SetParent(target.transform);

        this.target = target;
        effectManager = target.GetComponent<StatusEffectManager>();

    }

    // OnTick is used by effects like ignite / heal over time that need to be updated while applied to target
    public virtual void OnTick() { }

    // StatusEffectManager (on entity) calls OnLeave when the duration is passed or the countering effect is applied to the manager as new
    public virtual void OnLeave()
    {
        if (target != null)
        {
            GameObject.Destroy(graphicsCopy.gameObject);
        }
        else
        {
            Debug.Log("target is null");
        }
    }

    // esim. Moisturize overrides this and spawns water pool when hitting something that doesn't have health
    public virtual void HitNonlivingObject(Collision collision) { }

    // Refresh duration of effect and check for countering effects
    public virtual void ReApply(List<StatusEffect> allEffectsInSpell)
    {
        CheckForCounterEffects(allEffectsInSpell);
        endTime = Time.time + duration;
    }

    // Ignite and moisturize use this to check the existing StatusEffect and what are new effects in spell
    public virtual void CheckForCounterEffects(List<StatusEffect> allEffectsInSpell) { }

}
