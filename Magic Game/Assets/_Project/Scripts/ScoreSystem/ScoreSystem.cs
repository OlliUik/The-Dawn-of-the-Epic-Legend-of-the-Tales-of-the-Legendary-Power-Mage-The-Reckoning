using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    #region VARIABLES
    public float score = 0f;
    public float multiplier = 1f;       //Permanent multiplier

    public int killStreak = 0;
    public int segmentCount = 0;        //Updated when player enters brand new segment
    public int crystalsFound = 0;

    public bool crystalFound = false;
    #endregion

    #region PRIVATE_FUNCTIONS
    private void CrystalBoost()         //Updated when player finds crystal (multiplier is added by 0.1)
    {
        if (crystalFound)
        {
            crystalsFound++;
            multiplier += 0.1f;
            crystalFound = false;
        }
    }

    private void KillCombo()            //Updated when player kills enemy (lasts currently 3 seconds)
    {

    }
    #endregion
}