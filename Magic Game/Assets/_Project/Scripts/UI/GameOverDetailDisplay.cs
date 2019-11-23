using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverDetailDisplay : MonoBehaviour    
{
    [SerializeField] private Text totalScore = null;

    // Start is called before the first frame update
    void Start()
    {
        DisplayTotalScore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DisplayTotalScore()
    {
        totalScore.text = ScoreSystem.scoreSystem.score.ToString();
    }
}
