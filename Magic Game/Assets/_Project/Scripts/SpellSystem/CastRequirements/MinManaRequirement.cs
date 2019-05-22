using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinMana ", menuName = "SpellSystem/CastRequirements/MinManaRequirement")]
public class MinManaRequirement : CastRequirement
{
    [SerializeField] private float requiredAmount = 10.0f;

    public override bool isMet(Spellbook spellbook)
    {
        Mana mana = spellbook.mana;
        if(mana != null)
        {
            if(mana.mana >= requiredAmount)
            {
                return true;
            }
        }
        return false;
    }
}
