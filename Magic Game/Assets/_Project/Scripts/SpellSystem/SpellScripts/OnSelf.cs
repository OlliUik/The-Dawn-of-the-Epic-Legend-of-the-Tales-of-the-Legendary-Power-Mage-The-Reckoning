using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSelf : Spell
{

    [Header("OnSelf variables")]
    [SerializeField] private float dur = 0f;

    public override void CastSpell(Spellbook spellbook, int spellIndex)
    {

        ///<summary>
        ///
        ///                                 SELF SPELLS ARE DIFFERENT
        /// 
        ///     • self spells affect players directly so there is no need to instansiate the prefab
        ///     • on self spells work in a few ways
        ///         • They take instant effect on player (teleport a little distance forwards, etc.)
        ///         • They modify player stats directly (health, movementspeed, etc.) // heal, haste
        ///         • Give player a triple jump until he uses all of them ????
        ///         • Fix this shit --> no need to create instance just apply effects directly to player
        /// 
        /// </summary>

        // TODO:: prototype of self castable spell

        OnSelf self = Instantiate(this, spellbook.transform.position, spellbook.transform.rotation);
        self.transform.parent = spellbook.transform;

        ApplyModifiers(self.gameObject, spellIndex, spellbook);
        self.dur = GetDuration(spellbook, spellIndex);

        spellbook.StopCasting();

    }

    private float GetDuration(Spellbook spellbook, int spellIndex)
    {
        dur = 0f;

        Card[] cards = spellbook.spells[spellIndex].cards.ToArray();
        foreach (Card card in cards)
        {
            SpellBalance[] balances = card.balances.ToArray();
            foreach (SpellBalance balance in balances)
            {
                if(balance.GetType() == typeof(Duration))
                {
                    Duration duration = (Duration)balance;
                    if(duration.GetDuration() > dur)
                    {
                        dur = duration.GetDuration();
                    }
                }
            }
        }
        print("Longest duration: " + dur);
        return dur;
    }

    // destroy whole gameObject after longest duration has passed
    private void Update()
    {
        if(dur > 0f)
        {
            dur -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
