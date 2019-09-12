using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSpawner : MonoBehaviour
{
    //Prefab of crystals goes here
    [SerializeField]
    private GameObject crystalPrefab;

    //Vector3's where crystal can spawn
    [SerializeField]
    private Vector3[] spawns;

    [SerializeField, Range(0, 1)]
    private float spawnPercent;
    
    //List of random number order of spawn location count
    List<int> randomOrder = new List<int>();
    
    void Start()
    {
        for (int n = 0; n < spawns.Length; n++)
        {
            randomOrder.Add(n);
        }
    }

    void Update()
    {
        SpawnCrystals();
    }

    void SpawnCrystals()
    {
        for (int r = 0; r < randomOrder.Count; r++)
        {
            int index = Random.Range(0, randomOrder.Count);
            int i = randomOrder[index];

            if (Random.value < spawnPercent)
            {
                //Position of crystal randomly picked from list of spawn locations
                Vector3 spawnPosition = spawns[randomOrder[index]];

                //Add spawner's position to Vector3
                Vector3 newPosition = gameObject.transform.TransformPoint(spawnPosition);

                //Spawn crystal
                GameObject newCrystal = Instantiate(crystalPrefab, newPosition, Quaternion.identity, gameObject.transform) as GameObject;
            }

            randomOrder.RemoveAt(index);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        foreach (Vector3 spawn in spawns)
        {
            Gizmos.matrix = transform.localToWorldMatrix;

            Vector3 newGizmoPosition = Vector3.zero + spawn;

            Gizmos.DrawWireCube(newGizmoPosition, Vector3.one);
        }
    }
}