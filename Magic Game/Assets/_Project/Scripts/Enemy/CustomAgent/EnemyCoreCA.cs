//using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(EnemyVisionCA))]
//[RequireComponent(typeof(EnemyNavigation))]
public class EnemyCoreCA : MonoBehaviour
{
    #region VARIABLES

    public struct StatusEffects
    {
        public bool isOnFire;
        public bool isConfused;
        public bool isFrozen;
        public bool isKnocked;

        public StatusEffects(bool onFire, bool confused, bool frozen, bool knocked)
        {
            isOnFire = onFire;
            isConfused = confused;
            isFrozen = frozen;
            isKnocked = knocked;
        }
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
        CASTING,    //Stand still and execute an attack.
        ESCAPE,     //RANGED: Move away from player
        PANIC,      //Run in random directions
        RAGDOLLED   //No AI when ragdolled.
    };

    protected enum EDefaultState
    {
        DISABLED,
        IDLE,
        PATROL
    };

    public EnemyVision cVision { get; protected set; } = null;
    public EnemyNavigationCA cNavigation { get; protected set; } = null;
    public Health cHealth { get; protected set; } = null;

    public StatusEffects status { get; protected set; } = new StatusEffects(false, false, false, false);
    public EState currentState { get; protected set; } = EState.DISABLED;
    public bool isRanged { get; protected set; } = false;
    public int entitySpawnNumber { get; protected set; } = 0;
    public Vector3 spawnPosition { get; protected set; } = Vector3.zero;
    public Vector3 spawnRotation { get; protected set; } = Vector3.zero;

    [Header("Score")]
    public int score = 0;
    public int roundedScore;
    private bool hasStatusEffect = false;

    [Header("Core -> State Machine")]
    [SerializeField] protected float logicInterval = 0.5f;
    [SerializeField] protected EDefaultState defaultState = EDefaultState.IDLE;
    [SerializeField] protected bool searchPlayerAfterAttack = true;
    [SerializeField] protected bool alwaysAggressive = true;
    [SerializeField] protected float aggressiveRadius = 30.0f;
    [SerializeField] protected bool hearingEnabled = true;
    [SerializeField] protected float hearingRadius = 10.0f;
    [SerializeField] protected float radiusCheckInterval = 1.0f;
    [SerializeField] protected float paranoidDuration = 5.0f;

    [Header("Core -> Attacking")]
    [SerializeField] protected bool moveWhileCasting = false;
    [SerializeField] protected float standStillAfterCasting = 4.0f;
    [SerializeField] protected int attackAnimation = 0; //Check the animator controller to find out the desired number!
    [SerializeField] protected Animator animator = null;

    [Header("Core -> Ragdoll")]
    [SerializeField] protected Rigidbody ragdoll = null;

    [Header("Core -> Other")]
    [SerializeField] protected bool spawnAsGoodGuy = false;
    [SerializeField] protected bool spawnAsConfused = false;

    public bool MoveWhileCasting { get { return moveWhileCasting; } }
    public float LogicInterval { get { return logicInterval; } }

    //Core temporary values
    protected float radiusCheckTimer = 0.0f;
    protected float alertedTimer = 0.0f;
    protected float paranoidTimer = 0.0f;
    protected float castStandStillTimer = 0.0f;
    protected float ragdollSleepTimer = 0.0f;

    #endregion

    #region UNITY_DEFAULT_METHODS

    protected virtual void Awake()
    {
        if (spawnAsGoodGuy)
        {
            GlobalVariables.teamGoodGuys.Add(this.gameObject);
            entitySpawnNumber = GlobalVariables.teamGoodGuys.Count;
            status = new StatusEffects(status.isOnFire, !spawnAsConfused, status.isFrozen, status.isKnocked);
        }
        else
        {
            GlobalVariables.teamBadBoys.Add(this.gameObject);
            entitySpawnNumber = GlobalVariables.teamBadBoys.Count;
            status = new StatusEffects(status.isOnFire, spawnAsConfused, status.isFrozen, status.isKnocked);
        }
    }

    protected virtual void Start()
    {
        cVision = GetComponent<EnemyVision>();
        cNavigation = GetComponent<EnemyNavigationCA>();
        cHealth = GetComponent<Health>();

        spawnPosition = transform.position;
        spawnRotation = transform.rotation.eulerAngles;
        currentState = (EState)defaultState;

        InvokeRepeating("EnemyLogicLoop", Time.fixedDeltaTime * entitySpawnNumber, Time.fixedDeltaTime * Mathf.FloorToInt(logicInterval / Time.fixedDeltaTime));
    }

    protected virtual void Update()
    {
        float time = Time.deltaTime;

        radiusCheckTimer -= radiusCheckTimer > 0.0f ? time : 0.0f;
        paranoidTimer -= paranoidTimer > 0.0f ? time : 0.0f;
        alertedTimer -= alertedTimer > 0.0f ? time : 0.0f;
        castStandStillTimer -= castStandStillTimer > 0.0f ? time : 0.0f;

        if (currentState != EState.DISABLED)
        {
            if (status.isFrozen)
            {
                currentState = EState.DISABLED;
                return;
            }

            if (status.isKnocked)
            {
                currentState = EState.RAGDOLLED;
                return;
            }

            if (status.isOnFire)
            {
                currentState = EState.PANIC;
                return;
            }
        }
    }

    protected void OnTriggerStay(Collider other)
    {
        if (other.tag == "TriggerKill")
        {
            if (other.GetComponent<TriggerHurt>().killInstantly)
            {
                cHealth.Kill();
            }
            else
            {
                cHealth.Hurt(other.GetComponent<TriggerHurt>().damage, false);
            }
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        #if UNITY_EDITOR
        Handles.Label(transform.position + Vector3.up * 2.0f, currentState.ToString());

        if (hearingEnabled)
        {
            Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, hearingRadius);
        }
        
        if (!alwaysAggressive)
        {
            Gizmos.color = new Color(0.0f, 1.0f, 0.0f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, aggressiveRadius);
        }
        #endif
    }

    #endregion

    #region CUSTOM_METHODS
    
    protected void NoiseChecker()
    {
        if (hearingEnabled)
        {
            GameObject[] soundObjects = GameObject.FindGameObjectsWithTag("SFX");

            foreach (GameObject go in soundObjects)
            {
                if (go.activeInHierarchy && (transform.position - go.transform.position).sqrMagnitude < hearingRadius * hearingRadius)
                {
                    AudioSourceIdentifier identifier = go.GetComponent<AudioSourceIdentifier>();

                    if (identifier != null && identifier.isPlayer && identifier.madeNoise)
                    {
                        cVision.targetLocation = go.transform.position;
                        currentState = EState.SEARCH;
                    }
                }
            }
        }
    }

    public Vector3 PredictTargetPosition(Vector3 selfPosition, float spellSpeed, Vector3 targetPosition, Vector3 targetVelocity)
    {
        Vector3 predictedPosition = targetPosition;

        if (!status.isConfused)
        {
            float distance = Vector3.Distance(selfPosition, targetPosition);
            float timeUntilImpact = distance / spellSpeed;
            predictedPosition = targetPosition + targetVelocity * timeUntilImpact;
            predictedPosition.y = targetPosition.y;
        }

        return predictedPosition;
    }

    public void SwitchTeam()
    {

    }

    public void EnableRagdoll(bool b)
    {
        status = new StatusEffects(status.isOnFire, status.isConfused, status.isFrozen, b);
        animator.enabled = !b;
        animator.gameObject.GetComponent<RagdollModifier>().SetKinematic(!b, b);
        animator.transform.parent = b ? null : transform;
        cNavigation.cAgent.enabled = !b;
        currentState = b ? EState.RAGDOLLED : EState.ATTACK;
        ragdollSleepTimer = 2.0f;

        if (!b)
        {
            cNavigation.cAgent.Warp(ragdoll.transform.position);
        }
    }

    public virtual void OnHurt()
    {
        if (castStandStillTimer <= 0.0f)
        {
            //NOTE: move animation handling to another script?
            animator.SetTrigger("Take Damage");
        }

        switch (currentState)
        {
            case EState.IDLE:       currentState = EState.PARANOID; paranoidTimer = paranoidDuration; break;
            case EState.PATROL:     currentState = EState.PARANOID; paranoidTimer = paranoidDuration; break;
            case EState.SEARCH:     currentState = EState.PARANOID; paranoidTimer = paranoidDuration; break;
            case EState.PARANOID:   currentState = EState.ALERTED;  alertedTimer  = 2.0f; break;
        }
    }

    public virtual void OnDeath()
    {
        roundedScore = Mathf.RoundToInt(score * ScoreSystem.scoreSystem.multiplier);

        if (hasStatusEffect)
        {
            ScoreCalculator.scoreCalc.CountScore(score);
        }

        else
        {
            ScoreSystem.scoreSystem.addedScore = roundedScore;
            ScoreSystem.scoreSystem.score += roundedScore;
        }

        ScoreCombo.scoreCombo.isEnemyKilled = true;
        ScoreCombo.scoreCombo.combo++;

        currentState = EState.DISABLED;
        GlobalVariables.teamBadBoys.Remove(this.gameObject);

        //Detach the enemy model and ragdoll it
        animator.enabled = false;
        animator.gameObject.GetComponent<RagdollModifier>().SetKinematic(false, true);
        animator.transform.parent = null;

        Destroy(this.gameObject);
    }

    #endregion

    #region AI_LOGIC

    protected void EnemyLogicLoop()
    {
        if (GlobalVariables.bAnyPlayersAlive == false)
        {
            currentState = EState.DISABLED;
        }

        EnemyStateMachine();
        cNavigation.NavigationLoop();
    }

    protected virtual void EnemyStateMachine()
    {
        switch (currentState)
        {
            case EState.DISABLED: break;
            case EState.IDLE: AIIdle(); break;
            case EState.PATROL: AIPatrol(); break;
            case EState.ALERTED: AIAlerted(); break;
            case EState.PARANOID: AIParanoid(); break;
            case EState.SEARCH: AISearch(); break;
            case EState.ATTACK: AIAttack(); break;
            case EState.CASTING: AICasting(); break;
            case EState.ESCAPE: AIEscape(); break;
            case EState.PANIC: AIPanic(); break;
            case EState.RAGDOLLED: AIRagdolled(); break;
            default: currentState = EState.DISABLED; break;
        }
    }

    protected virtual void AIIdle()
    {
        if (cVision.bCanSeeTarget)
        {
            if (!alwaysAggressive)
            {
                if ((transform.position - cVision.targetLocation).sqrMagnitude < aggressiveRadius * aggressiveRadius)
                {
                    currentState = EState.ATTACK;
                    return;
                }
            }
            else
            {
                currentState = EState.ATTACK;
                return;
            }
        }
        NoiseChecker();
    }

    protected virtual void AIPatrol()
    {
        //Use idle logic by default
        AIIdle();
    }

    protected virtual void AIAlerted()
    {
        if (!cVision.bCanSeeTarget)
        {
            if (alertedTimer <= 0.0f)
            {
                currentState = EState.PARANOID;
                paranoidTimer = paranoidDuration;
                return;
            }

            GameObject target = null;

            foreach (GameObject go in (status.isConfused ? GlobalVariables.teamBadBoys : GlobalVariables.teamGoodGuys))
            {
                if (target == null)
                {
                    target = go;
                }
                else if ((transform.position - go.transform.position).sqrMagnitude < (transform.position - target.transform.position).sqrMagnitude)
                {
                    target = go;
                }
            }

            if (target != null)
            {
                cVision.targetLocation = target.transform.position;
            }
        }
        else
        {
            currentState = EState.ATTACK;
        }
    }

    protected virtual void AIParanoid()
    {
        if (cVision.bCanSeeTarget)
        {
            currentState = EState.ATTACK;
            return;
        }
        else
        {
            if (paranoidTimer <= 0.0f)
            {
                currentState = (EState)defaultState;
            }
        }
        NoiseChecker();
    }

    protected virtual void AISearch()
    {
        if (searchPlayerAfterAttack)
        {
            if (cVision.bCanSeeTarget)
            {
                currentState = EState.ATTACK;
                return;
            }
            else
            {
                if ((transform.position - cVision.targetLocation).sqrMagnitude < 3.0f || cVision.targetLocation == Vector3.zero)
                {
                    paranoidTimer = paranoidDuration;
                    currentState = EState.PARANOID;
                }
                
                //if ((transform.position - cVision.targetLocation).sqrMagnitude < cNavigation.navigationErrorMargin * cNavigation.navigationErrorMargin
                //    || cVision.targetLocation == Vector3.zero)
                //{
                //    paranoidTimer = paranoidDuration;
                //    currentState = EState.PARANOID;
                //}

                //if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(cVision.targetLocation.x, cVision.targetLocation.z)) < cNavigation.navigationErrorMargin 
                //    || cVision.targetLocation == Vector3.zero)
                //{
                //    paranoidTimer = paranoidDuration;
                //    currentState = EState.PARANOID;
                //}
            }
        }
        else
        {
            paranoidTimer = paranoidDuration;
            currentState = EState.PARANOID;
        }
        NoiseChecker();
    }

    protected virtual void AIAttack()
    {
        Debug.LogError("Use enemy variant scripts instead of EnemyCore!");
        currentState = EState.DISABLED;
    }

    protected virtual void AICasting()
    {
        Debug.LogError("Use enemy variant scripts instead of EnemyCore!");
        currentState = EState.DISABLED;
    }

    protected virtual void AIEscape()
    {
        Debug.LogError("Use enemy variant scripts instead of EnemyCore!");
        currentState = EState.DISABLED;
    }

    protected virtual void AIPanic()
    {
        if (!status.isOnFire)
        {
            currentState = EState.ATTACK;
        }
    }

    protected virtual void AIRagdolled()
    {
        if (status.isKnocked)
        {
            if (ragdoll.velocity.sqrMagnitude < 0.1f)
            {
                if (ragdollSleepTimer > 0.0f)
                {
                    ragdollSleepTimer -= logicInterval;
                }
                else
                {
                    EnableRagdoll(false);
                }
            }
            else
            {
                ragdollSleepTimer = 2.0f;
            }
        }
    }

    #endregion
}
