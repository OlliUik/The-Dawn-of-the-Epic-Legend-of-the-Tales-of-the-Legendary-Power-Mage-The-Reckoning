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

    public bool IsFinished
    {
        get { return Time.time > endTime; }
    }

    public StatusEffect(float duration, GameObject graphics) // will be inherited with more parameters
    {
        this.duration = duration;
        this.graphics = graphics;
    }


    public virtual void OnApply(GameObject target, List<StatusEffect> allEffectsInSpell)
    {

        graphics = GameObject.Instantiate(graphics, target.transform.position, Quaternion.FromToRotation(-graphics.transform.up, Vector3.up));
        graphics.transform.SetParent(target.transform);

        this.target = target;
        effectManager = target.GetComponent<StatusEffectManager>();

    }
    public virtual void OnTick() { } // this is used by effects like ignite / heal over time that need to be updated while applied to target

    public virtual void OnLeave()
    {
        if (target != null)
        {
            GameObject.Destroy(graphics);
        }
        else
        {
            Debug.Log("target is null");
        }
    }

    // refresh duration of effect and check for countering effects
    public virtual void ReApply(List<StatusEffect> allEffectsInSpell)
    {
        CheckForCounterEffects(allEffectsInSpell);
        endTime = Time.time + duration;
    }
    public virtual void CheckForCounterEffects(List<StatusEffect> allEffectsInSpell) { }

}
