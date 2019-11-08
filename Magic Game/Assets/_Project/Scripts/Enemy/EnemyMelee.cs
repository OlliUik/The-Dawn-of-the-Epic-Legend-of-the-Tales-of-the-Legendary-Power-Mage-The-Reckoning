using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : EnemyCore
{
    [Header("Melee -> Attacking")]
    [SerializeField] public float meleeAttackDistance = 10f;
    [SerializeField] public float meleeDamage = 25.0f;
    [SerializeField] public GameObject hammer;



    protected override void AIAttack()
    {
        if (cVision.bCanSeeTarget)
        {
            if ((transform.position - cVision.targetLocation).sqrMagnitude > meleeAttackDistance * meleeAttackDistance)
            {
                return;
            }

            castStandStillTimer = standStillAfterCasting;
            // animator.SetTrigger("Cast Spell");
            // animator.SetInteger("Spell Type", attackAnimation);


            if (!moveWhileCasting && cNavigation.cAgent.hasPath)
            {
                cNavigation.cAgent.ResetPath();
                cNavigation.cAgent.velocity = new Vector3(0.0f, cNavigation.cAgent.velocity.y, 0.0f);
            }

            if((transform.position - cVision.targetLocation).sqrMagnitude >= 8f)
            {

            }

            StartCoroutine(startAttack());
            //animator.SetBool("isIdle", false);
            //animator.SetBool("isWalking", false);
            //animator.SetBool("isAttack", true);
            hammer.GetComponent<MeshCollider>().enabled = false;

            currentState = EState.CASTING;
        }
        else
        {
            hammer.GetComponent<MeshCollider>().enabled = false;
            animator.SetBool("isAttack", false);
            animator.SetBool("isWalking", true);
            animator.SetBool("isIdle", false);
            currentState = EState.SEARCH;
        }
    }

    protected override void AICasting()
    {
        if (castStandStillTimer <= 0.0f)
        {
            currentState = EState.ATTACK;
        }
    }

    
    IEnumerator startAttack()
    {
        hammer.GetComponent<MeshCollider>().enabled = false;
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttack", true);
        //int randomAttack = Random.Range(0,3);
        //animator.SetInteger("attack!", randomAttack);
        yield return new WaitForSeconds(0.9f);
        hammer.GetComponent<MeshCollider>().enabled = true;
    }

    IEnumerator startAttackAlternate()
    {
        hammer.GetComponent<MeshCollider>().enabled = false;
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttack", true);
        //int randomAttack = Random.Range(0,3);
        //animator.SetInteger("attack!", randomAttack);
        yield return new WaitForSeconds(0.9f);
        hammer.GetComponent<MeshCollider>().enabled = true;
    }


    protected override void Update()
    {
        base.Update();

        if (cNavigation.cAgent.isStopped)
        {
            animator.SetBool("isIdle", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttack", false);
        }
        else if (!cNavigation.cAgent.isStopped )
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isWalking", true);
            //animator.SetBool("isAttack", false);
        }
    }
}
