using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Handles player's current score and multiplier</summary>
public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem scoreSystem;

    #region VARIABLES
    
    public int score = 0;
    public float multiplier = 1.0f;       //Permanent multiplier
    public bool crystalFound = false;

    #endregion

    #region UNITY_FUNCTIONS

    private void Start()
    {
        scoreSystem = this;
    }

    private void Update()
    {
        CrystalBoost();
    }

    #endregion

    #region CUSTOM_FUNCTIONS

    /// <summary>If player finds crystal, multiplier will changes 0.1x permanently.</summary>
    private void CrystalBoost()
    {
        if (crystalFound)
        {
            multiplier += 0.1f;         //Enemy level multiplier
            crystalFound = false;
        }
    }

    #endregion
}