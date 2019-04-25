using UnityEngine;

public class Mana : MonoBehaviour
{
    #region VARIABLES

    [HideInInspector] public float regenerationMultiplier = 1.0f;

    [Header("Public")]
    public float maxMana = 100.0f;

    [Header("Serialized")]
    [SerializeField] private float regenerationPerSecond = 1.0f;
    [SerializeField] private float regenerationDelay = 2.0f;

    public float mana { get; private set; } = 0.0f;

    private bool bIsPlayer = false;
    private float regenDelayTimer = 0.0f;

    #endregion

    #region UNITY_DEFAULT_METHODS

    void Start()
    {
        mana = maxMana;
        if (GetComponent<PlayerCore>() != null)
        {
            bIsPlayer = true;
        }
    }

    void Update()
    {
        if (regenDelayTimer <= 0.0f)
        {
            if (mana < maxMana)
            {
                mana += regenerationPerSecond * regenerationMultiplier * Time.deltaTime;

                if (mana > maxMana)
                {
                    mana = maxMana;
                }
            }
        }
        else
        {
            regenDelayTimer -= Time.deltaTime;
        }

        if (bIsPlayer)
        {
            GetComponent<PlayerCore>().GetHUD().SetMana(mana, maxMana);
        }
    }

    #endregion

    #region CUSTOM_METHODS

    public void UseMana(float amount)
    {
        mana -= amount;
        regenDelayTimer = regenerationDelay;
    }

    #endregion
}
