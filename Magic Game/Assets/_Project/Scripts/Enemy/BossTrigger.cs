using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] private EnemyCore boss = null;
    [SerializeField] private GameObject particles = null;
    [SerializeField] private BoxCollider[] col = null;

    private void Start()
    {
        col = GetComponents<BoxCollider>();
        boss.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (boss != null)
            {
                boss.gameObject.SetActive(true);
                Instantiate(particles, boss.transform);

                foreach (BoxCollider box in col)
                {
                    Destroy(box);
                }
            }

            else
            {
                Debug.LogWarning("Boss trigger " + gameObject + " has no boss attached to it!");
            }
        }
    }
}