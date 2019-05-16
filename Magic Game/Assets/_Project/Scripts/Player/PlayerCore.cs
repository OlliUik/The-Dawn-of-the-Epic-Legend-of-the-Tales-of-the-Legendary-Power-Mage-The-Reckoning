using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Mana))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Spellbook))]
public class PlayerCore : MonoBehaviour
{
    #region VARIABLES

    [Header("Public")]
    public Transform ragdollPosition = null;

    [Header("Serialized")]
    [SerializeField] private HUDManager canvasManager = null;
    [SerializeField] private GameObject ragdollObject = null;
    [SerializeField] private PlayerAnimationHandler cAnimHandler = null;
    [SerializeField] private Text debugText = null;

    public Health cHealth { get; private set; } = null;
    public Mana cMana { get; private set; } = null;
    public ThirdPersonCamera cTPCamera { get; private set; } = null;
    public CharacterController cCharacter { get; private set; } = null;
    public PlayerMovement cMovement { get; private set; } = null;
    public Spellbook cSpellBook { get; private set; } = null;

    private bool bInputEnabled = true;
    private bool bIsDead = false;
    private bool bShotFired = false;
    private bool bIsRagdolled = false;
    private float ragdollSleepTimer = 0.0f;
    private Vector3 ragdollPrevPosition = Vector3.zero;
    public int activeSpellIndex = 0;
    public bool openSpellEditingOnStart = false;

    #endregion

    #region UNITY_DEFAULT_METHODS

    void Awake()
    {
        GlobalVariables.teamGoodGuys.Add(this.gameObject);
        GlobalVariables.bAnyPlayersAlive = true;

        cHealth = GetComponent<Health>();
        cMana = GetComponent<Mana>();
        cTPCamera = GetComponent<ThirdPersonCamera>();
        cMovement = GetComponent<PlayerMovement>();
        cCharacter = GetComponent<CharacterController>();
        cSpellBook = GetComponent<Spellbook>();
    }

    void Start()
    {
        if(openSpellEditingOnStart)
            ToggleSpellEditingUI();

        //Quaternion spawnRotation = transform.localRotation;
        //transform.localRotation = Quaternion.Euler(Vector3.zero);
        //cTPCamera.lookDirection = spawnRotation.eulerAngles;
    }

    void Update()
    {
        if (debugText != null)
        {
            debugText.text = "Active spell: " + activeSpellIndex;
        }

        if (bIsDead)
        {
            cTPCamera.cameraObject.transform.LookAt(ragdollPosition.position);
        }
        else
        {
            if (bInputEnabled)
            {
                if (Input.GetButtonDown("Fire1") || Input.GetAxisRaw("Fire1") != 0.0f)
                {
                    //Don't allow repeated input from controller axis
                    if (!bShotFired)
                    {
                        cSpellBook.CastSpell(activeSpellIndex);
                        bShotFired = true;
                        cAnimHandler.CastSpell(activeSpellIndex);
                    }
                }
                else
                {
                    bShotFired = false;
                }

                if (Input.GetButtonDown("Fire2"))
                {
                    if (!bIsRagdolled)
                    {
                        EnableRagdoll(true);
                    }
                }

                // CHANGING ACTIVE SPELL
                if(Input.mouseScrollDelta.y != 0 && !cSpellBook.isCasting)
                {
                    if(Input.mouseScrollDelta.y > 0)
                    {
                        activeSpellIndex++;

                        if(activeSpellIndex > 2)
                        {
                            activeSpellIndex = 0;
                        }
                    }
                    else
                    {
                        activeSpellIndex--;

                        if(activeSpellIndex < 0)
                        {
                            activeSpellIndex = 2;
                        }
                    }
                }
            }

            if(Input.GetKeyDown(KeyCode.Return))
            {
                ToggleSpellEditingUI();
            }

            if (Input.GetButtonDown("Escape"))
            {
                EnableControls(!canvasManager.FlipPauseState(this));
            }
        }
    }

    void FixedUpdate()
    {
        if (!bIsDead && bIsRagdolled && ragdollPosition != null)
        {
            cMovement.Teleport(ragdollPosition.position);

            cHealth.AddInvulnerability(Time.fixedDeltaTime);

            if ((ragdollPosition.position - ragdollPrevPosition).sqrMagnitude < 0.04f)
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
                cHealth.Hurt(other.GetComponent<TriggerHurt>().damage, false);
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

    public void ToggleSpellEditingUI()
    {
        EnableControls(!canvasManager.FlipSpellEditingState(this));
    }

    public HUDManager GetHUD()
    {
        return canvasManager;
    }

    public void EnableControls(bool b)
    {
        if (canvasManager.bIsPaused || canvasManager.bIsEditingSpells)
        {
            cTPCamera.EnableCameraControls(false);
        }
        else
        {
            cTPCamera.EnableCameraControls(b);
        }

        if (!bIsRagdolled)
        {
            bInputEnabled = b;
            cMovement.enableControls = b;
        }
        else
        {
            bInputEnabled = false;
            cMovement.enableControls = false;
        }
    }

    public void EnableRagdoll(bool b)
    {
        bIsRagdolled = b;
        cTPCamera.isRagdolled = b;
        ragdollSleepTimer = 3.0f;

        ragdollObject.GetComponent<RagdollModifier>().SetKinematic(!b, b);
        ragdollObject.GetComponent<Animator>().enabled = !b;
        ragdollObject.GetComponent<PlayerAnimationHandler>().enabled = !b;
        
        ragdollObject.transform.parent = b ? null : transform;

        //ragdollPosition.GetComponent<Rigidbody>().AddForce(cCharacter.velocity, ForceMode.VelocityChange);
        ragdollPosition.GetComponent<Rigidbody>().velocity = cCharacter.velocity * 6.0f;

        if (!b)
        {
            ragdollObject.transform.localPosition = Vector3.zero;
            cMovement.OnDisableRagdoll();
        }

        EnableControls(true);
    }

    public void OnHurt()
    {
        canvasManager.OnPlayerHurt();
        ragdollObject.GetComponent<Animator>().SetTrigger("Take Damage");
        //GetComponent<PlayerAnimations>().TakeDamage();
    }

    public void OnDeath()
    {
        bIsDead = true;
        GlobalVariables.teamGoodGuys.Remove(this.gameObject);

        GlobalVariables.bAnyPlayersAlive = false;
        foreach (GameObject item in GlobalVariables.teamGoodGuys)
        {
            if (item.tag == "Player")
            {
                GlobalVariables.bAnyPlayersAlive = true;
            }
        }

        canvasManager.OnPlayerDeath();
        EnableRagdoll(true);
        EnableControls(false);
        //cCharacter.enabled = false;
        //cMovement.enabled = false;
    }

    #endregion
}
