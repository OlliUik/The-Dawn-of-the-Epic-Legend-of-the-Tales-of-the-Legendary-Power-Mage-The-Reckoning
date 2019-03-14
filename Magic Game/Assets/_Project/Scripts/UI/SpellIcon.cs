//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SpellIcon : MonoBehaviour
{
    /*
    //Commenting whole thing as other scripts this one uses change very rapidly.

    #region VARIABLES

    [SerializeField] private SpellStats spellData;
    [SerializeField] private Text spellNameText = null;
    [SerializeField] private Text spellCooldownText = null;
    
    private bool isUsed = false;
    private float cooldownTimer = 0.0f;

    #endregion

    #region UNITY_DEFAULT_METHODS

    void Start()
    {
        UpdateSpellData(null);
        spellCooldownText.text = "";
    }

    void Update()
    {
        if (cooldownTimer > 0.0f)
        {
            cooldownTimer -= Time.deltaTime;
            spellCooldownText.text = cooldownTimer.ToString("0.0");
        }
        else if (isUsed)
        {
            cooldownTimer = 0.0f;
            isUsed = false;
            GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);
            spellCooldownText.text = "";
        }
    }

    #endregion
    
    #region CUSTOM_METHODS

    public void UpdateSpellData(SpellStats sd)
    {
        if (sd != null)
        {
            spellData = sd;
        }

        if (spellData != null)
        {
            GetComponent<Image>().sprite = spellData.icon;

            if (spellNameText != null)
            {
                spellNameText.text = spellData.spellName;
            }
            else
            {
                Debug.LogWarning(this.gameObject + " has no text object attached to it!");
            }
        }
        else
        {
            Debug.LogWarning(this.gameObject + " has no Spell Data attached to it!");
        }
    }

    public bool SpellStatus()
    {
        return isUsed;
    }

    public void SpellStatus(bool b)
    {
        isUsed = b;

        if (b)
        {
            //baseCooldown got removed, using a placeholder value instead.
            //cooldownTimer = spellData.baseCooldown;

            cooldownTimer = 5.0f;
            GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
        }
        else
        {
            cooldownTimer = 0.0f;
        }
    }

    #endregion

    */
}
