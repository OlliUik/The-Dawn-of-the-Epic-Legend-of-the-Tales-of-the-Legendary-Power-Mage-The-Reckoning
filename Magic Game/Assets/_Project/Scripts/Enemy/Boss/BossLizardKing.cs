//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossLizardKing : EnemyCore
{
    public enum EBossAttackPattern
    {
        NONE,
        ATTACK_MELEE,
        ATTACK_RANGED,
        CIRCLE_AROUND_TARGET,
        TAUNT
    };

    [SerializeField] private Image bossHealthBar = null;

    private GameObject targetGO = null;
    private EBossAttackPattern currentPattern = EBossAttackPattern.TAUNT;
    private float tauntTimer = 5.0f;

    protected override void Update()
    {
        AdvanceTimers();

        if (currentState != EState.DISABLED)
        {
            switch (currentState)
            {
                case EState.ATTACK: AIAttack(); break;
                case EState.CASTING: AICasting(); break;
                case EState.SEARCH: AISearch(); break;
                default: currentState = EState.DISABLED; break;
            }

            if (GlobalVariables.bAnyPlayersAlive == false)
            {
                currentState = EState.DISABLED;
            }
        }

        if (vision.targetGO != null && vision.targetGO != targetGO)
        {
            targetGO = vision.targetGO;
        }
        else
        {
            if (targetGO != null)
            {
                targetPosition = targetGO.transform.position;
            }
        }
    }

    public override void OnHurt()
    {
        animator.SetTrigger("Take Damage");
        if (bossHealthBar != null)
        {
            bossHealthBar.rectTransform.localScale = new Vector3(cHealth.health / cHealth.maxHealth, 1.0f, 1.0f);
        }
    }

    protected override void AIAttack()
    {
        if (vision.bCanSeeTarget)
        {
            switch (currentPattern)
            {
                case EBossAttackPattern.NONE: break;
                case EBossAttackPattern.ATTACK_MELEE:
                    {
                        currentEnemyType = EEnemyType.MELEE;

                        if (Vector3.Distance(transform.position, vision.targetLocation) < meleeAttackDistance)
                        {
                            if (castingStandstillTimer > 0.0f)
                            {
                                animator.SetTrigger("Interrupt Spell");
                                return;
                            }
                            castingStandstillTimer = castingStandstill;
                            animator.SetTrigger("Cast Spell");
                            animator.SetInteger("Spell Type", castingSpellType);
                            currentState = EState.CASTING;
                        }
                        else if (Vector3.Distance(transform.position, vision.targetLocation) > rangedEscapeDistance * 2)
                        {
                            currentPattern = EBossAttackPattern.ATTACK_RANGED;
                        }
                        break;
                    }
                case EBossAttackPattern.ATTACK_RANGED:
                    {
                        currentEnemyType = EEnemyType.RANGED;

                        if (Vector3.Distance(transform.position, targetPosition) < rangedEscapeDistance)
                        {
                            currentPattern = EBossAttackPattern.ATTACK_MELEE;
                            return;
                        }

                        if (shootIntervalTimer <= 0.0f)
                        {
                            shootIntervalTimer = castingInterval;
                            castingTimer = castingTime;
                            castingStandstillTimer = castingStandstill;
                            animator.SetTrigger("Cast Spell");
                            animator.SetInteger("Spell Type", castingSpellType);
                            currentState = EState.CASTING;
                        }
                        break;
                    }
                case EBossAttackPattern.CIRCLE_AROUND_TARGET:
                    {
                        break;
                    }
                case EBossAttackPattern.TAUNT:
                    {
                        if (Vector3.Distance(transform.position, vision.targetLocation) < meleeAttackDistance)
                        {
                            currentPattern = EBossAttackPattern.ATTACK_MELEE;
                        }
                        else
                        {
                            if (tauntTimer > 0.0f)
                            {
                                tauntTimer -= Time.deltaTime;
                            }
                            else
                            {
                                currentPattern = EBossAttackPattern.ATTACK_MELEE;
                            }
                        }
                        break;
                    }
            }
        }
        else
        {
            currentState = EState.SEARCH;
        }
    }

    protected override void AICasting()
    {
        if (currentPattern == EBossAttackPattern.ATTACK_MELEE)
        {
            if (!bCastedProjectile)
            {
                vision.targetGO.GetComponent<Health>().Hurt(meleeDamage);
                bCastedProjectile = true;
            }

            if (castingStandstillTimer <= 0.0f)
            {
                bCastedProjectile = false;
                currentState = EState.ATTACK;
            }
        }
        else
        {
            if (castingTimer <= 0.0f)
            {
                if (!bCastedProjectile)
                {
                    //CastProjectile();
                    cSpellBook.CastSpell(0);
                    bCastedProjectile = true;
                }

                if (castingStandstillTimer <= 0.0f)
                {
                    bCastedProjectile = false;
                    currentState = EState.ATTACK;
                }
            }
        }
    }

    protected override void AISearch()
    {
        if (searchPlayerAfterAttack)
        {
            if (vision.bCanSeeTarget)
            {
                currentState = EState.ATTACK;
            }
            else
            {
                if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(vision.targetLocation.x, vision.targetLocation.z)) < navigation.navigationErrorMargin
                    || vision.targetLocation == Vector3.zero)
                {
                    currentState = EState.IDLE;
                }
            }
        }
        else
        {
            currentState = EState.ATTACK;
        }
    }
}
