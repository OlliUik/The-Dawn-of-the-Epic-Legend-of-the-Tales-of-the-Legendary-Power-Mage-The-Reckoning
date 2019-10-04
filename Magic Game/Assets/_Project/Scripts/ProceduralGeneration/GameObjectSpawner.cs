using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectSpawner : MonoBehaviour
{
    #region VARIABLES

    [SerializeField] private List<GameObject> objects = new List<GameObject>();

    [SerializeField, Range(0, 1)] private float spawnPercent = 0.0f;
    [SerializeField] private bool isDestroyingRandom = false;

    private List<GameObject> spawnedObjects = new List<GameObject>();

    #endregion

    #region UNITY_FUNCTIONS

    private void Start()
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

    #endregion
}