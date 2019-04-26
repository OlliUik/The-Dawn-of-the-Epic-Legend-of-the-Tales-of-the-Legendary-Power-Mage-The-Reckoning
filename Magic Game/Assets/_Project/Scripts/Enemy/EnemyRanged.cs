using UnityEngine;

public class EnemyRanged : EnemyCore
{
    [Header("Ranged -> Attacking")]
    //Ranged / Magic Ranged
    [SerializeField] protected float rangedEscapeDistance = 10.0f;
    
    //Ranged / Magic Ranged / Magic Melee
    [SerializeField] protected bool castInBursts = false;
    [SerializeField] protected float castingTime = 2.0f;
    [SerializeField] protected int burstCount = 3;
    [SerializeField] protected float timeBetweenCasts = 0.2f;

    //Temporary values
    protected bool bCastedProjectile = false;
    protected float shootIntervalTimer = 0.0f;
    protected float castingTimer = 0.0f;
    protected int shotsLeft = 0;

    protected override void Update()
    {
        base.Update();

        //shootIntervalTimer -= shootIntervalTimer > 0.0f ? time : 0.0f;
        //castingTimer -= castingTimer > 0.0f ? time : 0.0f;

        //if (castingTimer <= 0.0f)
        //{
        //}
    }






    protected override void AIEscape()
    {
        if (cVision.bCanSeeTarget)
        {
            if (Vector3.Distance(transform.position, cVision.targetLocation) > rangedEscapeDistance * 2)
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
