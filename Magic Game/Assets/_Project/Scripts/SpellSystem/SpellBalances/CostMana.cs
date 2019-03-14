using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CostMana ", menuName = "Balance/Cost/Mana")]
public class CostMana : SpellBalance
{

    [SerializeField] private float amount = 10.0f;

    public override void ApplyBalance(Spellbook spellbook)
    {
        spellbook.playerCore.cMana.UseMana(amount);
    }
}
