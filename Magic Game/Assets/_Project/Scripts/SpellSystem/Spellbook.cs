using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spellbook : MonoBehaviour
{

    #region Variables

    private bool isPlayer = false;
    public bool isCasting = false;
    public Transform spellPos = null;

    public SpellData[] spells = new SpellData[3];
    private float[] cooldowns;

    public Transform lookTransform = null;
    private Vector3 charPositionOffset = Vector3.up * 1.0f;
    private Vector3 castPoint = Vector3.zero;
    [SerializeField] private LayerMask raycastLayerMask = 3073;

    // Components
    public Health health { get; private set; }
    public Mana mana { get; private set; }

    public bool enableCasting = true;

    public GameObject failureAudio; //audio

    #endregion

    private void Awake()
    {
        
    }

    private void Start()
    {
        if (GetComponent<PlayerCore>() != null)
        {
            isPlayer = true;
            lookTransform = Camera.main.transform;
        }
        else
        {
            // give enemies look / forward transform here

        }

        isCasting = false;
        cooldowns = new float[spells.Length];

        for (int i = 0; i < cooldowns.Length; i++)
        {
            cooldowns[i] = 0.0f;
        }

        if (GetComponent<PlayerCore>() != null)
        {
            isPlayer = true;
        }
        else
        {
            isPlayer = false;
        }

        health = GetComponent<Health>();
        mana = GetComponent<Mana>();

        ResetAllSpellManagersSingleton();
    }

    public void CastSpell(int spellIndex)
    {
        if (CanCast(spellIndex))
        {
            StartCoroutine(StartCastingSpell(spellIndex));
        }
    }

    // works but not centered --> // spells can get this by calling spellbook.GetDirection()
    public Vector3 GetDirection()
    {
        Vector3 direction = Vector3.zero;

        if (isPlayer)
        {
            RaycastHit hitFromCamera;
            RaycastHit hitFromPlayer;

            if (!Physics.Raycast(lookTransform.transform.position, lookTransform.transform.forward, out hitFromCamera, Mathf.Infinity, raycastLayerMask))
            {
                hitFromCamera.point = transform.position + charPositionOffset + (lookTransform.transform.position + lookTransform.transform.forward * 5000.0f);
            }

            if (Physics.Raycast(transform.position + charPositionOffset, -Vector3.Normalize(transform.position + charPositionOffset - hitFromCamera.point), out hitFromPlayer, Mathf.Infinity, raycastLayerMask))
            {
                castPoint = hitFromPlayer.point;
            }
            else
            {
                castPoint = hitFromCamera.point;
            }

            direction = -Vector3.Normalize(spellPos.position - castPoint);
        }
        else
        {
            //direction = -Vector3.Normalize(lookTransform.position - GetComponent<EnemyCore>().vision.targetLocation);
            Vector3 prediction = GetComponent<EnemyCore>().cVision.targetLocation;
            Projectile proj = spells[0].spell as Projectile;
            if (proj != null)
            {
                EnemyCore ec = GetComponent<EnemyCore>();
                prediction = ec.PredictTargetPosition(spellPos.position, proj.baseSpeed, ec.cVision.targetLocation, ec.status.isConfused ? ec.cVision.targetGO.GetComponent<UnityEngine.AI.NavMeshAgent>().velocity : ec.cVision.targetGO.GetComponent<CharacterController>().velocity);
            }
            direction = -Vector3.Normalize(lookTransform.position - prediction);
        }
        
        return direction;

    }
    // fires directly towards mouse cursor
    //public Vector3 GetDirection2()
    //{
    //    Vector3 direction = Vector3.zero;

    //    RaycastHit hit;
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

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
                    Instantiate(failureAudio, new Vector3(0, 0, 0), Quaternion.identity); //audio
                    return false;
                }
            }
        }

        // check is spell on cooldown
        if(cooldowns[spellIndex] > Time.time)
        {
            print("Spell is on cooldown");
            Instantiate(failureAudio, new Vector3(0, 0, 0), Quaternion.identity); //audio
            return false;
        }

        // check if player is already casting something
        if(isCasting)
        {
            return false;
        }

        if (!enableCasting)
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


    /// This func should be called when some spell is about to be casted
    /// We should pass an animation as parameter (how the spell is charged)
    /// After charging the spell the animation can call "CastSpell()" with anim event
    private IEnumerator StartCastingSpell(int spellIndex)
    {
        isCasting = true;
        float castingTime = GetCastingTime(spellIndex);

        // use players mana 
        if(mana != null)
        {
            mana.UseMana(spells[spellIndex].spell.ManaCost);
        }

        if(spells[spellIndex].cards.Count > 0)
        {
            foreach (Card card in spells[spellIndex].cards)
            {
                foreach (SpellBalance balance in card.balances)
                {
                    // take spell extra cost here
                    balance.ApplyBalance(this);
                }
            }
        }

        // check if something modifies speed etc. while casting and apply effect here
        // also save them in temp list and remove effects after casting
        // do casting animation here
        yield return new WaitForSeconds(castingTime);

        spells[spellIndex].spell.CastSpell(this, spells[spellIndex]);
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

    // Reset all instances of the spell
    private void ResetAllSpellManagersSingleton()
    {
        PortalGateManager.ResetVariables();
        MeteorManager.ResetVariables();
    }

}
