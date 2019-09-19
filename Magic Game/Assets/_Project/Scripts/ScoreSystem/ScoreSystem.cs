using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public float score = 0f;
    public float multiplier = 1f;       //Permanent multiplier
    public int killStreak = 0;
    public int crystalsFound = 0;       //Updated when player finds crystal
    public int segmentCount = 0;        //Updated when player enters brand new segment

    
}