using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>Handles score system's UI stuff.</summary>
public class ScoreUI : MonoBehaviour
{
    public static ScoreUI scoreUI;

    #region VARIABLES

    public string addedScoreString = null;
    public string notificationString = null;

    public Color colorChange = new Color();

    private string scoreString = "{value}";
    private string multiplierString = "Multiplier: {value}x";
    private string comboString = "Killstreak: {value}";
    private string comboTimerString = "{value}";
    private float notificationTimer = 2.0f;

    [SerializeField] private Text scoreText = null;
    [SerializeField] private Text addedScoreText = null;
    [SerializeField] private Text multiplierText = null;
    [SerializeField] private Text comboText = null;
    [SerializeField] private Text comboTimerText = null;
    [SerializeField] private Text notificationText = null;

    public bool roasted;
    public bool cooleddown;
    public bool flooded;
    public bool thunderstruck;
    public bool suckeddry;
    public bool smackeddown;
    public bool blownaway;
    public bool doubletrouble;
    public bool tripletrouble;

    #endregion

    #region UNITY_FUNCTIONS

    private void Start()
    {
        scoreUI = this;
    }

    private void Update()
    {
        SetTexts();

        if (notificationString != null)
        {
            notificationTimer -= Time.deltaTime;

            if (notificationTimer <= 0)
            {
                notificationString = null;
            }
        }

        else
        {
            notificationTimer = 2.0f;
        }
    }

    #endregion

    #region CUSTOM_FUNCTIONS

    private void SetTexts()
    {
        addedScoreText.text = addedScoreString;
        addedScoreText.color = colorChange;

        scoreText.text = scoreString.Replace("{value}", ScoreSystem.scoreSystem.score.ToString(""));
        multiplierText.text = multiplierString.Replace("{value}", ScoreSystem.scoreSystem.multiplier.ToString(""));
        comboText.text = comboString.Replace("{value}", ScoreCombo.scoreCombo.combo.ToString(""));
        comboTimerText.text = comboTimerString.Replace("{value}", ScoreCombo.scoreCombo.comboTimer.ToString("F1"));
        notificationText.text = notificationString;
    }

    #endregion
}