using UnityEngine;

public class EnemyMelee : EnemyCore
{
    [Header("Melee -> Attacking")]
    [SerializeField] private float meleeAttackDistance = 2.0f;
    [SerializeField] private float meleeDamage = 25.0f;

    private bool bHasAttacked = false;

    protected override void AIAttack()
    {
        if (cVision.bCanSeeTarget)
        {
            if ((transform.position - cVision.targetLocation).sqrMagnitude > meleeAttackDistance * meleeAttackDistance)
            {
                return;
            }

            castStandStillTimer = standStillAfterCasting;
            animator.SetTrigger("Cast Spell");
            animator.SetInteger("Spell Type", attackAnimation);

            if (!moveWhileCasting && cNavigation.agent.hasPath)
            {
                cNavigation.agent.ResetPath();
                cNavigation.agent.velocity = new Vector3(0.0f, cNavigation.agent.velocity.y, 0.0f);
            }

            cVision.targetGO.GetComponent<Health>().Hurt(meleeDamage, false);
            bHasAttacked = true;

            currentState = EState.CASTING;
        }
        else
        {
            currentState = EState.SEARCH;
        }
    }

    protected override void AICasting()
    {
        if (castStandStillTimer <= 0.0f)
        {
            bHasAttacked = false;
            currentState = EState.ATTACK;
        }
    }
}
