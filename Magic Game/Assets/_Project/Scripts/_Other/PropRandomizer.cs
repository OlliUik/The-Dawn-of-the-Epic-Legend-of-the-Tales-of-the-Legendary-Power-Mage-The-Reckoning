using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropRandomizer : MonoBehaviour
{
    //Prefabs of props goes here
    [SerializeField]
    private List<GameObject> propPrefabs = new List<GameObject>();

    //List of vectors where props can spawn
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
        SpawnProps();
    }

    void SpawnProps()
    {
        for (int r = 0; r < randomOrder.Count; r++)
        {
            int index = Random.Range(0, randomOrder.Count);
            int i = randomOrder[index];

            if (Random.value < spawnPercent)
            {
                //Position of props randomly picked from list of spawn locations
                newPosition = spawns[randomOrder[index]];

                //Add spawner's position to Vector3
                newPosition = newPosition + gameObject.transform.position;

                //Spawn prop
                GameObject newProp = Instantiate(propPrefabs[Random.Range(0, propPrefabs.Count)], newPosition, Quaternion.identity) as GameObject;

                //Make spawner as crystal's parent
                newProp.transform.parent = gameObject.transform;
            }

            randomOrder.RemoveAt(index);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        foreach (Vector3 spawn in spawns)
        {
            Vector3 newGizmoPosition = transform.position + spawn;

            Gizmos.DrawWireCube(newGizmoPosition, new Vector3(1, 1, 1));
        }
    }
}