using UnityEngine;

[RequireComponent(typeof(Spellbook))]
public class EnemyMagicRanged : EnemyRanged
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
            if (!bCastedProjectile)
            {
                cSpellBook.CastSpell(0);
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
}
