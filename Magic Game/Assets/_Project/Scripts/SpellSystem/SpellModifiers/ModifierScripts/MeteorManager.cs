using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorManager
{
    
    public float damageAmount = 20;
    public float miniAoeRadius = 3f;
    public float projectileForce = 10f;
    public float explosionForce = 20f;

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

    // =============================================================

    #endregion

}
