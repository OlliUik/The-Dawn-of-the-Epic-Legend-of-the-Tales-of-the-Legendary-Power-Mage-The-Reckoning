using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameObjectSpawner : MonoBehaviour
{
    //List of objects to spawn
    [SerializeField]
    private List<GameObject> objects = new List<GameObject>();

    //List of spawned objects
    private List<GameObject> spawnedObjects = new List<GameObject>();

    //Percent for spawning
    [SerializeField, Range(0, 1)]
    private float spawnPercent;

    //Destroy one random game object
    [SerializeField]
    private bool isDestroyingRandom;

    void Start()
    {
        foreach (GameObject obj in objects)
        {
            if (Random.value < spawnPercent)
            {
                obj.SetActive(true);
                spawnedObjects.Add(obj);

            }

            else
            {
                Destroy(obj);
            }
        }
        
        if (isDestroyingRandom)
        {
            Destroy(spawnedObjects[Random.Range(0, spawnedObjects.Count)]);
        }
    }
}