using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    public static ScoreCalculator scoreCalc;

    #region VARIABLES

    [SerializeField] private bool isSendable = false;
    [SerializeField] private float score = 0f;              //Score that will be send to score system
    [SerializeField] private float currentMultiplier = 1f;  //Player's current multiplier

    private ScoreSystem scoreSystem;
    private float effectMultiplier = 0f;                    //Temporary increment to the multiplier for kill (if applied)

    #endregion

    #region UNITY_FUNCTIONS

    private void Start()
    {
        scoreCalc = this;
        scoreSystem = FindObjectOfType<ScoreSystem>();
    }

    private void Update()
    {
        if (isSendable)
        {
            SendScore(score);       //Send counted score to score system
        }
    }

    #endregion

    #region CUSTOM_FUNCTIONS

    /// <summary>Starts counting score for the kill</summary>
    public void CountScore(float enemyScore)
    {
        //enemyScore comes from EnemyCore

        currentMultiplier = scoreSystem.multiplier;                     //Updates current multiplier to be the current permanent multiplier
        AddMultiplier();                                                //Adds multiplier if effects were on enemy upon dying
        score = enemyScore * (currentMultiplier + effectMultiplier);    //Score from enemy is multiplied with current multiplier and multiplier from effect

        isSendable = true;
    }

    private void SendScore(float addScore)
    {
        scoreSystem.score += addScore;
        
        score = 0;
        effectMultiplier = 0;

        isSendable = false;
    }

    private float AddMultiplier()
    {
        //We need way to check what effects were on enemy upon dying
        return effectMultiplier;
    }

    #endregion
}