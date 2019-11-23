using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>Handles player's current score and multiplier</summary>
public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem scoreSystem;

    #region VARIABLES
    
    public int score = 0;
    public int addedScore = 0;
    public float multiplier = 1.0f;       //Permanent multiplier
    public bool crystalFound = false;
    public Rank rankInfo;
    
    private float timer = 1.0f;

    #endregion

    #region INNERCLASS

    //Singleton innerclass that store rankinfo and you can get the epic name of current rank.
    public sealed class Rank
    {
        public ERank currentRank { get; set; }
        private static Rank instant;

        public enum ERank
        {
            NB,
            WC,
            EL,
            ME,
            TBFD,
            EKSME,
            KHUC,
            TEDDW,
            SLHHDL,
            A3M,
            GMCMU,
            TEGIOTD,
            TMHBGIPOMFF
        }
        private Rank()
        {
            currentRank = ERank.NB;
        }

        public string getRankText()
        {
            switch (currentRank)
            {
                case ERank.NB: return "Noob";
                case ERank.WC: return "Wild Child";
                case ERank.EL: return "Epic Legend";
                case ERank.ME: return "Master Exploder";
                case ERank.TBFD: return "True Beast, the Final Devastator";
                case ERank.EKSME: return "Elite Killer Super Mega Eiminator";
                case ERank.KHUC: return "King of the Hill, the Ultimate Champion";
                case ERank.TEDDW: return "The Eternal Doom, Destroyer of Worlds";
                case ERank.SLHHDL: return "Savior of Life from the Highest Heaven of Divine Light";
                case ERank.A3M: return "Absolute Monster, the Megalomanic Mass Exterminator";
                case ERank.GMCMU: return "Godsent Megahero, the Chosen Master of the Universe";
                case ERank.TEGIOTD: return "The Extreme Grandmaster, the Immortal Omniguru and the Total Dominator";
                case ERank.TMHBGIPOMFF: return "The Most High Being: God of Infinite Power and the Overlord of the Multiverse - the Final Form";
                default: return "Noob";
            }
        }

        public static Rank getInstant()
        {
            if (instant == null)
            {
                instant = new Rank();
            }
            return instant;
        }
    }

    #endregion

    #region UNITY_FUNCTIONS

    private void Start()
    {
        scoreSystem = this;
        rankInfo = Rank.getInstant();
    }

    private void Update()
    {
        CrystalBoost();
        ScoreUpdate();
        RankUpdate();
    }

    #endregion

    #region CUSTOM_FUNCTIONS

    ///<summary>If player finds crystal, multiplier will changes 0.1x permanently.</summary>
    private void CrystalBoost()
    {
        if (crystalFound)
        {
            multiplier += 0.1f;         //Enemy level multiplier
        }
       crystalFound = false;
     }

    ///<summary>Handles notifications of score player has just gotten.</summary>
    private void ScoreUpdate()
    {
        if (addedScore > 0)
        {
            timer -= Time.deltaTime;
            ScoreUI.scoreUI.addedScoreString = "+" + addedScore;

            if (timer < 0)
            {
                addedScore = 0;
            }
        }

        else
        {
            ScoreUI.scoreUI.addedScoreString = "";
            ScoreUI.scoreUI.colorChange = Color.red;
            timer = 1.0f;
        }
    }

    ///<summary>Keep update of the player's rank base on current score.</summary>
    private void RankUpdate()
    {
        if (score < 10000)
        {
            rankInfo.currentRank = Rank.ERank.NB;
        } else if (score >= 10000 && score < 100000)
        {
            rankInfo.currentRank = Rank.ERank.WC;
        } else if (score >= 100000 && score < 150000)
        {
            rankInfo.currentRank = Rank.ERank.EL;
        } else if (score >= 150000 && score < 200000)
        {
            rankInfo.currentRank = Rank.ERank.ME;
        } else if (score >= 200000 && score < 250000)
        {
            rankInfo.currentRank = Rank.ERank.TBFD;
        } else if (score >= 250000 && score < 300000)
        {
            rankInfo.currentRank = Rank.ERank.EKSME;
        } else if (score >= 300000 && score < 350000)
        {
            rankInfo.currentRank = Rank.ERank.KHUC;
        } else if (score >= 350000 && score < 400000)
        {
            rankInfo.currentRank = Rank.ERank.TEDDW;
        } else if (score >= 400000 && score < 450000)
        {
            rankInfo.currentRank = Rank.ERank.SLHHDL;
        } else if (score >= 450000 && score < 500000)
        {
            rankInfo.currentRank = Rank.ERank.A3M;
        } else if (score >= 550000 && score < 600000)
        {
            rankInfo.currentRank = Rank.ERank.GMCMU;
        } else if (score >= 600000 && score < 650000)
        {
            rankInfo.currentRank = Rank.ERank.TEGIOTD;
        } else if (score >= 650000)
        {
            rankInfo.currentRank = Rank.ERank.TMHBGIPOMFF;
        }
    }

    #endregion
}