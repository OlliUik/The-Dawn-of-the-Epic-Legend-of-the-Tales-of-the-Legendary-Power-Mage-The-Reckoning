using UnityEngine;

public class Mana : MonoBehaviour
{
    #region VARIABLES

    [SerializeField] private float maxMana = 100.0f;
    [SerializeField] private float regenerationPerSecond = 1.0f;

    public float mana { get; private set; } = 0.0f;

    private bool bIsPlayer = false;

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
        if (mana < maxMana)
        {
            mana += regenerationPerSecond * Time.deltaTime;
        }
        else
        {
            mana = maxMana;
        }

        GetComponent<PlayerCore>().GetHUD().SetMana(mana, maxMana);
    }

    #endregion

    #region CUSTOM_METHODS

    public void UseMana(float amount)
    {
        mana -= amount;
    }

    #endregion
}
