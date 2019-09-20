using UnityEngine;

public class Health : MonoBehaviour
{
    #region VARIABLES

    [Header("Public")]
    public float maxHealth = 100.0f;

    [Header("Serialized")]
    [SerializeField] private float iFrameTime = 0.5f;
    [SerializeField] private float ragdollDamageThreshold = 50.0f;

    public bool bIsDead { get; private set; } = false;
    [HideInInspector] public float health /*{ get; private set; }*/ = 0.0f;

    private bool bIsPlayer = false;
    private float iftTimer = 0.0f;

    private StatusEffectManager effectManager;

    #endregion

    #region UNITY_DEFAULT_METHODS

    void Start()
    {
        health = maxHealth;
        if (GetComponent<PlayerCore>() != null)
        {
            bIsPlayer = true;
        }

        effectManager = GetComponent<StatusEffectManager>();
    }

    void Update()
    {
        iftTimer -= iftTimer > 0.0f ? Time.deltaTime : 0.0f;
    }

    #endregion

    #region CUSTOM_METHODS
    
    public void Hurt(float amount, bool ignoreIFrames)
    {
        if (!bIsDead)
        {
            if (ignoreIFrames || iftTimer <= 0.0f)
            {

                if(effectManager != null && effectManager.AppliedEffects[StatusEffectManager.EffectType.StackingDamage])
                {
                    // this is effected by stacking damage
                    var stackingDamage = (StackingDamageEffect)effectManager.affectingEffects.Find(x => x.GetType() == typeof(StackingDamageEffect));
                    amount = stackingDamage.ModifyDamage(amount);
                    Debug.Log(amount);
                }

                iftTimer = iFrameTime;
                health -= amount;

                if (bIsPlayer)
                {
                    GetComponent<PlayerCore>().OnHurt();
                    GetComponent<PlayerCore>().GetHUD().SetHealth(health, maxHealth);

                    if (amount > ragdollDamageThreshold)
                    {
                        GetComponent<PlayerCore>().EnableRagdoll(true);
                    }
                }
                else
                {
                    GetComponent<EnemyCore>().OnHurt();

                    if (amount > ragdollDamageThreshold)
                    {
                        GetComponent<EnemyCore>().EnableRagdoll(true);
                    }
                }

                if (health <= 0.0f)
                {
                    Kill();
                }
            }
        }
    }

    public void Heal(float amount)
    {
        if (!bIsDead)
        {
            health += Mathf.Abs(amount);

            if (health > maxHealth)
            {
                health = maxHealth;
            }

            if (bIsPlayer)
            {
                GetComponent<PlayerCore>().GetHUD().SetHealth(health, maxHealth);
            }
        }
    }

    public void AddInvulnerability(float amount)
    {
        iftTimer += amount;
    }

    public void Kill()
    {
        if (!bIsDead)
        {
            health = 0.0f;
            bIsDead = true;

            if (bIsPlayer)
            {
                GetComponent<PlayerCore>().OnDeath();
            }
            else
            {
                GetComponent<EnemyCore>().OnDeath();
            }
        }
    }

    #endregion
}
