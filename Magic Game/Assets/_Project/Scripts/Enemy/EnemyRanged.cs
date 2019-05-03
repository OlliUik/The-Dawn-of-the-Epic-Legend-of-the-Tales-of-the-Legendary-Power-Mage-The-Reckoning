using UnityEngine;

public class EnemyRanged : EnemyCore
{
    [Header("Ranged -> Attacking")]
    //Ranged / Magic Ranged
    public float rangedEscapeDistance = 10.0f;
    
    //Ranged / Magic Ranged / Magic Melee
    [SerializeField] protected bool castInBursts = false;
    [SerializeField] protected float castingTime = 2.0f;
    [SerializeField] protected int burstCount = 3;
    [SerializeField] protected float timeBetweenCasts = 0.2f;
    [SerializeField] protected float castingCooldown = 4.0f;

    //Temporary values
    protected bool bCastedProjectile = false;
    protected float castingTimer = 0.0f;
    protected float castingCooldownTimer = 0.0f;
    protected int shotsLeft = 0;

    protected override void Awake()
    {
        base.Awake();

        isRanged = true;
    }

    protected override void Update()
    {
        base.Update();

        castingTimer -= castingTimer > 0.0f ? Time.deltaTime : 0.0f;
        if (castingTimer <= 0.0f)
        {
            castingCooldownTimer -= castingCooldownTimer > 0.0f ? Time.deltaTime : 0.0f;
            if (currentState == EState.CASTING)
            {
                AICasting();
            }
        }
    }

    protected override void AIAttack()
    {
        if (cVision.bCanSeeTarget)
        {
            if ((transform.position - cVision.targetLocation).sqrMagnitude < rangedEscapeDistance * rangedEscapeDistance)
            {
                currentState = EState.ESCAPE;
                return;
            }

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
        else
        {
            currentState = EState.SEARCH;
        }
    }

    protected override void AICasting()
    {
        if (castingTimer <= 0.0f)
        {
            if (!bCastedProjectile)
            {
                //CastProjectile();
                //cSpellBook.CastSpell(0);
                Debug.LogError("Missing casting implementation! [AICasting() in EnemyRanged]");
                bCastedProjectile = true;
            }

            if (shotsLeft > 1)
            {
                if (!cVision.bCanSeeTarget)
                {
                    currentState = EState.ATTACK;
                    castingCooldownTimer *= 0.25f;
                    return;
                }

                animator.SetTrigger("Interrupt Spell");
                animator.SetTrigger("Cast Spell");
                bCastedProjectile = false;
                castingTimer = timeBetweenCasts;
                shotsLeft--;
            }
            else if (castStandStillTimer <= 0.0f)
            {
                bCastedProjectile = false;
                currentState = EState.ATTACK;
            }
        }
    }

    protected override void AIEscape()
    {
        if (cVision.bCanSeeTarget)
        {
            if ((transform.position - cVision.targetLocation).sqrMagnitude > rangedEscapeDistance * rangedEscapeDistance * 2)
            {
                currentState = EState.ATTACK;
            }
        }
        else
        {
            currentState = EState.SEARCH;
        }
    }
}
