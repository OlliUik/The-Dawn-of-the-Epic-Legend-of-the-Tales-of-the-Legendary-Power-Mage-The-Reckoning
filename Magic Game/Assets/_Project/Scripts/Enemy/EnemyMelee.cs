using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : EnemyCore
{
    [Header("Melee -> Attacking")]
    [SerializeField] public float meleeAttackDistance = 10f;
    [SerializeField] public float meleeDamage = 25.0f;

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

            // cVision.targetGO.GetComponent<Health>().Hurt(meleeDamage, false);
            StartCoroutine(startAttack());

            currentState = EState.CASTING;
        }
        else
        {
            animator.SetBool("isAttack", false);
            animator.SetBool("isWalking", true);
            animator.SetTrigger("Melee|MoveFwd");
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
        animator.SetBool("isAttack", true);
        animator.SetTrigger("Melee|AttackSmash");
        yield return new WaitForSeconds(0.7f);
       // cVision.targetGO.GetComponent<Health>().Hurt(meleeDamage, false);
    }

 
}
