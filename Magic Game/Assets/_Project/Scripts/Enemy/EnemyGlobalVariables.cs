using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGlobalVariables
{

    private static readonly float healthRate = 20f;
    private static readonly int enemyIncreaseRate = 5;

    public static float enemyExtraHealth = 0f;
    public static int extraEnemyAmount = 0;
    //public static float extraEnemyRate = 0f;

    public static void StageUp()
    {
        enemyExtraHealth += healthRate;
        extraEnemyAmount += enemyIncreaseRate;
    }

}
