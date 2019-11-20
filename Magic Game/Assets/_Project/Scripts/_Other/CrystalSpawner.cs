using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSpawner : MonoBehaviour
{
    [SerializeField] private GameObject crystalPrefab = null;
    [SerializeField] private Vector3[] spawns = null;
    [SerializeField, Range(0, 1)] private float spawnPercent = 0f;

    private List<int> randomOrder = new List<int>();
    
    private void Start()
    {
        for (int n = 0; n < spawns.Length; n++)
        {
            randomOrder.Add(n);
        }
    }

    private void Update()
    {
        SpawnCrystals();
    }

    private void SpawnCrystals()
    {
        for (int r = 0; r < randomOrder.Count; r++)
        {
            int index = Random.Range(0, randomOrder.Count);
            int i = randomOrder[index];

            if (Random.value < spawnPercent)
            {
                Vector3 spawnPosition = spawns[randomOrder[index]]; //Position of crystal randomly picked from list of spawn locations
                Vector3 newPosition = gameObject.transform.TransformPoint(spawnPosition); //Add spawner's position to Vector3

                GameObject newCrystal = Instantiate(crystalPrefab, newPosition, Quaternion.identity, gameObject.transform) as GameObject;
            }

            randomOrder.RemoveAt(index);
        }
    }

    private void OnDrawGizmos()
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