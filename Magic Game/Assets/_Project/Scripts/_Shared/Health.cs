using UnityEngine;

public class Health : MonoBehaviour
{
    #region VARIABLES

    [SerializeField] private float maxHealth = 100.0f;
    [SerializeField] private float iFrameTime = 0.5f;

    public bool bIsDead { get; private set; } = false;
    public float health { get; private set; } = 0.0f;

    private bool bIsPlayer = false;
    private float iftTimer = 0.0f;

    #endregion

    #region UNITY_DEFAULT_METHODS

    void Start()
    {
        health = maxHealth;
        if (GetComponent<PlayerCore>() != null)
        {
            bIsPlayer = true;
        }
    }

    void Update()
    {
        iftTimer -= iftTimer > 0.0f ? Time.deltaTime : 0.0f;
    }

    #endregion

    #region CUSTOM_METHODS
    
    public void Hurt(float amount)
    {
        if (!bIsDead)
        {
            if (iftTimer <= 0.0f)
            {
                iftTimer = iFrameTime;
                health -= amount;

                if (bIsPlayer)
                {
                    GetComponent<PlayerCore>().OnHurt();
                    GetComponent<PlayerCore>().GetHUD().SetHealth(health, maxHealth);
                }
                else
                {
                    GetComponent<EnemyCore>().OnHurt();
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
