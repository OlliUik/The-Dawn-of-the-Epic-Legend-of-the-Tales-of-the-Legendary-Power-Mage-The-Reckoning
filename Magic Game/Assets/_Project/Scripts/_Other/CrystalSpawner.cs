using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSpawner : MonoBehaviour
{
    //Prefab of crystal goes here
    public GameObject crystalPrefab;

    //List of empty game objects where crystal can spawn
    public List<Transform> spawnLocations = new List<Transform>();

    Transform newPosition;

    void Start()
    {
        //Position of crystal randomly picked from list of spawn locations
        newPosition = spawnLocations[Random.Range(0, spawnLocations.Count)];

        //Spawn crystal
        GameObject crystalSpawn = Instantiate(crystalPrefab, newPosition) as GameObject;
    }
}
