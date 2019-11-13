using UnityEngine;

[RequireComponent(typeof(Spellbook))]
public class EnemyMagicRangedCA : EnemyRangedCA
{
    public Spellbook cSpellBook { get; private set; } = null;

    protected override void Start()
    {
        base.Start();
        cSpellBook = GetComponent<Spellbook>();
    }

    protected override void AICasting()
    {
        if (castingTimer <= 0.0f)
        {
            if (shotsLeft > 0)
            {
                cSpellBook.CastSpell(0);
                animator.SetTrigger("Release Hold");
                shotsLeft--;
                animator.SetInteger("Casts Left", shotsLeft);

                if (!cVision.bCanSeeTarget)
                {
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
                    currentState = EState.ATTACK;
                }
            }

            //if (shotsLeft > 1)
            //{
            //    if (!cVision.bCanSeeTarget)
            //    {
            //        currentState = EState.ATTACK;
            //        castingCooldownTimer *= 0.25f;
            //        return;
            //    }

            //    animator.SetTrigger("Interrupt Spell");
            //    animator.SetTrigger("Cast Spell");
            //    bCastedProjectile = false;
                
            //    shotsLeft--;
            //}
            //else if (castStandStillTimer <= 0.0f)
            //{
            //    bCastedProjectile = false;
            //    currentState = EState.ATTACK;
            //}
        }
    }
}
