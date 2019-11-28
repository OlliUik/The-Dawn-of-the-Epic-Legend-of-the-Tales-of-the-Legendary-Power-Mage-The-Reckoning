using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGlobalVariables
{

    private static readonly float healthRate = 20f;
    private static readonly float damageRate = 2f;
    private static readonly int enemyIncreaseRate = 5;
    private static readonly int bossScoreIncrease = 10000;


    public static float enemyExtraHealth = 0f;
    public static float enemyExtraDamage = 0;
    public static int extraEnemyAmount = 0;
    public static int bossExtraScore = 0;
    public static bool isKilled;

    public static void StageUp()
    {
        enemyExtraHealth += healthRate;
        enemyExtraDamage += damageRate;
        extraEnemyAmount += enemyIncreaseRate;
    }

    public static void increaseBossScore()
    {
        bossExtraScore += bossScoreIncrease;
        isKilled = true;
    }

}
