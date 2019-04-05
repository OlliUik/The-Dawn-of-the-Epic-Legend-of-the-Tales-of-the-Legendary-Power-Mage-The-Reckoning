using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CostHealth ", menuName = "SpellSystem/Balances/Cost/Health")]
public class CostHealth : SpellBalance
{

    [SerializeField] private float amount = 10.0f;

    public override void ApplyBalance(Spellbook spellbook)
    {
        Health health = spellbook.GetComponent<Health>();
        health.Hurt(amount);
    }
}
