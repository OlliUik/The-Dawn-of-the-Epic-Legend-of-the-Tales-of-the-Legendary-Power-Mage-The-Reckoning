using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(EnemyVision))]
[RequireComponent(typeof(EnemyNavigation))]
public class EnemyCore : MonoBehaviour
{
    #region VARIABLES

    struct StatusEffects
    {
        bool isOnFire;
        bool isConfused;
        bool isFrozen;
        bool isKnocked;
    };

    public enum EState
    {
        //Also available in EDefaultState
        DISABLED,   //No AI.
        IDLE,       //Stand still.
        PATROL,     //Walk and patrol a specific route.

        //Only available in this enum
        ALERTED,    //Look towards suspicious sound sources.
        PARANOID,   //Stand still but look around for a while.
        SEARCH,     //Search for player at the last known location.
        ATTACK,     //MELEE: Move towards player and attack.
                    //RANGED: Stand still and shoot towards player.
        ESCAPE,     //RANGED: Move away from player
        PANIC,      //Run in random directions
        CONFUSED,   //Shoot at other enemies
        RAGDOLLED,  //No AI when ragdolled.
        VICTORY     //Enemy killed the player, laugh at his/her failure!
    };

    private enum EDefaultState
    {
        DISABLED,
        IDLE,
        PATROL
    };

    public enum EEnemyType
    {
        MELEE,
        RANGED,
        MAGIC
    };

    [SerializeField] private EDefaultState defaultState = EDefaultState.IDLE;
    [SerializeField] private EEnemyType enemyType = EEnemyType.MELEE;
    [SerializeField] private bool searchPlayerAfterAttack = true;
    [SerializeField] private float hearingRadius = 10.0f;
    [SerializeField] private float instantSightRadius = 3.0f;
    [SerializeField] private float rangedEscapeRadius = 10.0f;
    [SerializeField] private float radiusCheckInterval = 1.0f;
    [SerializeField] private float paranoidDuration = 5.0f;

    [SerializeField] private GameObject projectile = null;
    [SerializeField] private float shootInterval = 5.0f;

    public Vector3 spawnPosition { get; private set; } = Vector3.zero;
    public Vector3 spawnRotation { get; private set; } = Vector3.zero;
    public EState currentState { get; private set; } = EState.IDLE;
    public EEnemyType currentEnemyType { get; private set; } = EEnemyType.MELEE;
    public EnemyVision vision { get; private set; } = null;
    public EnemyNavigation navigation { get; private set; } = null;
    public Animator animator { get; private set; } = null;
    public Health cHealth { get; private set; } = null;
    public bool targetPlayer { get; private set; } = true;

    private bool bIsAttacking = false;
    private float radiusCheckTimer = 0.0f;
    private float alertedTimer = 0.0f;
    private float paranoidTimer = 0.0f;
    private float shootIntervalTimer = 0.0f;
    private Vector3 playerPosition = Vector3.zero;
    private Vector3 playerOffset = Vector3.zero;
    private EState previousState = EState.DISABLED;

    #endregion

    #region UNITY_DEFAULT_METHODS

    void Awake()
    {
        GlobalVariables.entityList.Add(this.gameObject);
    }

    void Start()
    {
        vision = GetComponent<EnemyVision>();
        navigation = GetComponent<EnemyNavigation>();
        cHealth = GetComponent<Health>();

        if (GetComponent<Animator>() != null)
        {
            animator = GetComponent<Animator>();
            animator.enabled = false;
        }

        playerOffset = Vector3.up * (GlobalVariables.player.GetComponent<CharacterController>().height / 2);
        spawnPosition = transform.position;
        spawnRotation = transform.rotation.eulerAngles;
        currentState = (EState)defaultState;
        currentEnemyType = enemyType;
    }

    void Update()
    {
        AdvanceTimers();
        playerPosition = GlobalVariables.player.transform.position + playerOffset;

        if (currentState == EState.IDLE
            || currentState == EState.PATROL
            || currentState == EState.SEARCH
            || currentState == EState.PARANOID)
        {
            if (Vector3.Distance(transform.position, playerPosition) < hearingRadius)
            {
                RaycastHit hit;
                if (Physics.Raycast(
                    transform.position,
                    Vector3.Normalize(playerPosition - transform.position),
                    out hit,
                    hearingRadius,
                    1
                    ))
                {
                    if (hit.transform.tag == "Player")
                    {
                        if (Vector3.Distance(transform.position, playerPosition) < instantSightRadius)
                        {
                            currentState = EState.ATTACK;
                        }
                        else
                        {
                            currentState = EState.PARANOID;
                            paranoidTimer = paranoidDuration;
                        }
                    }
                }
            }
        }

        switch (currentState)
        {
            case EState.DISABLED: AIDisabled(); break;
            case EState.IDLE: AIIdle(); break;
            case EState.PATROL: AIPatrol(); break;
            case EState.ALERTED: AIAlerted(); break;
            case EState.PARANOID: AIParanoid(); break;
            case EState.SEARCH: AISearch(); break;
            case EState.ATTACK: AIAttack(); break;
            case EState.ESCAPE: AIEscape(); break;
            case EState.PANIC: AIPanic(); break;
            case EState.CONFUSED: AIConfused(); break;
            case EState.RAGDOLLED: AIRagdolled(); break;
            case EState.VICTORY: AIVictory(); break;
            default: currentState = EState.DISABLED; break;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "TriggerKill")
        {
            if (other.GetComponent<TriggerHurt>().killInstantly)
            {
                cHealth.Kill();
            }
            else
            {
                cHealth.Hurt(other.GetComponent<TriggerHurt>().damage);
            }
        }
    }

    void OnDrawGizmos()
    {
        #if UNITY_EDITOR
        Handles.Label(transform.position + Vector3.up * 2.0f, currentState.ToString());
        Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.1f);
        Gizmos.DrawSphere(transform.position, hearingRadius);
        #endif
    }

    #endregion

    #region CUSTOM_METHODS

    void AdvanceTimers()
    {
        float time = Time.deltaTime;

        shootIntervalTimer      -= shootIntervalTimer > 0.0f    ? time : 0.0f;
        radiusCheckTimer        -= radiusCheckTimer > 0.0f      ? time : 0.0f;
        paranoidTimer           -= paranoidTimer > 0.0f         ? time : 0.0f;
        alertedTimer -= alertedTimer > 0.0f ? time : 0.0f;
    }

    public void OnHurt()
    {
        switch (currentState)
        {
            case EState.IDLE:       currentState = EState.PARANOID; paranoidTimer = paranoidDuration; break;
            case EState.PATROL:     currentState = EState.PARANOID; paranoidTimer = paranoidDuration; break;
            case EState.SEARCH:     currentState = EState.PARANOID; paranoidTimer = paranoidDuration; break;
            case EState.PARANOID:   currentState = EState.ALERTED;  alertedTimer  = 2.0f; break;
        }
    }

    public void OnDeath()
    {
        currentState = EState.DISABLED;
        GlobalVariables.entityList.Remove(this.gameObject);
        Destroy(this.gameObject);
    }

    #endregion

    #region AI_LOGIC

    void AIDisabled()
    {
        if (animator.enabled)
        {
            if (shootIntervalTimer <= 0.0f)
            {
                animator.enabled = false;
                currentState = EState.ATTACK;
            }
        }
    }

    void AIIdle()
    {
        if (vision.bCanSeeTarget)
        {
            currentState = EState.ATTACK;
        }
    }

    void AIPatrol()
    {
        if (vision.bCanSeeTarget)
        {
            currentState = EState.ATTACK;
        }
    }

    void AIAlerted()
    {
        if (vision.bCanSeeTarget)
        {
            currentState = EState.ATTACK;
        }
        else
        {
            if (alertedTimer <= 0.0f)
            {
                currentState = EState.PARANOID;
                paranoidTimer = paranoidDuration;
            }
        }
    }

    void AIParanoid()
    {
        if (vision.bCanSeeTarget)
        {
            currentState = EState.ATTACK;
        }
        else
        {
            if (paranoidTimer <= 0.0f)
            {
                currentState = (EState)defaultState;
            }
        }
    }

    void AISearch()
    {
        if (searchPlayerAfterAttack)
        {
            if (vision.bCanSeeTarget)
            {
                currentState = EState.ATTACK;
            }
            else
            {
                if (Vector3.Distance(transform.position, vision.targetLocation) < navigation.navigationErrorMargin || vision.targetLocation == Vector3.zero)
                {
                    currentState = EState.PARANOID;
                    paranoidTimer = paranoidDuration;
                }
            }
        }
        else
        {
            currentState = EState.PARANOID;
        }
    }

    void AIAttack()
    {
        if (vision.bCanSeeTarget)
        {
            if (currentEnemyType == EEnemyType.RANGED)
            {
                if (Vector3.Distance(transform.position, playerPosition) < rangedEscapeRadius)
                {
                    currentState = EState.ESCAPE;
                    return;
                }

                if (shootIntervalTimer <= 0.0f)
                {
                    shootIntervalTimer = shootInterval;
                    if (projectile != null)
                    {
                        Vector3 direction = -Vector3.Normalize(transform.position + Vector3.up * 1.0f - (vision.targetLocation));
                        Instantiate(projectile).GetComponent<ProjectileTemp>().Initialize(transform.position + Vector3.up * 1.0f, direction, this.gameObject);
                    }
                }
            }
            else if (currentEnemyType == EEnemyType.MELEE)
            {
                if (Vector3.Distance(transform.position, vision.targetLocation) < 2.0f)
                {
                    currentState = EState.DISABLED;
                    animator.enabled = true;
                    shootIntervalTimer = 1.0f;
                }
            }
        }
        else
        {
            currentState = EState.SEARCH;
        }
    }

    void AIEscape()
    {
        if (Vector3.Distance(transform.position, playerPosition) > 20.0f)
        {
            currentState = EState.ATTACK;
        }
    }

    void AIPanic()
    {

    }

    void AIConfused()
    {

    }

    void AIRagdolled()
    {

    }

    void AIVictory()
    {

    }

    #endregion
}
