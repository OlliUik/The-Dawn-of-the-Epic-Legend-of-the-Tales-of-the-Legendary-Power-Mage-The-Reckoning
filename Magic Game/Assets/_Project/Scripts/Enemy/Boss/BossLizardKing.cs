//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class BossLizardKing : EnemyMagicRanged
{
    public enum EBossPattern
    {
        PROJECTILE,
        BEAM,
        AOE,
        GROUND_SMASH,
        DASH
    }

    [Header("Boss -> Behaviour")]
    [SerializeField] private BossAttackPattern[] patterns;

    private BossAttackPattern currentPattern;
    private float patternTimer = 0.0f;

    protected override void Awake()
    {
        base.Awake();

        isRanged = false;
    }

    protected override void Start()
    {
        base.Start();

        ApplyPattern(patterns[0]);

        if (patterns.Length == 0)
        {
            Debug.LogError(this.gameObject + " has no patterns attached!");
        }
        else
        {
            for (int i = 0; i < patterns.Length; i++)
            {
                if (patterns[i] == null)
                {
                    Debug.LogWarning(this.gameObject + " pattern slot number " + i + " is empty!");
                }
            }
        }
    }

    protected override void EnemyStateMachine()
    {
        switch (currentState)
        {
            case EState.DISABLED: break;
            case EState.ATTACK: AIAttack(); break;
            case EState.CASTING: AICasting(); break;
            case EState.ESCAPE: AIAttack(); break;
            default: currentState = EState.ATTACK; break;
        }
    }

    private void ApplyRandomPattern()
    {
        ApplyPattern(patterns[Mathf.FloorToInt(Random.Range(0, patterns.Length))]);
    }

    private void ApplyPattern(BossAttackPattern pattern)
    {
        currentPattern = pattern;

        //Spellcasting
        cSpellBook.spells[0].spell = pattern.spell;
        cSpellBook.spells[0].type = pattern.spellType;
        cSpellBook.spells[0].cards.Clear();
        if (pattern.cards.Length > 0)
        {
            bool canApply = true;

            for (int i = 0; i < pattern.cards.Length; i++)
            {
                if (pattern.cards[i] == null)
                {
                    Debug.LogWarning(pattern + " has empty card slots!");
                    canApply = false;
                }
            }

            if (canApply)
            {
                cSpellBook.spells[0].cards.AddRange(pattern.cards);
                Debug.Log("Cards applied successfully.");
            }
        }

        switch (pattern.attackPattern)
        {
            case EBossPattern.PROJECTILE: attackAnimation = 0; break;
            case EBossPattern.BEAM: attackAnimation = 1; break;
            case EBossPattern.AOE: attackAnimation = 2; break;
            default: attackAnimation = 0; break;
        }

        moveWhileCasting = pattern.moveWhileCasting;
        standStillAfterCasting = pattern.standStillAfterCasting;
        castInBursts = pattern.castInBursts;
        castingTime = pattern.castingTime;
        burstCount = pattern.burstCount;
        timeBetweenCasts = pattern.timeBetweenCasts;
        castingCooldown = pattern.castingCooldown;
        
        //Navigation
        cNavigation.minDistanceFromAttackTarget = pattern.minDistanceFromAttackTarget;
        cNavigation.walkingSpeed = pattern.walkingSpeed;
        cNavigation.walkingAcceleration = pattern.walkingAcceleration;
        cNavigation.runningSpeed = pattern.runningSpeed;
        cNavigation.runningAcceleration = pattern.runningAcceleration;
        cNavigation.panicSpeed = pattern.panicSpeed;
        cNavigation.panicAcceleration = pattern.panicAcceleration;

        Debug.Log("Pattern applied successfully.");
    }

    protected override void AIAttack()
    {
        if (cVision.bCanSeeTarget)
        {
            if ((transform.position - cVision.targetLocation).sqrMagnitude < currentPattern.attackDistance * currentPattern.attackDistance)
            {
                patternTimer = 0.0f;

                if (castingCooldownTimer <= 0.0f)
                {
                    if (castInBursts)
                    {
                        shotsLeft = burstCount;
                    }

                    castingCooldownTimer = castingCooldown;
                    castingTimer = castingTime;
                    castStandStillTimer = standStillAfterCasting;
                    animator.SetTrigger("Cast Spell");
                    animator.SetInteger("Spell Type", attackAnimation);
                    currentState = EState.CASTING;
                }
            }

            patternTimer += logicInterval;
            if (patternTimer > 8.0f)
            {
                Debug.Log(this.gameObject + " took too long to attack, switching pattern...");
                patternTimer = 0.0f;
                ApplyRandomPattern();
            }
        }
    }

    protected override void AICasting()
    {
        base.AICasting();

        if (currentState == EState.ATTACK)
        {
            ApplyRandomPattern();
        }

        //if (currentPattern.attackPattern == EBossPattern.PROJECTILE)
        //{
        //}
    }

    /*
      
     
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

        if (cVision.targetGO != null && cVision.targetGO != targetGO)
        {
            targetGO = cVision.targetGO;
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
        if (cVision.bCanSeeTarget)
        {
            switch (currentPattern)
            {
                case EBossAttackPattern.NONE: break;
                case EBossAttackPattern.ATTACK_MELEE:
                    {
                        currentEnemyType = EEnemyType.MELEE;

                        if (Vector3.Distance(transform.position, cVision.targetLocation) < meleeAttackDistance)
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
                        else if (Vector3.Distance(transform.position, cVision.targetLocation) > rangedEscapeDistance * 2)
                        {
                            currentPattern = EBossAttackPattern.ATTACK_RANGED;
                        }
                        break;
                    }
                case EBossAttackPattern.ATTACK_RANGED:
                    {
                        currentEnemyType = EEnemyType.RANGED;

                        if (Vector3.Distance(transform.position, cVision.targetLocation) < rangedEscapeDistance)
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
                        if (Vector3.Distance(transform.position, cVision.targetLocation) < meleeAttackDistance)
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
                cVision.targetGO.GetComponent<Health>().Hurt(meleeDamage, false);
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
            if (cVision.bCanSeeTarget)
            {
                currentState = EState.ATTACK;
            }
            else
            {
                if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(cVision.targetLocation.x, cVision.targetLocation.z)) < cNavigation.navigationErrorMargin
                    || cVision.targetLocation == Vector3.zero)
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


    */
}
