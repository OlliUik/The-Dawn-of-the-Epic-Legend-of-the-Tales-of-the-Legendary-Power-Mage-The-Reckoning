using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCombo : MonoBehaviour
{
    public static ScoreCombo scoreCombo;

    #region VARIABLES

    public int combo = 0;

    [SerializeField]
    private float comboTimer = 3f;
    private float defaultTimer;
    private float comboScore = 0;

    private ScoreSystem scoreSystem;

    #endregion

    #region UNITY_FUNCTIONS

    private void Start()
    {
        scoreCombo = this;
        scoreSystem = FindObjectOfType<ScoreSystem>();
        defaultTimer = comboTimer;
    }

    private void Update()
    {
        //Player kills enemy, combo goes to 1
        if (combo >= 1)
        {
            comboTimer -= Time.deltaTime;

            //If timer goes to 0, combo resets
            if (comboTimer <= 0)
            {
                GiveScore();
                ResetCombo();
            }
        }
    }

    #endregion

    #region CUSTOM_FUNCTIONS

    /// <summary>Resets combo and timer.</summary>
    private void ResetCombo()
    {
        scoreSystem.score += comboScore;

        comboScore = 0;
        comboTimer = defaultTimer;
        combo = 0;
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