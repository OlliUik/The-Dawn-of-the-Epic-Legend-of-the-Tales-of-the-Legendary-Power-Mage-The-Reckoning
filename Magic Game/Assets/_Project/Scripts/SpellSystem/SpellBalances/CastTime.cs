using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Extra cast time ", menuName = "SpellSystem/Balances/Casting/CastTime")]
public class CastTime : SpellBalance
{

    [SerializeField] private float amount = 3.0f;

    public override void ApplyBalance(Spellbook spellbook)
    {
        // longer castTime
    }

    public float GetCastingTime()
    {
        return amount;
    }
}
