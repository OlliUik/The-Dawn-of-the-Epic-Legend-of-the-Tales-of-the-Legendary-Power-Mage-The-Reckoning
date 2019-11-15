using UnityEngine;

public class Health : MonoBehaviour
{
    #region VARIABLES

    [Header("Public")]
    public float maxHealth = 100.0f;

    [Header("Serialized")]
    [SerializeField] private float iFrameTime = 0.5f;
    [SerializeField] private float ragdollDamageThreshold = 50.0f;
    [SerializeField] public bool scaleWithCrystalsCollected ;
    [SerializeField] public bool ourStepDadKilled = false;
    [SerializeField] private float healthAddedByCrystal = 20.0f;
    [SerializeField] private float lizardKingdeath = 40.0f;

    public bool bIsDead { get; private set; } = false;
    public float health /*{ get; private set; }*/ = 0.0f;

    private bool bIsPlayer = false;
    private float iftTimer = 0.0f;
    private float originalMaxHealth = 100.0f;

    private StatusEffectManager effectManager;

    #endregion

    #region UNITY_DEFAULT_METHODS

    void Start()
    {
        originalMaxHealth = maxHealth;

        if (scaleWithCrystalsCollected)
        {
            maxHealth = maxHealth + healthAddedByCrystal * GlobalVariables.crystalsCollected;
        }

        if (ourStepDadKilled)
        {
            maxHealth = maxHealth + lizardKingdeath * GlobalVariables.angryBaddiesPoint;
        }



        health = maxHealth;

        if (GetComponent<PlayerCore>() != null)
        {
            bIsPlayer = true;
            GetComponent<PlayerCore>().GetHUD().SetHealth(health, maxHealth);
        }

        effectManager = GetComponent<StatusEffectManager>();
    }

    void Update()
    {
        iftTimer -= iftTimer > 0.0f ? Time.deltaTime : 0.0f;
    }

    #endregion

    #region CUSTOM_METHODS

    public void UpdateMaxHealth()
    {
        if (ourStepDadKilled)
        {
            Debug.Log(ourStepDadKilled.ToString());
            float oldMaxHealth = maxHealth;
            Debug.Log(oldMaxHealth.ToString());
            maxHealth = originalMaxHealth + lizardKingdeath * GlobalVariables.angryBaddiesPoint;
            Debug.Log(maxHealth.ToString());
            health = maxHealth;
            Debug.Log(health.ToString());
            if (bIsPlayer)
            {
                Debug.Log(bIsPlayer.ToString());
                GetComponent<PlayerCore>().GetHUD().SetHealth(health, maxHealth);
            }
            ourStepDadKilled = false;
        }

        if (scaleWithCrystalsCollected)
        {
            float oldMaxHealth = maxHealth;
            maxHealth = originalMaxHealth + healthAddedByCrystal * GlobalVariables.crystalsCollected;
            health = maxHealth;
            if (bIsPlayer)
            {
                GetComponent<PlayerCore>().GetHUD().SetHealth(health, maxHealth);
            }
            scaleWithCrystalsCollected = false;
        }

        
    }

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
    
    private float GetCurseMultiplier()
    {
        float temp = 1;
        if (GetComponent<CurseVariables>() != null)
        {
            CurseVariables[] curseVariables = GetComponents<CurseVariables>();
            foreach (CurseVariables curseVariable in curseVariables)
            {
                temp += curseVariable.DamageIncreasedPercentage;
            }
        }
        return temp;
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
                GetComponent<PlayerCore>().GetHUD().SetHealth(health, maxHealth);
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
