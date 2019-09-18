using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PortalGateManager
{
    
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

    // =============================================================

    List<GameObject> portalGates;
    public int maximumPortals = 2;

    List<GameObject> telepotingObjects;

    private PortalGateManager()
    {
        portalGates = new List<GameObject>();
        telepotingObjects = new List<GameObject>();
    }

    // Put instantiated Portal Gate in here to check for older portals
    public void CreatePortalGate(GameObject portalGate)
    {
        if(CreatedPortalsAmount() >= maximumPortals)
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
                toTeleport.transform.position = GetRandomPortal(steppedPortalGate).transform.position;
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
