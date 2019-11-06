using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour  
{
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private float frequency;
    private float defaultFrequency = 1f;
    protected float debugDrawRadius = 1.0F;


    void Start()
    {
        if (frequency < defaultFrequency)
        {
            Debug.LogWarning("Your inputted frequency value is too low. Now using default value which is 1.");
        }
        StartCoroutine(SpawnEnemy());
    }

    public virtual void OnDrawGizmos()
    {


        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, debugDrawRadius);

    }

    IEnumerator SpawnEnemy()
    {
        while(true)
        {
            int range = enemies.Count;
            GameObject enemy = enemies[Random.Range(0, range)];

            if (enemy.gameObject.tag.Equals("Enemy"))
            {
                if (range > 0)
                {
                    Instantiate(enemy, GetComponent<Transform>());
                    enemy.SetActive(true);
                }
            } else
            {
                Debug.LogError("You have input some non-enemy object into the list.");
            }

            if (frequency < defaultFrequency)
            {
                yield return new WaitForSeconds(defaultFrequency);
            } else
            {
                yield return new WaitForSeconds(frequency);
            }
        }
    }
}
