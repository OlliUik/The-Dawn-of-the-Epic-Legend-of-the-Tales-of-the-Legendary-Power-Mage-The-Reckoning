using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorManager
{
    public float explosionForce = 20f;
    public float currentScale = 1f;

    #region Singleton

    //Singleton things
    private static readonly object padlock = new object();
    private static MeteorManager instance = null;

    public static MeteorManager Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new MeteorManager();
                }
                return instance;
            }
        }
    }

    private MeteorManager()
    {
        Init();
    }

    public void Init()
    {
        explosionForce = 20f;
        currentScale = 1f;
    }

    public static void ResetVariables()
    {
        if (instance != null)
        {
            instance.Init();
        }
    }

    #endregion

}
