using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BossLizardKing : EnemyMagicRanged
{
    public enum EBossPattern
    {
        PROJECTILE,
        PROJECTILE_BEAM_ANIM,
        BEAM,
        AOE,
        GROUND_SMASH,
        DASH
    }

    [System.Serializable]
    private class PatternPreference
    {
        public Color color = Color.yellow;
        public Vector2 range = Vector2.up;
        public BossAttackPattern[] patterns = null;
    }

    [Header("Boss -> Behaviour")]
    [SerializeField] private BossAttackPattern defaultPattern = null;
    [SerializeField] private PatternPreference[] patternPreferences = null;
    //[SerializeField] private BossAttackPattern[] patterns;

    private BossAttackPattern currentPattern = null;
    private float patternTimer = 0.0f;
    private Vector2 currentPatternRange = Vector2.one;

    protected override void Awake()
    {
        base.Awake();

        isRanged = false;
    }

    protected override void Start()
    {
        base.Start();

        ApplyRandomPattern();
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        #if UNITY_EDITOR
        for (int i = 0; i < patternPreferences.Length; i++)
        {
            //Gizmos.color = patternPreferences[i].color;
            //Gizmos.DrawWireSphere(transform.position, patternPreferences[i].range.x);
            //Gizmos.DrawWireSphere(transform.position, patternPreferences[i].range.y);
            Handles.color = patternPreferences[i].color;
            Handles.DrawWireDisc(transform.position + Vector3.up * i * 0.1f, Vector3.up, patternPreferences[i].range.x);
            Handles.DrawWireDisc(transform.position + Vector3.up * i * 0.1f, Vector3.up, patternPreferences[i].range.y);
        }
        #endif
    }

    public override void OnDeath()
    {
        Debug.Log("boss is dead");
        currentState = EState.DISABLED;
        EnemyCore[] tempEnemies = GameObject.FindObjectsOfType<EnemyCore>();
        GlobalVariables.angryBaddiesPoint += 1;
        foreach (EnemyCore child in tempEnemies)
        {
            child.GetComponent<Health>().ourStepDadKilled = true;
            child.GetComponent<Health>().UpdateMaxHealth();
        }
        GlobalVariables.teamBadBoys.Remove(this.gameObject);    
        Destroy(this.gameObject);
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

        if (cVision.targetGO != null)
        {
            if (animator.GetComponent<RotateTowardsTarget>().targetTransform != cVision.targetGO.transform)
            {
                animator.GetComponent<RotateTowardsTarget>().targetTransform = cVision.targetGO.transform;
            }
        }
    }

    private void ApplyRandomPattern()
    {
        //Initialize counter
        int counter = 0;

        //Find the amount of all possible patterns, and initialize array with that amount
        for (int i = 0; i < patternPreferences.Length; i++)
        {
            counter += patternPreferences[i].patterns.Length;
        }
        BossAttackPattern[] possiblePatterns = new BossAttackPattern[counter];
        Vector2[] patternRanges = new Vector2[counter];

        //Reset counter, so it can be re-used
        counter = 0;
        
        //If player is within a certain range, add all the patterns in that range to possible patterns
        for (int outerLoop = 0; outerLoop < patternPreferences.Length; outerLoop++)
        {
            if ((transform.position - cVision.targetLocation).sqrMagnitude > patternPreferences[outerLoop].range.x * patternPreferences[outerLoop].range.x
                && (transform.position - cVision.targetLocation).sqrMagnitude < patternPreferences[outerLoop].range.y * patternPreferences[outerLoop].range.y)
            {
                for (int innerLoop = 0; innerLoop < patternPreferences[outerLoop].patterns.Length; innerLoop++)
                {
                    possiblePatterns[counter] = patternPreferences[outerLoop].patterns[innerLoop];
                    patternRanges[counter] = patternPreferences[outerLoop].range;
                    counter++;
                }
            }
        }

        if (counter == 0)
        {
            if (defaultPattern != null)
            {
                Debug.LogWarning(this.gameObject + " found no fitting pattern, using the default one...");
                ApplyPattern(defaultPattern);
                currentPatternRange = new Vector2(0.0f, Mathf.Infinity);
                return;
            }
            else
            {
                Debug.LogError(this.gameObject + " couldn't find any fitting patterns, and is missing default pattern!");
                currentState = EState.DISABLED;
                return;
            }
        }
        else
        {
            //Apply a random pattern from the list of possible patterns
            int random = Mathf.RoundToInt(Random.Range(0, counter));
            ApplyPattern(possiblePatterns[random]);
            currentPatternRange = patternRanges[random];
        }
    }

    private void ApplyPattern(BossAttackPattern pattern)
    {
        currentPattern = pattern;

        //Spellcasting
        cSpellBook.spells[0].spell = pattern.spell;
        //cSpellBook.spells[0].type = pattern.spellType;
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
            case EBossPattern.PROJECTILE_BEAM_ANIM: attackAnimation = 1; break;
            case EBossPattern.BEAM: attackAnimation = 1; break;
            case EBossPattern.AOE: attackAnimation = 2; break;
            case EBossPattern.DASH: attackAnimation = 4; break;
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
                    //If current pattern is no longer in valid preference range, randomize pattern
                    if ((transform.position - cVision.targetLocation).sqrMagnitude < currentPatternRange.x * currentPatternRange.x
                || (transform.position - cVision.targetLocation).sqrMagnitude > currentPatternRange.y * currentPatternRange.y)
                    {
                        Debug.LogWarning(this.gameObject + " pattern range is no longer valid, randomizing attack pattern...");
                        ApplyRandomPattern();
                    }

                    if (castInBursts)
                    {
                        shotsLeft = burstCount;
                    }
                    else
                    {
                        shotsLeft = 1;
                    }

                    castingCooldownTimer = castingCooldown;
                    castingTimer = castingTime;
                    castStandStillTimer = standStillAfterCasting;
                    animator.SetTrigger("Cast Spell");
                    animator.SetInteger("Spell Type", attackAnimation);
                    animator.SetInteger("Casts Left", shotsLeft);
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
        switch (currentPattern.attackPattern)
        {
            case EBossPattern.PROJECTILE_BEAM_ANIM:
                {
                    /*------------------------------------------------------------------------------------*/
                    
                    if (castingTimer <= 0.0f)
                    {
                        if (shotsLeft > 0)
                        {
                            cSpellBook.CastSpell(0);
                            shotsLeft--;
                            animator.SetInteger("Casts Left", shotsLeft);

                            if (!cVision.bCanSeeTarget)
                            {
                                bCastedProjectile = false;
                                currentState = EState.ATTACK;
                                castingCooldownTimer *= 0.25f;
                                return;
                            }
                            castingTimer = timeBetweenCasts;
                        }
                        else
                        {
                            if (castStandStillTimer <= 0.0f)
                            {
                                bCastedProjectile = false;
                                currentState = EState.ATTACK;
                            }
                        }
                    }
                    else if (!bCastedProjectile)
                    {
                        bCastedProjectile = true;
                        animator.SetTrigger("Release Hold");
                    }

                    /*------------------------------------------------------------------------------------*/

                    break;
                }
            case EBossPattern.DASH:
                {
                    /*------------------------------------------------------------------------------------*/

                    if (!bCastedProjectile)
                    {
                        bCastedProjectile = true;
                        //StartCoroutine("DashCoroutine");

                        Debug.LogWarning(this.gameObject + " Dashing is currently disabled in code!");
                        bCastedProjectile = false;
                        currentState = EState.ATTACK;
                    }

                    /*------------------------------------------------------------------------------------*/

                    break;
                }
            default:
                {
                    base.AICasting();
                    break;
                }
        }

        if (currentState == EState.ATTACK)
        {
            ApplyRandomPattern();
        }
    }

    //IEnumerator DashCoroutine()
    //{
    //    cNavigation.cAgent.ResetPath();
    //    cNavigation.cAgent.velocity = Vector3.zero;
    //    cNavigation.cAgent.SetDestination(cVision.targetLocation);
    //    yield return new WaitForSeconds(castingTime);
    //    bCastedProjectile = false;
    //    currentState = EState.ATTACK;
    //    yield return null;
    //}
}
