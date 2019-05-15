using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    //List of doors to spawn
    [SerializeField]
    private List<GameObject> doors = new List<GameObject>();

    //List of spawned doors
    private List<GameObject> spawnedDoors = new List<GameObject>();

    //Percent for opening a door
    [SerializeField, Range(0, 1)]
    private float spawnPercent;

    void Start()
    {
        foreach (GameObject door in doors)
        {
            if (Random.value < spawnPercent)
            {
                //Spawn door
                GameObject newDoor = Instantiate(door, gameObject.transform) as GameObject;
                spawnedDoors.Add(newDoor);
            }
        }

        if (spawnedDoors.Count >= doors.Count)
        {
            //Destroy one door in the list if there is no open doorways
            Destroy(spawnedDoors[Random.Range(0, spawnedDoors.Count)]);
        }
    }
}