using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropRandomizer : MonoBehaviour
{
    //Prefabs of props goes here
    public List<GameObject> propPrefabs = new List<GameObject>();

    //List of empty game objects where props can spawn
    public List<Transform> spawnLocations = new List<Transform>();

    //List of random number order of spawn location count
    List<int> randomOrder = new List<int>();

    Transform newPosition;

    void Start()
    {
        for (int n = 0; n < spawnLocations.Count; n++)
        {
            randomOrder.Add(n);
        }
    }

    void Update()
    {
        SpawnProps();
    }

    void SpawnProps()
    {
        for (int r = 0; r < randomOrder.Count; r++)
        {
            int index = Random.Range(0, randomOrder.Count);
            int i = randomOrder[index];

            //Position of props randomly picked from list of spawn locations
            newPosition = spawnLocations[randomOrder[index]];

            //Spawn prop
            GameObject newProp = Instantiate(propPrefabs[Random.Range(0, propPrefabs.Count)], newPosition) as GameObject;

            randomOrder.RemoveAt(index);
        }
    }
}