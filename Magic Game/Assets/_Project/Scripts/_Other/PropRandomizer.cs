using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropRandomizer : MonoBehaviour
{
    //Prefabs of props goes here
    [SerializeField]
    private List<GameObject> propPrefabs = new List<GameObject>();

    //Vector3's where props can spawn
    [SerializeField]
    private Vector3[] spawns;

    //Percent for spawning prop
    [SerializeField, Range(0, 1)]
    private float spawnPercent;

    //List of angles for every spawn point
    [SerializeField, Range(-180, 180)]
    private List<float> propAngle = new List<float>();

    void Start()
    {
        SpawnProps();
    }

    void SpawnProps()
    {
        for (int r = 0; r < spawns.Length; r++)
        {
            if (Random.value < spawnPercent)
            {
                //Position of props picked from list of spawn locations
                Vector3 spawnPosition = spawns[r];

                //Add spawner's position to Vector3
                Vector3 newPosition = transform.TransformPoint(spawnPosition);

                //Add rotation from propAngle
                Quaternion newRotation = Quaternion.Euler(-90, transform.eulerAngles.y + propAngle[r], transform.eulerAngles.z); //transform.eulerAngles.x

                //Spawn prop with new position
                GameObject newProp = Instantiate(propPrefabs[Random.Range(0, propPrefabs.Count)], newPosition, transform.parent.rotation, gameObject.transform) as GameObject;

                //Add rotation to prop
                newProp.transform.localRotation = newRotation;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        int i = 0;

        foreach (Vector3 spawn in spawns)
        {
            Vector3 newPosition = transform.parent.transform.position + spawn;
            Quaternion newRotation = Quaternion.Euler(0, transform.eulerAngles.y + propAngle[i], 0);

            Matrix4x4 newMatrix = Matrix4x4.TRS(newPosition, newRotation, transform.lossyScale);

            Gizmos.matrix = newMatrix * transform.localToWorldMatrix;
            Gizmos.DrawWireCube(newPosition - spawn, Vector3.one);

            i++;
        }
    }
}