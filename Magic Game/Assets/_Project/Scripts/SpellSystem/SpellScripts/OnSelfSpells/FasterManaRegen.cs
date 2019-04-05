using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FasterManaRegen : OnSelf
{
   
    [SerializeField] private float extraRegenMultiplier = 0.2f;
    [SerializeField] private float duration = 10f;
    private Mana mana;

    public override void CastSpell(Spellbook spellbook, SpellData data)
    {

        print("Extra mana regen activate");
        this.spellbook = spellbook;
        mana = spellbook.GetComponent<Mana>();

        if(mana != null)
        {
            mana.regenerationMultiplier += extraRegenMultiplier;
            // set on cooldown
        }
        //spellbook.playerCore.cMana.regenerationMultiplier += extraRegenMultiplier;
        //spellbook.onSelfCooldown = Time.time + spellbook.selfSpells[spellIndex].Cooldown;
        Invoke("RemoveEffect", duration);

    }

    public override void RemoveEffect()
    {
        print("Extra mana regen exterminate");

        if(mana != null)
        {
            mana.regenerationMultiplier -= extraRegenMultiplier;
        }
        //spellbook.playerCore.cMana.regenerationMultiplier -= extraRegenMultiplier;
    }

}
