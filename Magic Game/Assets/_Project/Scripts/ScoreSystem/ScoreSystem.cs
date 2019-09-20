using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Handles player's current score and multiplier</summary>
public class ScoreSystem : MonoBehaviour
{
    #region VARIABLES

    public float score = 0f;
    public float multiplier = 1f;       //Permanent multiplier
    public bool crystalFound = false;

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