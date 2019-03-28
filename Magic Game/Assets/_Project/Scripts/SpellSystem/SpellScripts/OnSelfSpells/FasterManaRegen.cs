using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FasterManaRegen : OnSelf
{
   
    [SerializeField] private float extraRegenMultiplier = 0.2f;
    [SerializeField] private float duration = 10f;

    public override void CastSpell(Spellbook spellbook, SpellData data)
    {

        print("Extra mana regen activate");
        this.spellbook = spellbook;
        //spellbook.playerCore.cMana.regenerationMultiplier += extraRegenMultiplier;
        //spellbook.onSelfCooldown = Time.time + spellbook.selfSpells[spellIndex].Cooldown;
        Invoke("RemoveEffect", duration);

    }

    public override void RemoveEffect()
    {
        print("Extra mana regen exterminate");
        //spellbook.playerCore.cMana.regenerationMultiplier -= extraRegenMultiplier;
    }

}
