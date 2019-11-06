using UnityEngine;

[RequireComponent(typeof(Spellbook))]
public class EnemyMagicMeleeCA : EnemyMagicRangedCA
{
    [Header("Magic Melee -> Attacking")]
    [SerializeField] private float meleeAttackDistance = 2.0f;

    protected override void Awake()
    {
        base.Awake();

        isRanged = false;
    }

    protected override void AIAttack()
    {
        if (cVision.bCanSeeTarget)
        {
            if (castingCooldownTimer <= 0.0f)
            {
                if ((transform.position - cVision.targetLocation).sqrMagnitude > meleeAttackDistance * meleeAttackDistance)
                {
                    return;
                }

                if (!moveWhileCasting && cNavigation.cAgent.hasPath)
                {
                    cNavigation.cAgent.ResetPath();
                    cNavigation.cAgent.velocity = new Vector3(0.0f, cNavigation.cAgent.velocity.y, 0.0f);
                }

                if (castInBursts)
                {
                    shotsLeft = burstCount;
                    castStandStillTimer = castingTime + timeBetweenCasts * burstCount + standStillAfterCasting;
                }
                else
                {
                    shotsLeft = 1;
                    castStandStillTimer = castingTime + standStillAfterCasting;
                }

                castingCooldownTimer = castingCooldown;
                castingTimer = castingTime;
                
                animator.SetTrigger("Cast Spell");
                animator.SetInteger("Spell Type", attackAnimation);
                animator.SetInteger("Casts Left", shotsLeft);
                currentState = EState.CASTING;
            }
        }
        else
        {
            currentState = EState.SEARCH;
        }
    }
}
