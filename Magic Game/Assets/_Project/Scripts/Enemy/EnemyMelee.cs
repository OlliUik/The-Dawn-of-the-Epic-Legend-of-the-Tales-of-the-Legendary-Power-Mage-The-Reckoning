using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : EnemyCore
{
    [Header("Melee -> Attacking")]
    [SerializeField] public float meleeAttackDistance = 10f;
    [SerializeField] public float rangeAttackDistance = 20f;
    [SerializeField] public GameObject hammer;
    private int randomAttack;
    private int oldAttack;
    private bool isAnimationAttacking;

    protected override void AIAttack()
    {   

        Debug.Log("Entering Attack state");
        if (cVision.bCanSeeTarget)
        {   

            if ((transform.position - cVision.targetLocation).sqrMagnitude > meleeAttackDistance * meleeAttackDistance)
            {
                animator.SetBool("isAttack", false);
                return;
            }


            if (!moveWhileCasting && cNavigation.cAgent.hasPath)
            {
                cNavigation.cAgent.ResetPath();
                cNavigation.cAgent.velocity = new Vector3(0.0f, cNavigation.cAgent.velocity.y, 0.0f);
            }
            else
            {
                castStandStillTimer = standStillAfterCasting;
            }


            StartCoroutine(startAttack());

            //currentState = EState.CASTING;
            currentState = EState.ATTACK;
        }
        else
        {
            animator.SetBool("isAttack", false);
            animator.SetBool("isWalking", true);
            animator.SetBool("isIdle", false);
            currentState = EState.SEARCH;
        }
    }

    protected override void AICasting()
    {
        Debug.Log("Entering casting state");
        if (castStandStillTimer <= 0.0f)
        {
            StartCoroutine(startAttack());
            foreach (BoxCollider col in hammer.GetComponents<BoxCollider>()) { col.enabled = false; }
            currentState = EState.ATTACK;
        }
    }


    IEnumerator startAttack()
    {

        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttack", true);
        
        if (!isAnimationAttacking)
        {

            isAnimationAttacking = true;
            randomAttack = Random.Range(0, 4);
            while (randomAttack == oldAttack)
            {
                randomAttack = Random.Range(0, 4);
            }
            oldAttack = randomAttack;
            animator.SetInteger("meleeIndex", randomAttack);
            yield return new WaitForSeconds(0.2f);
            foreach (BoxCollider col in hammer.GetComponents<BoxCollider>()) { col.enabled = true; }
            yield return new WaitForSeconds(1.5f);
            foreach (BoxCollider col in hammer.GetComponents<BoxCollider>()) { col.enabled = false; }
            isAnimationAttacking = false;
        }
        

    }



    IEnumerator startAttackAlternative()
    {
        hammer.GetComponent<MeshCollider>().enabled = false;
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttack", true);
        animator.SetInteger("meleeIndex", 3);
        yield return new WaitForSeconds(0.9f);
        hammer.GetComponent<MeshCollider>().enabled = true;
    }



    protected override void Update()
    {
        base.Update();
        randomAttack = Random.Range(0, 3);
        if (cNavigation.cAgent.isStopped)
        {
            animator.SetBool("isIdle", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttack", false);
        }
        else if (!cNavigation.cAgent.isStopped)
        {
            animator.SetBool("isIdle", false);
            animator.SetBool("isWalking", true);
        }
    }
}
