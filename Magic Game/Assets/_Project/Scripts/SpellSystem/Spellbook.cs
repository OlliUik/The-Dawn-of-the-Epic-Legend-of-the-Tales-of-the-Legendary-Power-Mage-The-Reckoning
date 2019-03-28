using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerCore))]
public class Spellbook : MonoBehaviour
{

    #region Variables

    [SerializeField] public bool isCasting              = false;
    public Transform spellPos                           = null;

    public SpellData[] spells                           = new SpellData[3];
    private float[] cooldowns;

    public OnSelf[] selfSpells                          = new OnSelf[2];
    [HideInInspector] public float onSelfCooldown       = 0.0f;
    [HideInInspector] public int selfIndex              = 0;
    public KeyCode onSelfKey                            = KeyCode.None;
    public KeyCode onSelfSwapKey                        = KeyCode.None;

    public PlayerCore playerCore                        { get; private set; }

    private Camera cam                                  = null;
    private Vector3 charPositionOffset                  = Vector3.up * 1.0f;
    private Vector3 castPoint                           = Vector3.zero;

    #endregion

    private void Awake()
    {
        cam = Camera.main;
        playerCore = GetComponent<PlayerCore>();
    }

    private void Start()
    {
        isCasting = false;
        cooldowns = new float[spells.Length];

        for (int i = 0; i < cooldowns.Length; i++)
        {
            cooldowns[i] = 0.0f;
        }
    }

    // Inputs
    void Update()
    {
        // COMBAT SPELLS
        for (int i = 0; i < spells.Length; i++)
        {
            if(Input.GetKeyDown(spells[i].castKey) && CanCast(i))
            {
                StartCoroutine(StartCastingSpell(i));
            }
        }

        // ON_SELF SPELLS
        if(Input.GetKeyDown(onSelfKey) && Time.time > onSelfCooldown)
        {
            // check if OnSelf is not on cooldown
            if(selfSpells[selfIndex] != null)
            {
                selfSpells[selfIndex].CastSpell(this, selfIndex);
            }
        }
        else if(Input.GetKeyDown(onSelfSwapKey))
        {
            if(selfSpells[1] != null)
            {
                selfIndex += 1;

                if(selfIndex > 1)
                {
                    selfIndex = 0;
                }

                print("OnSelf swapped to " + selfSpells[selfIndex].name);
                return;
            }

            print("Only one OnSelf spell");
        }

        // check if some self spell is invoking and cancel if needed
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    if(activeOnSelfSpell.IsInvoking())
        //    {
        //        activeOnSelfSpell.RemoveEffect();
        //        activeOnSelfSpell.CancelInvoke();
        //        print(activeOnSelfSpell + " effect ended early");
        //    }
        //}

    }

    // works but not centered --> // spells can get this by calling spellbook.GetDirection()
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
    //private bool CanCastOnSelf()
    //{
    //    foreach (Card card in onSelfSpell.collectedOnSelfSpells[onSelfIndex].cards)
    //    {
    //        // check that every cards requirements are met before doing anything
    //        foreach (CastRequirement requirement in card.castRequirements)
    //        {
    //            if (!requirement.isMet(this))
    //            {
    //                print(requirement.name + " was not met");
    //                return false;
    //            }
    //        }
    //    }

    //    // check cooldown
    //    if(activeOnSelfSpell.Cooldown > Time.time)
    //    {
    //        print("OnSelf on cooldown");
    //        return false;
    //    }

    //    // check if player is already casting something
    //    if (isCasting)
    //    {
    //        return false;
    //    }

    //    return true;
    //}

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

    private IEnumerator StartCastingSpell(int spellIndex)
    {
        isCasting = true;
        float castingTime = GetCastingTime(spellIndex);

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
