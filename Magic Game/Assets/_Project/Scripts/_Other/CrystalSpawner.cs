using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSpawner : MonoBehaviour
{
    //Prefab of crystals goes here
    [SerializeField]
    private GameObject crystalPrefab;

    //List of empty game objects where crystal can spawn
    [SerializeField]
    private Vector3[] spawns;

    [SerializeField, Range(0, 1)]
    private float spawnPercent;
    
    //List of random number order of spawn location count
    List<int> randomOrder = new List<int>();

    Vector3 newPosition;

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
                newPosition = spawns[randomOrder[index]];

                //Add spawner's position to Vector3
                newPosition = newPosition + gameObject.transform.position;

                //Spawn crystal
                GameObject newCrystal = Instantiate(crystalPrefab, newPosition, Quaternion.identity) as GameObject;

                //Make spawner as crystal's parent
                newCrystal.transform.parent = gameObject.transform;
            }

            randomOrder.RemoveAt(index);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        foreach (Vector3 spawn in spawns)
        {
            Vector3 newGizmoPosition = transform.position + spawn;

            Gizmos.DrawWireCube(newGizmoPosition, new Vector3(1, 1, 1));
        }
    }
}