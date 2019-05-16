using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectSpawner : MonoBehaviour
{
    //List of objects to spawn
    [SerializeField]
    private List<GameObject> objects = new List<GameObject>();

    //List of spawned objects
    private List<GameObject> spawnedObjects = new List<GameObject>();

    //Percent for opening a door
    [SerializeField, Range(0, 1)]
    private float spawnPercent;

    //Destroy one random game object
    [SerializeField]
    private bool isDestroyingOneRandom;

    void Start()
    {
        foreach (GameObject obj in objects)
        {
            if (Random.value < spawnPercent)
            {
                //Spawn object
                GameObject newObject = Instantiate(obj, gameObject.transform) as GameObject;
                spawnedObjects.Add(newObject);
            }
        }

        if (isDestroyingOneRandom)
        {
            //Destroy one game object in the list
            Destroy(spawnedObjects[Random.Range(0, spawnedObjects.Count)]);
        }
    }
}