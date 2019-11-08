using System.Collections.Generic;
using UnityEngine;

public static class GlobalVariables
{
    public static List<string> scenesInBuild = new List<string>();
    public static List<GameObject> teamGoodGuys = new List<GameObject>();
    public static List<GameObject> teamBadBoys = new List<GameObject>();
    public static bool bAnyPlayersAlive = true;
    public static bool usePostProcessing = true;
    public static string aaMethod = "fxaa";
    public static string loadLevel = "MainMenu";
    public static string errorCode = "0000-0000";
    public static Vector2 sensitivity = QualityManager.DATA.SENSITIVITY;
    public static int crystalsCollected = 0;
}
