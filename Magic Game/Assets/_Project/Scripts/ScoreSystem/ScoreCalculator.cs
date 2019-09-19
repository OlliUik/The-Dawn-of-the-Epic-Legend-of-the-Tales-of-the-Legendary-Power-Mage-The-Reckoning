using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    [SerializeField] private float defaultScore = 0f;       //Default score enemy type gives
    [SerializeField] private float score = 0f;              //Score that will be send to score system
    [SerializeField] private float currentMultiplier = 1f;  //Player's current multiplier

    private float tempIncrease = 0f;                        //Temporary increment to the multiplier

    [SerializeField] private GameObject player;
    private ScoreSystem scoreSystem;

    void Start()
    {
        scoreSystem = player.GetComponent<ScoreSystem>();
    }

    void Update()
    {
        CountScore();           //Count score
        SendScore(score);       //Send counted score to score system
    }

    private void CountScore()
    {
        EnemyType();     //Check enemy's type

        currentMultiplier = scoreSystem.multiplier;
        currentMultiplier += AddMultiplier();
        score = defaultScore * currentMultiplier;
    }

    private void SendScore(float newScore)
    {
        scoreSystem.score += newScore;
        //Update multipliers
    }

    private void EnemyType()
    {
        //We need way to check enemy type
        AddMultiplier();
    }

    private float AddMultiplier()
    {
        //We need way to check what effects where on enemy upon dying
        return tempIncrease;
    }
}