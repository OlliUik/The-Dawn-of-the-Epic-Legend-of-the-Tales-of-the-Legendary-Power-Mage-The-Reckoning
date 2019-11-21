using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class will be added to the enemy when the enemy got hit by Freeze spell.
 */
public class FreezeVariables : MonoBehaviour
{

    public GameObject copyIceStunParticle;

    public float startRunSpeed;                    // what was the speed before started slowing
    public float startWalkSpeed;                    // what was the speed before started slowing
    public float startPanicSpeed;                    // what was the speed before started slowing

    public EnemyVision enemyVision;
    public EnemyNavigation enemyNav;
    public Spellbook spellbook;
    private bool isStun = false;

    private EnemyCore enemyCore;

    private bool originalCanCast = true;

    public void freeze(float slowAmount, GameObject iceStunParticle, int cardAmount, bool hasMoisture)
    {

        enemyNav = GetComponent<EnemyNavigation>();
        enemyVision = GetComponent<EnemyVision>();
        spellbook = GetComponent<Spellbook>();
        enemyCore = GetComponent<EnemyCore>();

        // Enemy slow
        if (enemyNav != null)
        {
            startRunSpeed = enemyNav.runningSpeed;
            enemyNav.runningSpeed *= 1 / slowAmount;
            startWalkSpeed = enemyNav.walkingSpeed;
            enemyNav.walkingSpeed *= 1 / slowAmount;
            startPanicSpeed = enemyNav.panicSpeed;
            enemyNav.panicSpeed *= 1 / slowAmount;
        }

        //If card >= 3 enemy will be stuned
        if (cardAmount >= 3 || hasMoisture)
        {
            if (enemyCore != null)
            {
                enemyCore.enabled = false;
            }
            isStun = true;
            if (enemyNav != null)
            {
                enemyNav.runningSpeed = 0;
                enemyNav.walkingSpeed = 0;
                enemyNav.panicSpeed = 0;
                enemyNav.enabled = false;
            }
            if (enemyVision != null)
            {
                enemyVision.enabled = false;
                if (iceStunParticle != null)
                {
                    copyIceStunParticle = GameObject.Instantiate(iceStunParticle, transform.position, transform.rotation);
                    copyIceStunParticle.transform.parent = transform;
                }
            }
            if (spellbook != null)
            {
                originalCanCast = spellbook.enableCasting;
                spellbook.enableCasting = false;
                spellbook.enabled = false;
            }
        }

    }

    private void OnDestroy()
    {
        if (enemyNav != null)
        {
            enemyNav.enabled = true;
            enemyNav.runningSpeed = startRunSpeed;
            enemyNav.walkingSpeed = startWalkSpeed;
            enemyNav.panicSpeed = startPanicSpeed;
        }
        if (copyIceStunParticle != null)
        {
            GameObject.Destroy(copyIceStunParticle);
            copyIceStunParticle = null;
        }
        if (enemyVision != null)
        {
            enemyVision.enabled = true;
        }
        if (spellbook != null)
        {
            spellbook.enabled = true;
            spellbook.enableCasting = originalCanCast;
        }
        if (enemyCore != null)
        {
            enemyCore.enabled = true;
        }
    }


    /*
    EnemyNavigation toRestore;

    private void Start()
    {
        toRestore = GetComponent<EnemyNavigation>();
    }

    public void Stun(float duration)
    {
        if (toRestore != null)
        {
            toRestore.enabled = false;
        }
    }

    IEnumerator destroyInSeconds(float duration)
    {
        yield return new WaitForSeconds(duration);

    }

    private void OnDestroy()
    {
        if (toRestore != null)
        {
            toRestore.enabled = true;
        }
    }
    */

}
