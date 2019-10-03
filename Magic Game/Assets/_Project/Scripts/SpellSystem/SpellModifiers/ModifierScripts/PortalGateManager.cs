using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PortalGateManager
{

    #region Singleton

    //Singleton things
    private static readonly object padlock = new object();
    private static PortalGateManager instance = null;

    public static PortalGateManager Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new PortalGateManager();
                }
                return instance;
            }
        }
    }

    private PortalGateManager()
    {
        Init();
    }

    public void Init()
    {
        portalGates = new List<GameObject>();
        telepotingObjects = new List<GameObject>();
        usingMaximumPortals = maximumPortals;
    }

    public static void ResetVariables()
    {
        if(instance != null)
        {
            instance.Init();
        }
    }

    #endregion

    List<GameObject> portalGates;
    public int maximumPortals = 2;
    int usingMaximumPortals;

    List<GameObject> telepotingObjects;
    public GameObject portalActiveParticle;

    // Put instantiated Portal Gate in here to check for older portals
    public void CreatePortalGate(GameObject portalGate)
    {
        if(CreatedPortalsAmount() >= usingMaximumPortals)
        {
            GameObject.Destroy(portalGates[0]);
            portalGates.RemoveAt(0);
        }
        portalGates.Add(portalGate);
    }

    public GameObject GetRandomPortal()
    {
        return portalGates[Random.Range(0, portalGates.Count)];
    }

    // Random portal gate which is not the same as before
    public GameObject GetRandomPortal(GameObject steppedPortal)
    {
        GameObject temp;
        do
        {
            temp = GetRandomPortal();
        } while (GameObject.ReferenceEquals(steppedPortal, temp));
        return temp;
    }

    public void StepIn(GameObject toTeleport, GameObject steppedPortalGate)
    {
        if (telepotingObjects.IndexOf(toTeleport) == -1) {
            telepotingObjects.Add(toTeleport);
            if (CreatedPortalsAmount() > 1)
            {
                CharacterController characterController = toTeleport.GetComponent<CharacterController>();
                Debug.Log(characterController);
                if (characterController != null)
                {
                    characterController.enabled = false;
                    characterController.transform.position = GetRandomPortal(steppedPortalGate).transform.position;
                    characterController.enabled = true;
                }
                else
                {
                    toTeleport.transform.position = GetRandomPortal(steppedPortalGate).transform.position;
                }
            }
        }
        else
        {
            StepOut(toTeleport);
        }
    }

    public void StepOut(GameObject teleported)
    {
        telepotingObjects.Remove(teleported);
    }

    public int CreatedPortalsAmount()
    {
        return portalGates.Count;
    }

}
