using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricEffect : StatusEffect
{



    public ElectricEffect(float duration, GameObject graphics) : base(duration, graphics)
    {
    }

    public override void HitNonlivingObject(Collision collision)
    {
        var water = collision.collider.GetComponent<Water>();
        if(water != null)
        {
            if(!water.electric)
            {
                water.SetEletric(true, duration);
                // refresh duration?
            }
        }
    }

    public override void OnApply(GameObject target, List<StatusEffect> allEffectsInSpell)
    {
        // check if target is moist

        // deal extra damage if they are
    }
}
