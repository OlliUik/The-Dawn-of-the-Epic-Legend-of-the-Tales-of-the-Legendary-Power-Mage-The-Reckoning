using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PropRandomizer : MonoBehaviour
{
    [SerializeField] private List<GameObject> propPrefabs = new List<GameObject>();
    [SerializeField] private Vector3[] spawns = null;
    [SerializeField, Range(0, 1)] private float spawnPercent = 0.0f;
    [SerializeField, Range(-180, 180)] private List<float> propAngle = new List<float>();
    public SetActiveRoom surface;

    private void Start()
    {   
        SpawnProps();
        surface.genNavMesh();
    }

    private void SpawnProps()
    {
        for (int r = 0; r < spawns.Length; r++)
        {
            if (Random.value < spawnPercent)
            {
                Vector3 spawnPosition = spawns[r]; //Position of props picked from list of spawn locations
                Vector3 newPosition = transform.TransformPoint(spawnPosition); //Add spawner's position to Vector3

                Quaternion newRotation = Quaternion.Euler(-90, transform.eulerAngles.y + propAngle[r], transform.eulerAngles.z); //transform.eulerAngles.x //Add rotation from propAngle
                GameObject newProp = Instantiate(propPrefabs[Random.Range(0, propPrefabs.Count)], newPosition, transform.parent.rotation, gameObject.transform) as GameObject; //Spawn prop with new position

                newProp.transform.localRotation = newRotation; //Add rotation to prop
            }
        }

    }

    private void OnDrawGizmos()
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