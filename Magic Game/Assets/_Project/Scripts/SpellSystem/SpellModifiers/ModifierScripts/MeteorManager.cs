using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorManager
{

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
        currentExplosionForce = explosionForce;
        currentMeteorScale = meteorScale;
    }

    public static void ResetVariables()
    {
        if (instance != null)
        {
            instance.Init();
        }
    }

    #endregion

    public float explosionForce = 20f;
    float currentExplosionForce = 20f;
    public float meteorScale = 1;
    float currentMeteorScale = 1;

    public float GetExplosionForce()
    {
        return currentExplosionForce;
    }

    public float GetMeteorScale()
    {
        return currentMeteorScale;
    }

}
