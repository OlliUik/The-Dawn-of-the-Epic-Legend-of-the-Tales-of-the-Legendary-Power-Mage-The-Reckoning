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
    [SerializeField] private float meleeAttackDistance = 2.0f;
    [SerializeField] private bool searchPlayerAfterAttack = true;
    [SerializeField] private float hearingRadius = 10.0f;
    //[SerializeField] private float instantSightRadius = 3.0f;
    [SerializeField] private float rangedEscapeDistance = 10.0f;
    [SerializeField] private float radiusCheckInterval = 1.0f;
    [SerializeField] private float paranoidDuration = 5.0f;
    [SerializeField] private GameObject projectile = null;
    [SerializeField] private float shootInterval = 5.0f;
    [SerializeField] private float castingTime = 2.0f;
    [SerializeField] private float castingStandstill = 4.0f;
    [SerializeField] private int castingSpellType = 0; //Check the animator controller to find out the desired number!
    [SerializeField] private Animator animator = null;
    [SerializeField] private BoxCollider meleeHitbox = null;

    public Vector3 spawnPosition { get; private set; } = Vector3.zero;
    public Vector3 spawnRotation { get; private set; } = Vector3.zero;
    public EState currentState { get; private set; } = EState.IDLE;
    public EEnemyType currentEnemyType { get; private set; } = EEnemyType.MELEE;
    public EnemyVision vision { get; private set; } = null;
    public EnemyNavigation navigation { get; private set; } = null;
    public Health cHealth { get; private set; } = null;
    public StatusEffects status { get; private set; } = new StatusEffects(false, false, false, false);
    //public Spellbook cSpellBook { get; private set; } = null;

    //Temporary values
    private bool bCastedProjectile = false;
    private float radiusCheckTimer = 0.0f;
    private float alertedTimer = 0.0f;
    private float paranoidTimer = 0.0f;
    private float shootIntervalTimer = 0.0f;
    private float castingTimer = 0.0f;
    private float castingStandstillTimer = 0.0f;
    private Vector3 targetPosition = Vector3.zero;
    private Vector3 playerOffset = Vector3.zero;

    public float RangedEscapeRadius
    {
        get
        {
            return rangedEscapeDistance;
        }
    }

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
        //cSpellBook = GetComponent<Spellbook>();

        spawnPosition = transform.position;
        spawnRotation = transform.rotation.eulerAngles;
        currentState = (EState)defaultState;
        currentEnemyType = enemyType;
    }

    void Update()
    {
        AdvanceTimers();
        targetPosition = vision.targetLocation;

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

    void OnDrawGizmosSelected()
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

        shootIntervalTimer -= shootIntervalTimer > 0.0f ? time : 0.0f;
        radiusCheckTimer -= radiusCheckTimer > 0.0f ? time : 0.0f;
        paranoidTimer -= paranoidTimer > 0.0f ? time : 0.0f;
        alertedTimer -= alertedTimer > 0.0f ? time : 0.0f;
        castingTimer -= castingTimer > 0.0f ? time : 0.0f;
        castingStandstillTimer -= castingStandstillTimer > 0.0f ? time : 0.0f;
    }

    void NoiseChecker()
    {
        if (radiusCheckTimer <= 0.0f)
        {
            radiusCheckTimer = radiusCheckInterval;
            
            GameObject[] soundObjects = GameObject.FindGameObjectsWithTag("SFX");

            foreach (GameObject go in soundObjects)
            {
                if (go.activeInHierarchy && Vector3.Distance(transform.position, go.transform.position) < hearingRadius)
                {
                    if (go.GetComponent<AudioSourceIdentifier>() != null)
                    {
                        if (go.GetComponent<AudioSourceIdentifier>().isPlayer)
                        {
                            if (go.GetComponent<AudioSourceIdentifier>().madeNoise)
                            {
                                vision.targetLocation = go.transform.position;
                                currentState = EState.SEARCH;
                            }
                        }
                    }
                }
            }
        }
    }

    void CastProjectile()
    {
        if (projectile != null)
        {
            Vector3 direction = -Vector3.Normalize(transform.position + Vector3.up * 1.0f - (vision.targetLocation));
            Instantiate(projectile).GetComponent<ProjectileTemp>().Initialize(transform.position + Vector3.up * 1.0f, direction, this.gameObject);
        }
        else
        {
            Debug.LogWarning(this.gameObject + " tried to cast a spell, but it has no projectile prefab assigned to it!");
        }
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

    void AIIdle()
    {
        if (vision.bCanSeeTarget)
        {
            currentState = EState.ATTACK;
        }
        NoiseChecker();
    }

    void AIPatrol()
    {
        if (vision.bCanSeeTarget)
        {
            currentState = EState.ATTACK;
        }
        NoiseChecker();
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
        NoiseChecker();
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
                if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(vision.targetLocation.x, vision.targetLocation.z)) < navigation.navigationErrorMargin 
                    || vision.targetLocation == Vector3.zero)
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
            switch (currentEnemyType)
            {
                case EEnemyType.MELEE:
                    {
                        if (Vector3.Distance(transform.position, vision.targetLocation) < meleeAttackDistance)
                        {
                            if (castingStandstillTimer > 0.0f)
                            {
                                animator.SetTrigger("Interrupt Spell");
                                return;
                            }
                            castingStandstillTimer = castingStandstill;
                            animator.SetTrigger("Cast Spell");
                            animator.SetInteger("Spell Type", castingSpellType);
                            currentState = EState.CASTING;
                        }
                        break;
                    }
                case EEnemyType.RANGED:
                    {
                        if (Vector3.Distance(transform.position, targetPosition) < rangedEscapeDistance)
                        {
                            currentState = EState.ESCAPE;
                            return;
                        }

                        if (shootIntervalTimer <= 0.0f)
                        {
                            shootIntervalTimer = shootInterval;
                            castingTimer = castingTime;
                            castingStandstillTimer = castingStandstill;
                            animator.SetTrigger("Cast Spell");
                            animator.SetInteger("Spell Type", castingSpellType);
                            currentState = EState.CASTING;
                        }
                        break;
                    }
                case EEnemyType.MAGIC:
                    {
                        if (Vector3.Distance(transform.position, targetPosition) < rangedEscapeDistance)
                        {
                            currentState = EState.ESCAPE;
                            return;
                        }

                        if (shootIntervalTimer <= 0.0f)
                        {
                            shootIntervalTimer = shootInterval;
                            castingTimer = castingTime;
                            animator.SetTrigger("Cast Spell");
                            animator.SetInteger("Spell Type", 3);
                            currentState = EState.CASTING;
                        }
                        break;
                    }
            }
        }
        else
        {
            currentState = EState.SEARCH;
        }
    }

    void AICasting()
    {
        if (currentEnemyType == EEnemyType.MELEE)
        {
            meleeHitbox.enabled = true;
            if (castingStandstillTimer <= 0.0f)
            {
                currentState = EState.ATTACK;
                meleeHitbox.enabled = false;
            }
        }
        else
        {
            if (castingTimer <= 0.0f)
            {
                if (!bCastedProjectile)
                {
                    CastProjectile();
                    //cSpellBook.CastSpell(0);
                    bCastedProjectile = true;
                }

                if (castingStandstillTimer <= 0.0f)
                {
                    bCastedProjectile = false;
                    currentState = EState.ATTACK;
                }
            }
        }
    }

    void AIEscape()
    {
        if (vision.bCanSeeTarget)
        {
            if (Vector3.Distance(transform.position, targetPosition) > rangedEscapeDistance * 2)
            {
                currentState = EState.ATTACK;
            }
        }
        else
        {
            currentState = EState.SEARCH;
        }
    }

    void AIPanic()
    {

    }

    void AIRagdolled()
    {

    }

    #endregion
}
