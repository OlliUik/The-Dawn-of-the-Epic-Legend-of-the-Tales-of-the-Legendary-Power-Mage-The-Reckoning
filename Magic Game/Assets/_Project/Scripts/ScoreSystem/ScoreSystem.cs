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
            M = 0, // doesn't really matter to have value because it's the lowest rank from 0 to next rank.
            WC = 100000,
            EL = 150000,
            ME = 270000,
            TBFD = 330000,
            EKSME = 420000,
            KHUC = 510000,
            TEDDW = 660000,
            SLHHDL = 770000,
            A3M = 880000,
            GMCMU = 990000,
            TEGIOTD = 1000000,
            TMHBGIPOMFF = 1110000
        }
        private Rank()
        {
            currentRank = ERank.M;
        }

        public string getRankText()
        {
            switch (currentRank)
            {
                case ERank.M: return "Muggle";
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
                default: return "Muggle";
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
        if (score < (int)Rank.ERank.WC)
        {
            rankInfo.currentRank = Rank.ERank.M;
        } else if (score < (int)Rank.ERank.EL)
        {
            rankInfo.currentRank = Rank.ERank.WC;
        } else if (score < (int)Rank.ERank.ME)
        {
            rankInfo.currentRank = Rank.ERank.EL;
        } else if (score < (int)Rank.ERank.TBFD)
        {
            rankInfo.currentRank = Rank.ERank.ME;
        } else if (score < (int)Rank.ERank.EKSME)
        {
            rankInfo.currentRank = Rank.ERank.TBFD;
        } else if (score < (int)Rank.ERank.KHUC)
        {
            rankInfo.currentRank = Rank.ERank.EKSME;
        } else if (score < (int)Rank.ERank.TEDDW)
        {
            rankInfo.currentRank = Rank.ERank.KHUC;
        } else if (score < (int)Rank.ERank.SLHHDL)
        {
            rankInfo.currentRank = Rank.ERank.TEDDW;
        } else if (score < (int)Rank.ERank.A3M)
        {
            rankInfo.currentRank = Rank.ERank.SLHHDL;
        } else if (score < (int)Rank.ERank.GMCMU)
        {
            rankInfo.currentRank = Rank.ERank.A3M;
        } else if (score < (int)Rank.ERank.TEGIOTD)
        {
            rankInfo.currentRank = Rank.ERank.GMCMU;
        } else if (score < (int)Rank.ERank.TMHBGIPOMFF)
        {
            rankInfo.currentRank = Rank.ERank.TEGIOTD;
        } else if (score >= (int)Rank.ERank.TMHBGIPOMFF)
        {
            rankInfo.currentRank = Rank.ERank.TMHBGIPOMFF;
        }

    }

    #endregion
}