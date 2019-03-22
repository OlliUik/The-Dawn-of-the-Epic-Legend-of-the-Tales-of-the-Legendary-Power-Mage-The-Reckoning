using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpellData 
{
    public Spell spell;         // determinates how spell is casted and what happens on cast
    public List<Card> cards;    // all modifiers are here
    // TODO:: keep track of cooldown here
}

[RequireComponent(typeof(PlayerCore))]
public class Spellbook : MonoBehaviour
{

    #region Variables

    [SerializeField] public bool isCasting              = false;
    public Transform spellPos                           = null;

    public SpellData[] spells                           = new SpellData[4];
    private float[] cooldowns                           = new float[4];

    public PlayerCore playerCore                        { get; private set; }

    private Camera cam                                  = null;
    private Vector3 charPositionOffset                  = Vector3.up * 1.0f;
    private Vector3 castPoint                           = Vector3.zero;

    #endregion

    private void Start()
    {
        cam = Camera.main;
        playerCore = GetComponent<PlayerCore>();

        for (int i = 0; i < cooldowns.Length; i++)
        {
            cooldowns[i] = 0.0f;
        }

        isCasting = false;
    }

    // Inputs
    void Update()
    {

        Vector3 direction = GetDirection();

        if (Input.GetKeyDown(KeyCode.Alpha1) && CanCast(0))
        {
            float castingTime = GetCastingTime(0);
            StartCoroutine(StartCastingSpell(0, castingTime, direction));            
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && CanCast(1))
        {
            float castingTime = GetCastingTime(1);
            StartCoroutine(StartCastingSpell(1, castingTime, direction));
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && CanCast(2))
        {
            float castingTime = GetCastingTime(2);
            StartCoroutine(StartCastingSpell(2, castingTime, direction));
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && CanCast(3))
        {
            float castingTime = GetCastingTime(3);
            StartCoroutine(StartCastingSpell(3, castingTime, direction));
        }
    }

    // works but not centered
    public Vector3 GetDirection()
    {
        Vector3 direction = Vector3.zero;

        RaycastHit hitFromCamera;
        RaycastHit hitFromPlayer;

        if (!Physics.Raycast(cam.transform.position, cam.transform.forward, out hitFromCamera, Mathf.Infinity))
        {
            hitFromCamera.point = transform.position + charPositionOffset + (cam.transform.position + cam.transform.forward * 5000.0f);
        }

        if (Physics.Raycast(transform.position + charPositionOffset, -Vector3.Normalize(transform.position + charPositionOffset - hitFromCamera.point), out hitFromPlayer, Mathf.Infinity))
        {
            castPoint = hitFromPlayer.point;
        }
        else
        {
            castPoint = hitFromCamera.point;
        }

        direction = -Vector3.Normalize(transform.position + charPositionOffset - castPoint);
        return direction;

    }

    // fires directly towards mouse cursor
    //public Vector3 GetDirection2()
    //{
    //    Vector3 direction = Vector3.zero;

    //    RaycastHit hit;
    //    Ray ray = cam.ScreenPointToRay(Input.mousePosition);

    //    if(Physics.Raycast(ray, out hit))
    //    {
    //        direction = (hit.point - spellPos.position).normalized;
    //    }
    //    else
    //    {
    //        direction = ray.direction.normalized;
    //    }

    //    return direction;
    //}

    private bool CanCast(int spellIndex)
    {
        // loop through all cards
        foreach (Card card in spells[spellIndex].cards)
        {
            // check that every cards requirements are met before doing anything
            foreach (CastRequirement requirement in card.castRequirements)
            {
                if(!requirement.isMet(this))
                {
                    print(requirement.name + " was not met");
                    return false;
                }
            }
        }

        // check is spell on cooldown
        if(cooldowns[spellIndex] > Time.time)
        {
            print("Spell is on cooldown");
            return false;
        }

        // check if player is already casting something
        if(isCasting)
        {
            return false;
        }

        return true;
    }

    private float GetCastingTime(int spellIndex)
    {
        float castingTime = spells[spellIndex].spell.CastTime;

        foreach (Card card in spells[spellIndex].cards)
        {
            // get all balances that effect the cast time of the spell
            for (int i = 0; i < card.balances.Count; i++)
            {
                if (card.balances[i].GetType() == typeof(CastTime))
                {
                    CastTime ct = (CastTime)card.balances[i];
                    castingTime += ct.GetCastingTime();
                    print("New casting time is: " + castingTime);
                }
            }
        }
        return castingTime;
    }

    private IEnumerator StartCastingSpell(int spellIndex, float castingTime, Vector3 direction)
    {
        isCasting = true;

        // use players mana 
        playerCore.cMana.UseMana(spells[spellIndex].spell.ManaCost);

        foreach (Card card in spells[spellIndex].cards)
        {
            foreach (SpellBalance balance in card.balances)
            {
                // take spell extra cost here
                balance.ApplyBalance(this);
            }
        }

        // check if something modifies speed etc. while casting and apply effect here
        // also save them in temp list and remove effects after casting
        // do casting animation here
        yield return new WaitForSeconds(castingTime);

        spells[spellIndex].spell.CastSpell(this, spellIndex);
        lastCastedSpell = spellIndex;
    }

    public void StopCasting()
    {
        isCasting = false;
        SetCooldown();
    }

    int lastCastedSpell;
    public void SetCooldown()
    {
        cooldowns[lastCastedSpell] = Time.time + spells[lastCastedSpell].spell.Cooldown; // check if some spells card has extra cooldown and add it here
        isCasting = false;
    }
}
