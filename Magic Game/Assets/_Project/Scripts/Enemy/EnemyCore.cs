using System.Collections.Generic;
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

    [Header("Melee Only Variables")]
    [SerializeField] protected float meleeAttackDistance = 2.0f;
    [SerializeField] protected float meleeDamage = 25.0f;

    [Header("Ranged Only Variables")]
    [SerializeField] protected float rangedEscapeDistance = 10.0f;

    [Header("Ranged & Melee/Ranged Magic Variables")]
    [SerializeField] protected bool castInBursts = false;
    [SerializeField] protected float castingTime = 2.0f;
    [SerializeField] protected int burstCount = 3;
    [SerializeField] protected float timeBetweenShots = 0.2f;
     
    [Header("Shared Variables")]
    [SerializeField] protected EDefaultState defaultState = EDefaultState.IDLE;
    [SerializeField] protected EEnemyType enemyType = EEnemyType.MELEE;
    [SerializeField] protected bool useMagic = false;
    [SerializeField] protected bool searchPlayerAfterAttack = true;
    [SerializeField] protected bool alwaysAggressive = true;
    [SerializeField] protected float aggressiveRadius = 30.0f;
    [SerializeField] protected float hearingRadius = 10.0f;
    [SerializeField] protected float radiusCheckInterval = 1.0f;
    [SerializeField] protected float paranoidDuration = 5.0f;
    [SerializeField] protected float castingInterval = 1.0f;
    [SerializeField] protected float castingStandstill = 4.0f;
    [SerializeField] protected int castingSpellType = 0; //Check the animator controller to find out the desired number!
    [SerializeField] protected float logicUpdateInterval = 0.5f;
    [SerializeField] protected Animator animator = null;
    public Transform ragdollPosition = null;

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

    public enum EEnemyType
    {
        MELEE,
        RANGED
    };

    [HideInInspector] public EState currentState = EState.IDLE;

    public int entitySpawnNumber { get; private set; } = 0;
    public Vector3 spawnPosition { get; private set; } = Vector3.zero;
    public Vector3 spawnRotation { get; private set; } = Vector3.zero;
    public EEnemyType currentEnemyType { get; protected set; } = EEnemyType.MELEE;
    public EnemyVision cVision { get; private set; } = null;
    public EnemyNavigation cNavigation { get; private set; } = null;
    public Health cHealth { get; private set; } = null;
    public StatusEffects status { get; private set; } = new StatusEffects(false, false, false, false);
    public Spellbook cSpellBook { get; private set; } = null;

    //Temporary values
    protected bool bCastedProjectile = false;
    protected float radiusCheckTimer = 0.0f;
    protected float alertedTimer = 0.0f;
    protected float paranoidTimer = 0.0f;
    protected float shootIntervalTimer = 0.0f;
    protected float castingTimer = 0.0f;
    protected float castingStandstillTimer = 0.0f;
    protected int shotsLeft = 0;
    protected Vector3 playerOffset = Vector3.zero;
    protected Vector3 ragdollPrevPosition = Vector3.zero;
    protected float ragdollSleepTimer = 0.0f;

    public float RangedEscapeRadius
    {
        get
        {
            return rangedEscapeDistance;
        }
    }

    #endregion

    #region UNITY_DEFAULT_METHODS

    protected void Awake()
    {
        GlobalVariables.teamBadBoys.Add(this.gameObject);
        entitySpawnNumber = GlobalVariables.teamBadBoys.Count;
    }

    protected virtual void Start()
    {
        cVision = GetComponent<EnemyVision>();
        cNavigation = GetComponent<EnemyNavigation>();
        cHealth = GetComponent<Health>();

        if (useMagic)
        {
            cSpellBook = GetComponent<Spellbook>();
            if (cSpellBook == null)
            {
                Debug.LogWarning(this.gameObject + " is marked as a spellcaster, but has no spellbook!");
                useMagic = false;
            }
        }

        spawnPosition = transform.position;
        spawnRotation = transform.rotation.eulerAngles;
        currentState = (EState)defaultState;
        currentEnemyType = enemyType;

        InvokeRepeating("EnemyCoreLogic", Time.fixedDeltaTime * entitySpawnNumber, Time.fixedDeltaTime * Mathf.FloorToInt(logicUpdateInterval / Time.fixedDeltaTime));
    }

    protected virtual void Update()
    {
        AdvanceTimers();
    }

    protected virtual void FixedUpdate()
    {
        if (status.isKnocked)
        {
            if (Vector3.Distance(ragdollPosition.position, ragdollPrevPosition) < 0.01f)
            {
                if (ragdollSleepTimer > 0.0f)
                {
                    ragdollSleepTimer -= Time.fixedDeltaTime;
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
            ragdollPrevPosition = ragdollPosition.position;
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
                cHealth.Hurt(other.GetComponent<TriggerHurt>().damage);
            }
        }
    }

    protected void OnDrawGizmosSelected()
    {
        #if UNITY_EDITOR
        Handles.Label(transform.position + Vector3.up * 2.0f, currentState.ToString());
        Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, hearingRadius);

        if (!alwaysAggressive)
        {
            Gizmos.color = new Color(0.0f, 1.0f, 0.0f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, aggressiveRadius);
        }
        #endif
    }

    #endregion

    #region CUSTOM_METHODS

    protected void EnemyCoreLogic()
    {
        if (GlobalVariables.bAnyPlayersAlive == false)
        {
            currentState = EState.DISABLED;
        }

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

    protected void AdvanceTimers()
    {
        float time = Time.deltaTime;

        shootIntervalTimer -= shootIntervalTimer > 0.0f ? time : 0.0f;
        radiusCheckTimer -= radiusCheckTimer > 0.0f ? time : 0.0f;
        paranoidTimer -= paranoidTimer > 0.0f ? time : 0.0f;
        alertedTimer -= alertedTimer > 0.0f ? time : 0.0f;
        castingTimer -= castingTimer > 0.0f ? time : 0.0f;

        if (castingTimer <= 0.0f)
        {
            castingStandstillTimer -= castingStandstillTimer > 0.0f ? time : 0.0f;
        }
    }

    protected void NoiseChecker()
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

    public void EnableRagdoll(bool b)
    {
        status = new StatusEffects(status.isOnFire, status.isConfused, status.isFrozen, b);
        animator.enabled = !b;
        animator.gameObject.GetComponent<RagdollModifier>().SetKinematic(!b);
        animator.transform.parent = b ? null : transform;
        cNavigation.agent.enabled = !b;
        currentState = b ? EState.RAGDOLLED : EState.ATTACK;
        ragdollSleepTimer = 2.0f;

        if (!b)
        {
            cNavigation.agent.Warp(ragdollPosition.position);
        }
    }

    public virtual void OnHurt()
    {
        //animator.SetTrigger("Take Damage");

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
        GlobalVariables.teamBadBoys.Remove(this.gameObject);

        //Detach the enemy model and ragdoll it
        animator.enabled = false;
        animator.gameObject.GetComponent<RagdollModifier>().SetKinematic(false);
        animator.transform.parent = null;

        Destroy(this.gameObject);
    }

    #endregion

    #region AI_LOGIC

    protected virtual void AIIdle()
    {
        if (cVision.bCanSeeTarget)
        {
            if (!alwaysAggressive)
            {
                if (Vector3.Distance(transform.position, cVision.targetLocation) < aggressiveRadius)
                {
                    currentState = EState.ATTACK;
                }
            }
            else
            {
                currentState = EState.ATTACK;
            }
        }
        NoiseChecker();
    }

    protected virtual void AIPatrol()
    {
        if (cVision.bCanSeeTarget)
        {
            currentState = EState.ATTACK;
        }
        NoiseChecker();
    }

    protected virtual void AIAlerted()
    {
        if (cVision.bCanSeeTarget)
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

    protected virtual void AIParanoid()
    {
        if (cVision.bCanSeeTarget)
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
        NoiseChecker();
    }

    protected virtual void AISearch()
    {
        if (searchPlayerAfterAttack)
        {
            if (cVision.bCanSeeTarget)
            {
                currentState = EState.ATTACK;
            }
            else
            {
                if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(cVision.targetLocation.x, cVision.targetLocation.z)) < cNavigation.navigationErrorMargin 
                    || cVision.targetLocation == Vector3.zero)
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

    protected virtual void AIAttack()
    {
        if (cVision.bCanSeeTarget)
        {
            switch (currentEnemyType)
            {
                case EEnemyType.MELEE:
                    {
                        if (Vector3.Distance(transform.position, cVision.targetLocation) > meleeAttackDistance)
                        {
                            return;
                        }
                        break;
                    }
                case EEnemyType.RANGED:
                    {
                        if (Vector3.Distance(transform.position, cVision.targetLocation) < rangedEscapeDistance)
                        {
                            currentState = EState.ESCAPE;
                            return;
                        }
                        break;
                    }
            }

            if (shootIntervalTimer <= 0.0f)
            {
                if (castInBursts)
                {
                    shotsLeft = burstCount;
                }

                shootIntervalTimer = castingInterval;
                castingTimer = castingTime;
                castingStandstillTimer = castingStandstill;
                animator.SetTrigger("Cast Spell");
                animator.SetInteger("Spell Type", castingSpellType);
                currentState = EState.CASTING;
            }
        }
        else
        {
            currentState = EState.SEARCH;
        }
    }

    protected virtual void AICasting()
    {
        if (!useMagic && currentEnemyType == EEnemyType.MELEE)
        {
            if (cNavigation.agent.hasPath)
            {
                cNavigation.agent.ResetPath();
                cNavigation.agent.velocity = new Vector3(0.0f, cNavigation.agent.velocity.y, 0.0f);
            }

            if (!bCastedProjectile)
            {
                cVision.targetGO.GetComponent<Health>().Hurt(meleeDamage);
                bCastedProjectile = true;
            }

            if (castingStandstillTimer <= 0.0f)
            {
                bCastedProjectile = false;
                currentState = EState.ATTACK;
            }
        }
        else
        {
            if (castingTimer <= 0.0f)
            {
                if (!bCastedProjectile)
                {
                    //CastProjectile();
                    cSpellBook.CastSpell(0);
                    bCastedProjectile = true;
                }

                if (shotsLeft > 1)
                {
                    if (!cVision.bCanSeeTarget)
                    {
                        currentState = EState.ATTACK;
                        shootIntervalTimer *= 0.25f;
                        return;
                    }

                    animator.SetTrigger("Interrupt Spell");
                    animator.SetTrigger("Cast Spell");
                    bCastedProjectile = false;
                    castingTimer = timeBetweenShots;
                    shotsLeft--;
                }
                else if (castingStandstillTimer <= 0.0f)
                {
                    bCastedProjectile = false;
                    currentState = EState.ATTACK;

                }
            }
        }
    }

    protected virtual void AIEscape()
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

    protected virtual void AIPanic()
    {

    }

    protected virtual void AIRagdolled()
    {

    }

    #endregion
}
