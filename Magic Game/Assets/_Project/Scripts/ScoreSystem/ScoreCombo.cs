using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCombo : MonoBehaviour
{
    #region VARIABLES

    [SerializeField]
    private float comboTimer = 3f;
    private float defaultTimer;
    private float comboScore = 0;
    private int combo = 0;

    #endregion

    #region UNITY_FUNCTIONS

    private void Start()
    {
        defaultTimer = comboTimer;
    }

    #endregion

    #region CUSTOM_FUNCTIONS

    /// <summary>If player kills an enemy, combo timer starts running.</summary>
    private void Combo()
    {
        //Player kills enemy, combo goes to 1

        if (combo > 1)
        {
            comboScore = 0;
            comboTimer -= Time.deltaTime;

            //If player kills within timer
            //combo++;
        }

        //If timer goes to 0, combo resets
        if (comboTimer <= 0)
        {
            GiveScore();
            ResetCombo();
        }
    }

    /// <summary>Resets combo and timer.</summary>
    private void ResetCombo()
    {
        combo = 0;
        comboTimer = defaultTimer;
    }
    
    /// <summary>Checks combo and rewards player accordingly.</summary>
    private float GiveScore()
    {
        if (combo >= 2 && combo <= 9)
        {
            //MORE THAN 2 but LESS THAN 10 enemies killed during combo - Add +777 points
            comboScore = 777;
        }

        else if (combo >= 10 && combo <= 19)
        {
            //MORE THAN 10 but LESS THAN 20 enemies killed during combo - Add +5 000 points
            comboScore = 5000;
        }

        else if (combo >= 20 && combo <= 29)
        {
            //MORE THAN 20 but LESS THAN 30 enemies killed during combo - Add +7 000 points
            comboScore = 7000;
        }

        else if (combo >= 30)
        {
            //MORE THAN 30 enemies killed during combo - Add +10 000 points
            comboScore = 10000;
        }

        else if (combo <= 1)
        {
            //If player kills 1 enemy during combo - Add NOTHING >:D
            comboScore = 0;
        }

        return comboScore;
    }

    #endregion
}