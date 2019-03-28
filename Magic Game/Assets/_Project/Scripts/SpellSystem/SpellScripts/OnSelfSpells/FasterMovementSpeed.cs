using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FasterMovementSpeed : OnSelf
{

    [SerializeField] private float movementSpeedIncrease = 0f;
    [SerializeField] private float duration = 10f;

    public override void CastSpell(Spellbook spellbook, SpellData data)
    {

        print("Extra movementspeed activate");
        this.spellbook = spellbook;
        //spellbook.playerCore.cMovement.accelerationMultiplier += movementSpeedIncrease;
        //spellbook.onSelfCooldown = Time.time + spellbook.selfSpells[spellIndex].Cooldown;
        Invoke("RemoveEffect", duration);

    }

    public override void RemoveEffect()
    {
        print("Extra movementspeed exterminate");
        //spellbook.playerCore.cMovement.accelerationMultiplier -= movementSpeedIncrease;
    }

}
