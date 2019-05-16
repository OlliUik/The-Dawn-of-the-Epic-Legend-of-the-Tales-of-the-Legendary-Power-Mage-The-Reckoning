using UnityEngine;

public class EnemyMelee : EnemyCore
{
    [Header("Melee -> Attacking")]
    [SerializeField] private float meleeAttackDistance = 2.0f;
    [SerializeField] private float meleeDamage = 25.0f;

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

            if (!moveWhileCasting && cNavigation.cAgent.hasPath)
            {
                cNavigation.cAgent.ResetPath();
                cNavigation.cAgent.velocity = new Vector3(0.0f, cNavigation.cAgent.velocity.y, 0.0f);
            }

            cVision.targetGO.GetComponent<Health>().Hurt(meleeDamage, false);

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
            currentState = EState.ATTACK;
        }
    }
}
