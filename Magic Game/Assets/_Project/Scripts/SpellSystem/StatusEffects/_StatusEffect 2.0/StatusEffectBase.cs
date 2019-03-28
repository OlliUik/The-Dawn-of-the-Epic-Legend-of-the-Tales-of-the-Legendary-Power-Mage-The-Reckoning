using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectBase : MonoBehaviour
{

    public GameObject target;

    public bool tick = false;
    public float duration;
    public float timeBetweenTicks = 1f;
    public float startDelay = 1f;

    //[HideInInspector] public Entity entity;
    [HideInInspector] public GameObject instance;

    public virtual void OnApplyEffect(GameObject target)
    {
        this.target = target;
        instance = Instantiate(gameObject, target.transform.position, target.transform.rotation);
        instance.transform.parent = target.transform;
        //entity = target.GetComponent<Entity>();

        if (tick)
        {
            InvokeRepeating("OnTick", startDelay, timeBetweenTicks);
        }

        Invoke("OnRemoveEffect", duration);
    }

    public virtual void OnTick()
    {

    }

    public virtual void OnRemoveEffect()
    {
        CancelInvoke();
        target.GetComponent<StatusEffectManagerBase>().statusEffects.Remove(this);
        Destroy(instance);
    }

}
