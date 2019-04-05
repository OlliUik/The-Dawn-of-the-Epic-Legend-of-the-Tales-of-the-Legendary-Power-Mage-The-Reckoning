using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Mana))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Spellbook))]
public class PlayerCore : MonoBehaviour
{
    #region VARIABLES

    [Header("Serialized")]
    [SerializeField] private HUDManager canvasManager = null;
    [SerializeField] private GameObject ragdollObject = null;
    [SerializeField] private Transform ragdollPosition = null;

    public Health cHealth { get; private set; } = null;
    public Mana cMana { get; private set; } = null;
    public ThirdPersonCamera cTPCamera { get; private set; } = null;
    public CharacterController cCharacter { get; private set; } = null;
    public PlayerMovement cMovement { get; private set; } = null;
    //public PlayerSpellCaster cSpellCaster { get; private set; } = null;
    public Spellbook cSpellBook { get; private set; } = null;

    private bool bInputEnabled = true;
    private bool bIsDead = false;
    private bool bShotFired = false;
    private bool bIsRagdolled = false;
    private float ragdollSleepTimer = 0.0f;
    private Vector3 ragdollPrevPosition = Vector3.zero;

    #endregion

    #region UNITY_DEFAULT_METHODS

    void Awake()
    {
        GlobalVariables.entityList.Add(this.gameObject);
        GlobalVariables.bAnyPlayersAlive = true;

        cHealth = GetComponent<Health>();
        cMana = GetComponent<Mana>();
        cTPCamera = GetComponent<ThirdPersonCamera>();
        cMovement = GetComponent<PlayerMovement>();
        cCharacter = GetComponent<CharacterController>();

        if (GetComponent<Spellbook>() != null)
        {
            cSpellBook = GetComponent<Spellbook>();
        }
        //if (GetComponent<PlayerSpellCaster>() != null)
        //{
        //    cSpellCaster = GetComponent<PlayerSpellCaster>();
        //}
    }

    void Start()
    {
        //Quaternion spawnRotation = transform.localRotation;
        //transform.localRotation = Quaternion.Euler(Vector3.zero);
        //cTPCamera.lookDirection = spawnRotation.eulerAngles;
    }

    void Update()
    {
        if (bIsDead)
        {
            //Camera.main.transform.LookAt(playerModel.transform);
        }
        else
        {
            if (bInputEnabled)
            {
                //cTPCamera.Look(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                //cMovement.GetInput(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), Input.GetButtonDown("Jump"), Input.GetButtonDown("Fire3"));

                if (Input.GetButtonDown("Fire1") || Input.GetAxisRaw("Fire1") != 0.0f)
                {
                    //Don't allow repeated input from controller axis
                    if (!bShotFired)
                    {
                        //cSpellCaster.CastSpell();
                        cSpellBook.CastSpell(0);
                        //GetComponent<PlayerAnimations>().CastSpell(0);
                        bShotFired = true;
                    }
                }
                else
                {
                    bShotFired = false;
                }

                if (Input.GetButtonDown("Fire2"))
                {
                    EnableRagdoll(!bIsRagdolled);
                    //cTPCamera.SwitchSide();
                }
            }

            if (Input.GetButtonDown("Escape"))
            {
                EnableControls(!canvasManager.FlipPauseState(this));
            }

            if(Input.GetKeyDown(KeyCode.Return))
            {
                EnableControls(!canvasManager.FlipSpellEditingState(this));
            }
        }
    }

    void FixedUpdate()
    {
        if (!bIsDead && bIsRagdolled && ragdollPosition != null)
        {
            cHealth.AddInvulnerability(Time.fixedDeltaTime);

            if (Vector3.Distance(ragdollPosition.position, ragdollPrevPosition) < 0.2f)
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

    void OnEnable()
    {
        if (bInputEnabled)
        {
            EnableControls(true);
        }
    }

    void OnDisable()
    {
        EnableControls(false);
    }

    #endregion

    #region CUSTOM_METHODS

    public HUDManager GetHUD()
    {
        return canvasManager;
    }

    public void EnableControls(bool b)
    {
        bInputEnabled = b;
        cMovement.enableControls = b;
        cTPCamera.EnableCameraControls(b);

        //if (cSpellCaster != null)
        //{
        //    cSpellCaster.CastBeamActive(b);
        //}
    }

    public void EnableRagdoll(bool b)
    {
        bIsRagdolled = b;
        ragdollObject.GetComponent<Animator>().enabled = !b;
        ragdollObject.GetComponent<PlayerAnimationHandler>().enabled = !b;
        cTPCamera.isRagdolled = b;
        cMovement.enableControls = !b;
        ragdollSleepTimer = 3.0f;

        if (!b)
        {
            cMovement.OnDisableRagdoll();
        }
    }

    public void OnHurt()
    {
        canvasManager.OnPlayerHurt();
        //GetComponent<PlayerAnimations>().TakeDamage();
    }

    public void OnDeath()
    {
        bIsDead = true;
        GlobalVariables.entityList.Remove(this.gameObject);

        GlobalVariables.bAnyPlayersAlive = false;
        foreach (GameObject item in GlobalVariables.entityList)
        {
            if (item.tag == "Player")
            {
                GlobalVariables.bAnyPlayersAlive = true;
            }
        }

        canvasManager.OnPlayerDeath();
        EnableControls(false);
        //cCharacter.enabled = false;
        //cMovement.enabled = false;
    }

    #endregion
}
