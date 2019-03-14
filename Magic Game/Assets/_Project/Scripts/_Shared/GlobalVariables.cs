using System.Collections.Generic;
using UnityEngine;

public static class GlobalVariables
{
    public static PlayerCore player;
    public static List<string> scenesInBuild = new List<string>();
    public static List<GameObject> entityList = new List<GameObject>();
    public static string loadLevel = "MainMenu";
    public static string errorCode = "0000-0000";
}
