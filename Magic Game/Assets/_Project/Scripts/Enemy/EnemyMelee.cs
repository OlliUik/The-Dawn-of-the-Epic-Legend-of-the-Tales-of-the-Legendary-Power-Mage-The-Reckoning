using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : EnemyCore
{
    [Header("Melee -> Attacking")]
    [SerializeField] public float meleeAttackDistance = 10f;
    [SerializeField] public float meleeDamage = 25.0f;
    [SerializeField] public GameObject hammer;
    private int randomAttack;



    protected override void AIAttack()
    {
        randomAttack = Random.Range(0, 4);
        if (cVision.bCanSeeTarget)
        {
            if ((transform.position - cVision.targetLocation).sqrMagnitude > meleeAttackDistance * meleeAttackDistance)
            {
                return;
            }
          

            castStandStillTimer = standStillAfterCasting;


            if (!moveWhileCasting && cNavigation.cAgent.hasPath)
            {
                cNavigation.cAgent.ResetPath();
                cNavigation.cAgent.velocity = new Vector3(0.0f, cNavigation.cAgent.velocity.y, 0.0f);
            }

            StartCoroutine(startAttack());
            foreach (BoxCollider col in hammer.GetComponents<BoxCollider>())
            {
                col.enabled = false;
            }

            currentState = EState.CASTING;
        }
        else
        {
            foreach(BoxCollider col in hammer.GetComponents<BoxCollider>())
            {
                col.enabled = false;
            }
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
        hammer.GetComponent<BoxCollider>().enabled = false;
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttack", true);
        animator.SetInteger("meleeIndex", randomAttack);
        yield return new WaitForSeconds(0.8f);
        foreach (BoxCollider col in hammer.GetComponents<BoxCollider>())
        {
            col.enabled = true;
        }
    }

    /*
    IEnumerator startAttackAlternative()
    {
        hammer.GetComponent<MeshCollider>().enabled = false;
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttack", true);
        int randomAttack = Random.Range(0,1);
        animator.SetInteger("meleeIndex", randomAttack);
        yield return new WaitForSeconds(0.9f);
        hammer.GetComponent<MeshCollider>().enabled = true;
    }
    */

    protected override void Update()
    {
        base.Update();
        randomAttack = Random.Range(0, 4);
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
        }
    }
}
