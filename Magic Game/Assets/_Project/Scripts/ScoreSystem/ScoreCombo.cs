using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>Keeps track of player's killstreak/combo and awards player accordingly</summary>
public class ScoreCombo : MonoBehaviour
{
    public static ScoreCombo scoreCombo;

    #region VARIABLES

    public bool isEnemyKilled = false;
    public int combo = 0;
    public float comboTimer = 7.0f;

    private float defaultTimer = 0.0f;
    private int comboScore = 0;

    private GameObject player = null;
    private ScoreSystem scoreSystem = null;

    #endregion

    #region UNITY_FUNCTIONS

    private void Start()
    {
        scoreCombo = this;
        player = GameObject.FindGameObjectWithTag("Player");
        scoreSystem = FindObjectOfType<ScoreSystem>();
        defaultTimer = comboTimer;
    }

    private void Update()
    {
        if (isEnemyKilled)
        {
            comboTimer = defaultTimer;
            isEnemyKilled = false;
        }

        //Player kills enemy, combo goes to 1
        if (combo > 0)
        {
            comboTimer -= Time.deltaTime;

            //If timer goes to 0, combo resets
            if (comboTimer <= 0 || player.GetComponent<Health>().bIsDead)
            {
                GiveScore();
                ResetCombo();
            }
        }
    }

    #endregion

    #region CUSTOM_FUNCTIONS

    ///<summary>Resets combo and timer.</summary>
    private void ResetCombo()
    {
        scoreSystem.addedScore = comboScore;
        ScoreUI.scoreUI.colorChange = Color.blue;
        scoreSystem.score += comboScore;
        comboScore = 0;
        comboTimer = defaultTimer;
        combo = 0;
    }
    
    ///<summary>Checks combo and rewards player accordingly.</summary>
    private float GiveScore()
    {
        if (combo >= 2 && combo <= 9)
        {
            ScoreUI.scoreUI.notificationString = "Killstreak!";
            comboScore = 777;
        }

        else if (combo >= 10 && combo <= 19)
        {
            ScoreUI.scoreUI.notificationString = "Nice killstreak!";
            comboScore = 5000;
        }

        else if (combo >= 20 && combo <= 29)
        {
            ScoreUI.scoreUI.notificationString = "Great killstreak!";
            comboScore = 7000;
        }

        else if (combo >= 30)
        {
            ScoreUI.scoreUI.notificationString = "Awesome killstreak!";
            comboScore = 10000;
        }

        else
        {
            comboScore = 0;
        }

        return comboScore;
    }

    #endregion
}