using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyNavigation : MonoBehaviour
{
    #region VARIABLES

    [Header("Movement Speed")]
    public float walkingSpeed = 5.0f;
    public float walkingAcceleration = 8.0f;
    public float runningSpeed = 10.0f;
    public float runningAcceleration = 8.0f;
    public float panicSpeed = 12.0f;
    public float panicAcceleration = 8.0f;
    [Header("Navigation")]
    //[SerializeField] private bool moveWhileCasting = false;
    //[SerializeField] private float navigationInterval = 1.0f;
    //[SerializeField] private float navigationIntervalPlayerLocated = 0.2f;
    public float minDistanceFromAttackTarget = 2.0f;
    [SerializeField] private float paranoidMoveInterval = 1.0f;
    //[SerializeField] private float waitAtPatrolPoint = 0.0f;
    //[SerializeField] private Vector3[] patrolPoints = null;

    //Dictates whether the agentwaits on each node.
    //[SerializeField]  bool patrolWait;

    //Total time that the patrol wait on each node.
    //[SerializeField] float totalWaitTime;
    //[SerializeField] float waitTimer;

    [Header("Patroling")]

    //Probality of switching node.
    [SerializeField]  float switchProbalitiy = 0.2f;

    //Probality of waiting on a node.
    [SerializeField] float waitProbalitiy = 0.2f;

    [SerializeField] GameObject patrolPointGroup;

    [SerializeField]  List<Waypoint> patrolPoint;



    public float navigationErrorMargin { get; private set; } = 0.5f;
    public NavMeshAgent cAgent { get; private set; } = null;


    private float navTimer = 0.0f;
    private float navErrorTimer = 0.0f;
    private float paranoidTimer = 0.0f;
    private EnemyCore cEnemyCore = null;

    [SerializeField]
    float min, max;

    int navCurrentPoint;
    bool isTravel;
    bool isWaiting;
    bool patrolForward;  
    bool onPath;

    #endregion

    #region UNITY_DEFAULT_METHODS


    void Start()
    {
        cEnemyCore = GetComponent<EnemyCore>();
        cAgent = GetComponent<NavMeshAgent>();

        //get list of patrol points in a section.
        foreach (Transform child in patrolPointGroup.transform)
        {
                patrolPoint.Add(child.GetComponent<Waypoint>());
          
        }

  




        //Agent and patrol point checking
        if (cAgent == null)
        {
            Debug.Log("no mesh agent");
        }
        else
        {
       

            if (patrolPoint != null && patrolPoint.Count >= 2)
            {
                navCurrentPoint = 0;
                SetDestination();
            }
            else
            {
                Debug.Log("not enough patrolpoint");
            }
        }
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

        //When walking away from player, give more acceleration
        float accel = Vector3.Angle(cAgent.velocity.normalized, (cEnemyCore.cVision.targetLocation - transform.position).normalized) * 0.05f;
        cAgent.acceleration += accel;
    }
       
    
    void Update()
    {
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
        /*
        if(cAgent.remainingDistance > cAgent.stoppingDistance)
            {
                cAgent.SetDestination(cAgent.desiredVelocity);
            }
            else
            {
                cAgent.SetDestination(Vector3.zero);

            }
          */

       

    }
   
    /*
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
    */

    #endregion

    #region AI_LOGIC
    
    //Idle is now switching between patrol and idle randomly.
    void AIIdle()
    {
        //Debug.Log("Now Entering Idle/Patrol state");

        /*
        if (Vector3.Distance(transform.position, cEnemyCore.spawnPosition) > navigationErrorMargin)
        {
            cAgent.SetDestination(cEnemyCore.spawnPosition);
        }
        */
        //AIPatrol();

        walkingSpeed = 3f;
        //check if we're close to the destination.
        if (isTravel && cAgent.remainingDistance <= 1.0f)
        {
            isTravel = false;
            //wait?
            if (isWaiting)
            {
                //isWaiting = false;
                StartCoroutine(idleTime());
            }
            else
            {
                ChangePatrolPoint();
                SetDestination();
                isWaiting = (Random.value > 0.5f);
            }
        }

        //normal wait checking
        if (isWaiting)
        {
            //Debug.Log("waiting");
            ChangePatrolPoint();
            SetDestination();
            StartCoroutine(idleTime());
            isWaiting = (Random.value > 0.5f);
        }

    }
    
    void AIPatrol()
    {
        /*
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
        */

        /*
        walkingSpeed = 3f;
        //Debug.Log("Now Entering Patrol/Idle State");       
        //check if we're close to the destination.
        if (isTravel && cAgent.remainingDistance <= 1.0f)
        {
            isTravel = false;
            //wait?
            if (isWaiting)
            {
                //isWaiting = false;
                StartCoroutine(idleTime());
            }
            else
            {
                ChangePatrolPoint();
                SetDestination();
                isWaiting = (Random.value > 0.5f);
            }
        }

        //normal wait checking
        if (isWaiting)
        {
            //Debug.Log("waiting");
            ChangePatrolPoint();
            SetDestination();
            StartCoroutine(idleTime());
            isWaiting = (Random.value > 0.5f);
        }
        */
    }

    //set the destination of the enemy wizard
    private void SetDestination()
    {
        if (patrolPoint != null)
        {

                Vector3 targetVector = patrolPoint[navCurrentPoint].transform.position;
                cAgent.SetDestination(targetVector);
                isTravel = true;
        }
    }

    //Change the destination of the enemy wizard
    private void ChangePatrolPoint()
    {
        if(UnityEngine.Random.Range(0f, 1f) <= switchProbalitiy)
        {
            patrolForward = !patrolForward;
        }
        if(patrolForward)
        {
            navCurrentPoint = (navCurrentPoint + 1) % patrolPoint.Count;
        }
        else
        {
            if(--navCurrentPoint < 0)
            {
                navCurrentPoint = patrolPoint.Count - 1;
            }
        }
    }

    //stop and chilling in da castle.
    IEnumerator idleTime()
    {
        //Debug.Log("waiting");
        float randomNum = Random.Range(min,max);
        cAgent.isStopped = true;
        yield return new WaitForSeconds(randomNum);
        cAgent.isStopped = false;

    }



    void AIAlerted()
    {
        cAgent.isStopped = false;
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
