using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyNavigation : MonoBehaviour
{
    #region VARIABLES

    [Header("Movement Speed")]
    [SerializeField] private float walkingSpeed = 5.0f;
    [SerializeField] private float walkingAcceleration = 8.0f;
    [SerializeField] private float runningSpeed = 10.0f;
    [SerializeField] private float runningAcceleration = 8.0f;
    [SerializeField] private float panicSpeed = 12.0f;
    [SerializeField] private float panicAcceleration = 8.0f;
    [Header("Navigation")]
    //[SerializeField] private bool moveWhileCasting = false;
    //[SerializeField] private float navigationInterval = 1.0f;
    //[SerializeField] private float navigationIntervalPlayerLocated = 0.2f;
    [SerializeField] private float minDistanceFromAttackTarget = 2.0f;
    [SerializeField] private float paranoidMoveInterval = 1.0f;
    [SerializeField] private float waitAtPatrolPoint = 0.0f;
    [SerializeField] private Vector3[] patrolPoints = null;

    public float navigationErrorMargin { get; private set; } = 0.5f;
    public NavMeshAgent cAgent { get; private set; } = null;

    private int navCurrentPoint = 0;
    private float navTimer = 0.0f;
    private float waitTimer = 0.0f;
    private float navErrorTimer = 0.0f;
    private float paranoidTimer = 0.0f;
    private EnemyCore cEnemyCore = null;

    #endregion

    #region UNITY_DEFAULT_METHODS

    void Start()
    {
        cEnemyCore = GetComponent<EnemyCore>();
        cAgent = GetComponent<NavMeshAgent>();
        navTimer = Random.Range(0.0f, 2.0f);
    }

    public void NavigationLoop()
    {
        switch (cEnemyCore.currentState)
        {
            case EnemyCore.EState.IDLE: AIIdle(); break;
            case EnemyCore.EState.PATROL: AIPatrol(); break;
            case EnemyCore.EState.ALERTED: AIAlerted(); break;
            case EnemyCore.EState.PARANOID: AIParanoid(); break;
            case EnemyCore.EState.SEARCH: AISearch(); break;
            case EnemyCore.EState.ATTACK: AIAttack(); break;
            case EnemyCore.EState.CASTING: AICasting(); break;
            case EnemyCore.EState.ESCAPE: AIEscape(); break;
            case EnemyCore.EState.PANIC: AIPanic(); break;
            case EnemyCore.EState.RAGDOLLED: break;
            default: if (cAgent.hasPath) cAgent.ResetPath(); break;
        }

        if (cEnemyCore.currentState == EnemyCore.EState.IDLE
            || cEnemyCore.currentState == EnemyCore.EState.PATROL
            || cEnemyCore.currentState == EnemyCore.EState.PARANOID
            || cEnemyCore.currentState == EnemyCore.EState.CASTING)
        {
            cAgent.speed = walkingSpeed;
            cAgent.acceleration = walkingAcceleration;
        }
        else if (cEnemyCore.currentState == EnemyCore.EState.PANIC)
        {
            cAgent.speed = panicSpeed;
            cAgent.acceleration = panicAcceleration;
        }
        else
        {
            cAgent.speed = runningSpeed;
            cAgent.acceleration = runningAcceleration;
        }

        if (cEnemyCore.currentState == EnemyCore.EState.ATTACK || cEnemyCore.currentState == EnemyCore.EState.CASTING)
        {
            cAgent.stoppingDistance = 1.0f;
        }
        else
        {
            cAgent.stoppingDistance = 0.0f;
        }
    }

    //void Update()
    //{
    //    if (navTimer <= 0.0f)
    //    {
    //        navTimer = cEnemyCore.cVision.bCanSeeTarget ? navigationIntervalPlayerLocated : navigationInterval;
    //        switch (cEnemyCore.currentState)
    //        {
    //            case EnemyCore.EState.IDLE: AIIdle(); break;
    //            case EnemyCore.EState.PATROL: AIPatrol(); break;
    //            case EnemyCore.EState.ALERTED: AIAlerted(); break;
    //            case EnemyCore.EState.PARANOID: AIParanoid(); break;
    //            case EnemyCore.EState.SEARCH: AISearch(); break;
    //            case EnemyCore.EState.ATTACK: AIAttack(); break;
    //            case EnemyCore.EState.CASTING: AICasting(); break;
    //            case EnemyCore.EState.ESCAPE: AIEscape(); break;
    //            case EnemyCore.EState.PANIC: AIPanic(); break;
    //            case EnemyCore.EState.RAGDOLLED: break;
    //            default: if (cAgent.hasPath) cAgent.ResetPath(); break;
    //        }
    //    }
    //    else
    //    {
    //        navTimer -= Time.deltaTime;
    //    }

    //    if (waitTimer > 0.0f)
    //    {
    //        waitTimer -= Time.deltaTime;
    //    }
    //}

    void OnDrawGizmosSelected()
    {
        int patrolLength = patrolPoints.Length;

        if (patrolLength != 0)
        {
            for (int i = 0; i < patrolLength; i++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(patrolPoints[i], Vector3.one * 0.3f);
                if (patrolLength > 1)
                {
                    if (i == patrolLength - 1)
                    {
                        Gizmos.DrawLine(patrolPoints[i], patrolPoints[0]);
                    }
                    else
                    {
                        Gizmos.DrawLine(patrolPoints[i], patrolPoints[i + 1]);
                    }
                }
            }
        }
    }

    #endregion

    #region AI_LOGIC

    void AIIdle()
    {
        if (Vector3.Distance(transform.position, cEnemyCore.spawnPosition) > navigationErrorMargin)
        {
            cAgent.SetDestination(cEnemyCore.spawnPosition);
        }
    }

    void AIPatrol()
    {
        if (patrolPoints.Length > 1)
        {
            Vector2 entityPos = new Vector2(transform.position.x, transform.position.z);
            Vector2 patrolPos = new Vector2(patrolPoints[navCurrentPoint].x, patrolPoints[navCurrentPoint].z);

            if (Vector2.Distance(entityPos, patrolPos) < navigationErrorMargin)
            {
                waitTimer = waitAtPatrolPoint;
                navCurrentPoint++;
                if (navCurrentPoint >= patrolPoints.Length)
                {
                    navCurrentPoint = 0;
                }
            }

            if (waitTimer <= 0.0f)
            {
                cAgent.SetDestination(patrolPoints[navCurrentPoint]);
            }
        }
        else
        {
            Debug.LogWarning(this.gameObject + " is trying to patrol but has less than 2 patrol points!");
        }
    }

    void AIAlerted()
    {
        cAgent.SetDestination(cEnemyCore.cVision.targetLocation);
    }

    void AIParanoid()
    {
        if (paranoidTimer <= 0.0f)
        {
            paranoidTimer = paranoidMoveInterval;

            Vector3 randomPosition = Vector3.zero;
            randomPosition.x = Random.Range(-1.0f, 1.0f);
            randomPosition.y = 0.0f;
            randomPosition.z = Random.Range(-1.0f, 1.0f);

            cAgent.SetDestination(transform.position + randomPosition);
        }
        else
        {
            paranoidTimer -= cEnemyCore.LogicInterval;
        }
    }

    void AISearch()
    {
        //if (cAgent.remainingDistance < navigationErrorMargin)
        //{
        //    cAgent.SetDestination(cEnemyCore.cVision.targetLocation);
        //}
        //else
        //{
        //    if (navErrorTimer < 3.0f && cAgent.velocity.sqrMagnitude < 1.0f)
        //    {
        //        navErrorTimer += cEnemyCore.LogicInterval;
        //    }
        //    else if (navErrorTimer >= 3.0f)
        //    {
        //        Debug.LogWarning(this.gameObject + " seems to have no valid path towards given location...");
        //        navErrorTimer = 0.0f;
        //        //cEnemyCore.currentState = EnemyCore.EState.PARANOID;
        //    }
        //}

        cAgent.SetDestination(cEnemyCore.cVision.targetLocation);

        if (navErrorTimer < 3.0f && cAgent.velocity.sqrMagnitude < 1.0f)
        {
            navErrorTimer += cEnemyCore.LogicInterval;
        }
        else if (navErrorTimer >= 3.0f)
        {
            Debug.LogWarning(this.gameObject + " seems to have no valid path towards given location...");
            navErrorTimer = 0.0f;
            cEnemyCore.cVision.targetLocation = Vector3.zero;
        }


        //NavMeshHit navHit;
        //if (NavMesh.Raycast(cEnemyCore.cVision.targetLocation, cEnemyCore.cVision.targetLocation + Vector3.down * 5.0f, out navHit, NavMesh.AllAreas))
        //{
        //}
        //else
        //{
        //    if (navErrorTimer < 3.0f && agent.velocity.sqrMagnitude < 1.0f)
        //    {
        //        navErrorTimer += navigationInterval;
        //    }
        //    else if (navErrorTimer >= 3.0f)
        //    {
        //        Debug.LogWarning(this.gameObject + " seems to have no valid path towards given location...");
        //        navErrorTimer = 0.0f;
        //        //cEnemyCore.currentState = EnemyCore.EState.PARANOID;
        //    }
        //}
    }

    void AIAttack()
    {
        if (cEnemyCore.isRanged)
        {
            float escapeDistance = (cEnemyCore as EnemyRanged).rangedEscapeDistance;
            if ((transform.position - cEnemyCore.cVision.targetLocation).sqrMagnitude > escapeDistance * escapeDistance)
            {
                return;
            }
        }

        Vector3 nearTargetLocation = cEnemyCore.cVision.targetLocation + Vector3.Normalize(transform.position - cEnemyCore.cVision.targetLocation) * minDistanceFromAttackTarget;
        cAgent.SetDestination(nearTargetLocation);

        //if (!cEnemyCore.MoveWhileCasting)
        //{
        //    if (agent.hasPath)
        //    {
        //        agent.ResetPath();
        //    }
        //}
        //else
        //{
        //    agent.SetDestination(GetComponent<EnemyVision>().targetLocation);
        //}
    }

    void AICasting()
    {
        if (cEnemyCore.MoveWhileCasting)
        {
            Vector3 nearTargetLocation = cEnemyCore.cVision.targetLocation + Vector3.Normalize(transform.position - cEnemyCore.cVision.targetLocation) * minDistanceFromAttackTarget;
            cAgent.SetDestination(nearTargetLocation);
        }
        else
        {
            if (cAgent.hasPath)
            {
                cAgent.ResetPath();
            }
            cAgent.velocity = new Vector3(0.0f, cAgent.velocity.y, 0.0f);
        }
    }

    void AIEscape()
    {
        if (Vector3.Distance(transform.position, cEnemyCore.cVision.targetLocation) < 20.0f)
        {
            cAgent.SetDestination(transform.position + Vector3.Normalize(transform.position - cEnemyCore.cVision.targetLocation) * 5.0f);
        }
    }

    void AIPanic()
    {
        if (cAgent.remainingDistance < 2.0f)
        {
            Vector3 randomPosition = Vector3.zero;
            randomPosition.x = Random.Range(-5.0f, 5.0f);
            randomPosition.y = 0.0f;
            randomPosition.z = Random.Range(-5.0f, 5.0f);

            cAgent.SetDestination(transform.position + randomPosition);
        }
    }

    #endregion
}
