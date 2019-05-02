using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{

    public GameObject caster;

    [Header("Spell")]
    [SerializeField] protected float cooldown = 5.0f;
    [SerializeField] protected float castTime = 5.0f;
    [SerializeField] protected float manaCost = 5.0f;
    public List<StatusEffect> statusEffects = new List<StatusEffect>();

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
    public virtual void CastSpell(Spellbook spellbook, SpellData data)
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

        Spell spell = Instantiate(data.spell, spellbook.spellPos.position, spellbook.transform.rotation);
        spell.caster = spellbook.gameObject;

        foreach (Card card in data.cards)
        {
            foreach (SpellBalance balance in card.balances)
            {
                balance.ApplyBalance(spellbook);
            }
        }

        spellbook.StopCasting();

    }    

    public virtual void ApplyModifiers(GameObject go, SpellData data)
    {
        Spell spell = go.GetComponent<Spell>();

        foreach (Card card in data.cards)
        {
            // add all modifiers to spell
            foreach (SpellScriptableModifier modifier in card.modifiers)
            {
                modifier.AddSpellModifier(go);
                print("Added: " + modifier.name);
            }

            /* Graphics old

            // check if card has graphics assained if so add them to the spell
            if(card.graphics != null)
            {
                // first check if spell has primary graphics
        
                // if not add these 
                GameObject graphics = Instantiate(card.graphics, go.transform.position, card.graphics.transform.rotation);
                graphics.transform.SetParent(go.transform);
                break; // if primary graphics get added don't and secendary of the same ( fire particles on fire projectile )
            }
        
            if(card.secendaryGraphics != null)
            {
                // card has some secendary graphics
                GameObject graphics = Instantiate(card.secendaryGraphics, go.transform.position, go.transform.rotation);
                graphics.transform.parent = go.transform;
            }

            */        

        }

        SpellModifier[] modifiers = go.GetComponents<SpellModifier>();
        foreach (SpellModifier mod in modifiers)
        {
            mod.OnSpellCast(spell);
        }

    }

    public virtual void DealDamage(Health health, float amount)
    {
        health.Hurt(amount);
    }

    public virtual void ApplyStatusEffects(StatusEffectManager manager, List<StatusEffect> effects)
    {
        // call ApplyStatusEffect in the hitObjects StatusEffectManager and do null checks there
        foreach (StatusEffect effect in statusEffects)
        {
            manager.ApplyStatusEffect(effect, statusEffects);
        }
    }

}
