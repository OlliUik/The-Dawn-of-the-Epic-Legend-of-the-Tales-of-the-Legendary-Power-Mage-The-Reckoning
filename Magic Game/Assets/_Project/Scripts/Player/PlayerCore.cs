using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(ThirdPersonCamera))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(CharacterController))]
public class PlayerCore : MonoBehaviour
{
    #region VARIABLES

    [SerializeField] private HUDManager canvasManager           = null;
    [SerializeField] private GameObject playerModel             = null;

    public Health cHealth { get; private set; }                 = null;
    public Mana cMana { get; private set; }                     = null;
    public ThirdPersonCamera cTPCamera { get; private set; }    = null;
    public CharacterController cCharacter { get; private set; } = null;
    public PlayerMovement cMovement { get; private set; }       = null;
    public LayerMask physicsLayerMask { get; private set; }     = 1;

    private bool bInputEnabled                                  = true;
    private bool bIsDead                                        = false;
    private bool bShotFired                                     = false;
    private PlayerSpellCaster cSpellCaster                      = null;
    private Spellbook cSpellBook = null;

    #endregion

    #region UNITY_DEFAULT_METHODS

    void Awake()
    {
        GlobalVariables.player = this;
        GlobalVariables.entityList.Add(this.gameObject);

        cHealth         = GetComponent<Health>();
        cTPCamera       = GetComponent<ThirdPersonCamera>();
        cMovement       = GetComponent<PlayerMovement>();
        cCharacter      = GetComponent<CharacterController>();
        cSpellBook = GetComponent<Spellbook>();

        if (GetComponent<Mana>() != null)
        {
            cMana = GetComponent<Mana>();
        }
        if (GetComponent<PlayerSpellCaster>() != null)
        {
            cSpellCaster = GetComponent<PlayerSpellCaster>();
        }
    }

    void Update()
    {
        if (bIsDead)
        {
            Camera.main.transform.LookAt(playerModel.transform);
        }
        else
        {
            if (bInputEnabled)
            {
                cTPCamera.Look(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                cMovement.GetInput(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), Input.GetButtonDown("Jump"), Input.GetButtonDown("Fire3"));

                if (Input.GetButtonDown("Fire1") || Input.GetAxisRaw("Fire1") != 0.0f)
                {
                    //Don't allow repeated input from controller axis
                    if (!bShotFired)
                    {
                        cSpellCaster.CastSpell();
                        bShotFired = true;
                    }
                }
                else
                {
                    bShotFired = false;
                }

                if (Input.GetButtonDown("Fire2"))
                {
                    cTPCamera.SwitchSide();
                }
            }

            if (Input.GetButtonDown("Escape"))
            {
                EnableControls(!canvasManager.FlipPauseState(this));
            }
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
        Cursor.lockState = b ?
            CursorLockMode.Locked
            : CursorLockMode.None;
        Cursor.visible = !b;

        bInputEnabled = b;

        if (cSpellCaster != null)
        {
            cSpellCaster.CastBeamActive(b);
        }
    }

    public void OnHurt()
    {
        canvasManager.OnPlayerHurt();
    }

    public void OnDeath()
    {
        bIsDead = true;
        GlobalVariables.entityList.Remove(this.gameObject);
        canvasManager.OnPlayerDeath();
        EnableControls(false);

        if (playerModel != null)
        {
            cCharacter.enabled = false;
            GetComponent<PlayerMovement>().enabled = false;
            playerModel.GetComponent<PlayerModelRotator>().enabled = false;
            playerModel.GetComponent<Rigidbody>().isKinematic = false;
            playerModel.GetComponent<CapsuleCollider>().enabled = true;
            playerModel.transform.SetParent(null);
            playerModel.GetComponent<Rigidbody>().AddForce(playerModel.transform.forward + playerModel.transform.up * -2.0f, ForceMode.VelocityChange);
        }
    }

    #endregion
}
