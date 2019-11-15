using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BossTrigger : MonoBehaviour
{
    [SerializeField] private EnemyCore boss = null;
    //[SerializeField] private GameObject particles = null;

    private void Start()
    {
        boss.gameObject.SetActive(false);
        //particles.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (boss != null)
            {
                boss.gameObject.SetActive(true);
                //particles.gameObject.SetActive(true);
                Destroy(gameObject);
            }

            else
            {
                Debug.LogWarning("Boss trigger " + gameObject + " has no boss attached to it!");
            }
        }
    }
}
