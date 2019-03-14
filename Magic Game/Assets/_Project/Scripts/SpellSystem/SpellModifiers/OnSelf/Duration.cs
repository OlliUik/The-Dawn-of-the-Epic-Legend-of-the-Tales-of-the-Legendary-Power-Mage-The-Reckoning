using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "OnSelf/Modifier Duration", fileName = "Duration ")]
public class Duration : SpellBalance
{
    public float duration = 10f;

    public override void ApplyBalance(Spellbook spellbook)
    {
        // abstract has to be here
    }

    public float GetDuration()
    {
        return duration;
    }

}
