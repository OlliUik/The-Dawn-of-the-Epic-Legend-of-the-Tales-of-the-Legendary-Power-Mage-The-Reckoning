﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>Handles score system's UI stuff.</summary>
public class ScoreUI : MonoBehaviour
{
    public static ScoreUI scoreUI;

    #region VARIABLES

    public string addedScoreString = null;

    private string scoreString = "Score: {value}";
    private string multiplierString = "Multiplier: {value}x";
    private string comboString = "Killstreak/combo: {value}";
    private string comboTimerString = "{value}";

    [SerializeField] private Text scoreText = null;
    [SerializeField] private Text addedScoreText = null;
    [SerializeField] private Text multiplierText = null;
    [SerializeField] private Text comboText = null;
    [SerializeField] private Text comboTimerText = null;

    #endregion

    #region UNITY_FUNCTIONS

    private void Start()
    {
        scoreUI = this;
    }

    private void Update()
    {
        addedScoreText.text = addedScoreString;
        scoreText.text = scoreString.Replace("{value}", ScoreSystem.scoreSystem.score.ToString(""));
        multiplierText.text = multiplierString.Replace("{value}", ScoreSystem.scoreSystem.multiplier.ToString(""));
        comboText.text = comboString.Replace("{value}", ScoreCombo.scoreCombo.combo.ToString(""));
        comboTimerText.text = comboTimerString.Replace("{value}", ScoreCombo.scoreCombo.comboTimer.ToString("F1"));
    }

    #endregion
}