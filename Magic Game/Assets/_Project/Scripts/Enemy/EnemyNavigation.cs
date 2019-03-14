﻿using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyCore))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyNavigation : MonoBehaviour
{
    #region VARIABLES

    [SerializeField] private float navigationInterval = 1.0f;
    [SerializeField] private float waitAtPatrolPoint = 0.0f;
    [SerializeField] private Vector3[] patrolPoints;

    public float navigationErrorMargin { get; private set; } = 0.5f;

    private int navCurrentPoint = 0;
    private float navTimer = 0.0f;
    private float waitTimer = 0.0f;
    private EnemyCore cEnemyCore = null;
    private NavMeshAgent agent = null;

    #endregion

    #region UNITY_DEFAULT_METHODS

    void Start()
    {
        cEnemyCore = GetComponent<EnemyCore>();
        agent = GetComponent<NavMeshAgent>();
        navTimer = Random.Range(0.0f, 2.0f);
    }

    void Update()
    {
        if (navTimer <= 0.0f)
        {
            navTimer = navigationInterval;
            switch (cEnemyCore.currentState)
            {
                case EnemyCore.EState.IDLE: AIIdle(); break;
                case EnemyCore.EState.PATROL: AIPatrol(); break;
                case EnemyCore.EState.ALERTED: AIAlerted(); break;
                case EnemyCore.EState.PARANOID: AIParanoid(); break;
                case EnemyCore.EState.SEARCH: AISearch(); break;
                case EnemyCore.EState.ATTACK: AIAttack(); break;
                case EnemyCore.EState.ESCAPE: AIEscape(); break;
                case EnemyCore.EState.PANIC: AIPanic(); break;
                case EnemyCore.EState.CONFUSED: AIConfused(); break;
                case EnemyCore.EState.RAGDOLLED: break;
                case EnemyCore.EState.VICTORY: break;
                default: if (agent.hasPath) agent.ResetPath(); break;
            }
        }
        else
        {
            navTimer -= Time.deltaTime;
        }

        if (waitTimer > 0.0f)
        {
            waitTimer -= Time.deltaTime;
        }
    }

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
            agent.SetDestination(cEnemyCore.spawnPosition);
        }
    }

    void AIPatrol()
    {
        Vector2 playerPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 patrolPos = new Vector2(patrolPoints[navCurrentPoint].x, patrolPoints[navCurrentPoint].z);

        if (Vector2.Distance(playerPos, patrolPos) < navigationErrorMargin)
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
            agent.SetDestination(patrolPoints[navCurrentPoint]);
        }
    }

    void AIAlerted()
    {
        //agent.SetDestination(GlobalVariables.player.transform.position);
    }

    void AIParanoid()
    {
        Vector3 randomPosition = Vector3.zero;
        randomPosition.x = Random.Range(-1.0f, 1.0f);
        randomPosition.y = 0.0f;
        randomPosition.z = Random.Range(-1.0f, 1.0f);

        agent.SetDestination(transform.position + randomPosition);
    }

    void AISearch()
    {
        agent.SetDestination(GetComponent<EnemyVision>().targetLocation);
    }

    void AIAttack()
    {
        if (cEnemyCore.currentEnemyType == EnemyCore.EEnemyType.MELEE)
        {
            agent.SetDestination(GetComponent<EnemyVision>().targetLocation);
        }
        else if (cEnemyCore.currentEnemyType == EnemyCore.EEnemyType.RANGED)
        {
            agent.ResetPath();
        }
    }

    void AIEscape()
    {
        if (Vector3.Distance(transform.position, GlobalVariables.player.transform.position) < 20.0f)
        {
            agent.SetDestination(transform.position + Vector3.Normalize(transform.position - GlobalVariables.player.transform.position) * 5.0f);
        }
    }

    void AIPanic()
    {

    }

    void AIConfused()
    {

    }

    #endregion
}
