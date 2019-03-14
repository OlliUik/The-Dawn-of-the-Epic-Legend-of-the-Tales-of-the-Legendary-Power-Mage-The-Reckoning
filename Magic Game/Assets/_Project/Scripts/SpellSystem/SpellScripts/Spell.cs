using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{

    [Header("-- Spell --")]
    [SerializeField] protected float amount = 5.0f;
    [SerializeField] protected float cooldown = 5.0f;
    [SerializeField] protected float castTime = 5.0f;
    [SerializeField] protected float manaCost = 5.0f;

    public float Cooldown
    {
        get { return cooldown; }
        set { cooldown = value; }
    }
    public float CastTime
    {
        get { return castTime; }
        set { castTime = value; }
    }
    public float ManaCost
    {
        get { return manaCost; }
        set { manaCost = value; }
    }

    // most spells override this cause they require different logic
    public virtual void CastSpell(Spellbook spellbook, int spellIndex, Vector3 direction)
    {
        ///<summary>
        ///
        ///                                 SPELLS
        /// 
        ///     • Creates an instance of the spell player casts
        ///     • Loop through all the cards under the spell and apply all the balances and modifiers on it from all cards
        ///     • Every spell overrides this and does their own logic but the main spell loop stays the same
        ///     • Spells call spellbook.StopCasting() to set their own cooldown and allowing player to cast other spells
        /// 
        /// </summary>

        Spell spell = Instantiate(spellbook.spells[spellIndex].spell, spellbook.spellPos.position, spellbook.transform.rotation);
        foreach (Card card in spellbook.spells[spellIndex].cards)
        {
            foreach (SpellBalance balance in card.balances)
            {
                balance.ApplyBalance(spellbook);
            }
        }

        spellbook.StopCasting();

    }    

    public void ApplyModifiers(GameObject go, int spellIndex, Spellbook spellbook)
    {
        foreach (Card card in spellbook.spells[spellIndex].cards)
        {
            foreach (GameObject gameObject in card.spellModifiers)
            {
                SpellModifier[] mods = gameObject.GetComponents<SpellModifier>();
                foreach (SpellModifier mod in mods)
                {
                    mod.Apply(go);
                    print("Added: " + mod.name);
                }
            }
        }
    }
}
