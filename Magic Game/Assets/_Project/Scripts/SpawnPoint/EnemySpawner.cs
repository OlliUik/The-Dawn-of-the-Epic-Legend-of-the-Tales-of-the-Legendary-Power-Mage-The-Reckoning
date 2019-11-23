using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemies = null;
    [SerializeField] private float frequency = 1.0f;
    private float defaultFrequency = 1.0f;

    void Start()
    {
        // Check if the inputted frequency value is lower than one or not, if so use the default value which is one instead.
        if (frequency < defaultFrequency)
        {
            Debug.LogWarning("Your inputted frequency value is too low. Now using default value which is 1.");
        }

        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            int range = enemies.Count;
            GameObject enemy = enemies[Random.Range(0, range)];

            if (enemy.gameObject.tag.Equals("Enemy"))
            {
                if (range > 0)
                {
                    Instantiate(enemy, GetComponent<Transform>());
                }
            }
            else
            {
                Debug.LogWarning("You have inputted some non-enemy object into the list which will not be spawn.");
            }

            if (frequency < defaultFrequency)
            {
                yield return new WaitForSeconds(defaultFrequency);
            }
            else
            {
                yield return new WaitForSeconds(frequency);
            }
        }
    }
}
