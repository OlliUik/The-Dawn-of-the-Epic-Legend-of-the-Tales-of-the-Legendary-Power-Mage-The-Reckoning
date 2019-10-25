using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeVariables : MonoBehaviour
{
    
    public GameObject copyIceStunParticle;

    public float startRunSpeed;                    // what was the speed before started slowing
    public float startWalkSpeed;                    // what was the speed before started slowing
    public float startPanicSpeed;                    // what was the speed before started slowing

    public EnemyVision enemyVision;
    public EnemyNavigation enemyNav;

    public void freeze(float slowAmount, GameObject iceStunParticle, int cardAmount)
    {

        Debug.Log("Slow");

        // Enemy slow
        if (GetComponent<EnemyNavigation>() != null)
        {
            startRunSpeed = GetComponent<EnemyNavigation>().runningSpeed;
            GetComponent<EnemyNavigation>().runningSpeed *= 1 / slowAmount;
            startWalkSpeed = GetComponent<EnemyNavigation>().walkingSpeed;
            GetComponent<EnemyNavigation>().walkingSpeed *= 1 / slowAmount;
            startPanicSpeed = GetComponent<EnemyNavigation>().panicSpeed;
            GetComponent<EnemyNavigation>().panicSpeed *= 1 / slowAmount;

            //If card >= 3 enemy will be stuned
            if (cardAmount >= 3)
            {
                GetComponent<EnemyNavigation>().runningSpeed = 0;
                GetComponent<EnemyNavigation>().walkingSpeed = 0;
                GetComponent<EnemyNavigation>().panicSpeed = 0;
                //This doesn't work, enemies still able to attack player when stun. They stopped walking/running tho
                if (GetComponent<EnemyVision>() != null)
                {
                    enemyVision = GetComponent<EnemyVision>();
                    enemyVision.enabled = false;
                    if (iceStunParticle != null)
                    {
                        copyIceStunParticle = GameObject.Instantiate(iceStunParticle, transform.position, transform.rotation);
                        copyIceStunParticle.transform.parent = transform;
                    }
                }
            }

        }
    }

    private void OnDestroy()
    {
        //This doesn't work, enemies still able to attack player when stun
        if (GetComponent<EnemyNavigation>() != null)
        {
            GetComponent<EnemyNavigation>().runningSpeed = startRunSpeed;
            GetComponent<EnemyNavigation>().walkingSpeed = startWalkSpeed;
        }

        if (copyIceStunParticle != null)
        {
            GameObject.Destroy(copyIceStunParticle);
            copyIceStunParticle = null;
        }
        if (enemyNav != null)
        {
            enemyNav.enabled = true;
            enemyNav = null;
        }
        if (enemyVision != null)
        {
            enemyVision.enabled = true;
            enemyVision = null;
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
