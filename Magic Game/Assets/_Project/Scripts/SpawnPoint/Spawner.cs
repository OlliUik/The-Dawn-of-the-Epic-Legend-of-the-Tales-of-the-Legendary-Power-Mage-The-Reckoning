using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour  
{
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private float frequency;
    
    void Start()
    {
        StartCoroutine(SpawnEnememy());
    }

    IEnumerator SpawnEnememy()
    {
        while(true)
        {
            int range = enemies.Count;
            
            if (range > 0)
            {
                GameObject enemy = enemies[Random.Range(0, range)];
                Instantiate(enemy, GetComponent<Transform>());
            }

            yield return new WaitForSeconds(frequency);
        }
    }
}
