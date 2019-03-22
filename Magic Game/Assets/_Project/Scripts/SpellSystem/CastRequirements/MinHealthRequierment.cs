using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinHealth ", menuName = "SpellSystem/CastRequirements/MinHealthRequirement")]
public class MinHealthRequierment : CastRequirement
{
    [SerializeField] private float reuiqredAmount = 10.0f;

    public override bool isMet(Spellbook spellbook)
    {
        if(spellbook.playerCore.cHealth.health >= reuiqredAmount)
        {
            return true;
        }
        return false;
    }
}
