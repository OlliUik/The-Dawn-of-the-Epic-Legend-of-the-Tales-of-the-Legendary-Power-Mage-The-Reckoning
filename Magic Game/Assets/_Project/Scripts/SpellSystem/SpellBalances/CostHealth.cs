using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CostHealth ", menuName = "Balance/Cost/Health")]
public class CostHealth : SpellBalance
{

    [SerializeField] private float amount = 10.0f;

    public override void ApplyBalance(Spellbook spellbook)
    {
        spellbook.playerCore.cHealth.Hurt(amount);
    }
}
