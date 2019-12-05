using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [SerializeField] private GameObject boss = null;
    [SerializeField] private GameObject particles = null;
    [SerializeField] private GameObject portal = null;
    [SerializeField] private GameObject elevator = null;
    [SerializeField] private Transform elevatorTarget = null;
    [SerializeField] private BoxCollider[] col = null;

    private void Start()
    {
        col = GetComponents<BoxCollider>();
        particles.transform.position = boss.transform.position + (Vector3.up * 2);
        particles.gameObject.SetActive(false);
        boss = FindObjectOfType<BossLizardKing>();
        boss.gameObject.SetActive(false);
        portal.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (boss == null)
        {
            portal.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (boss != null)
            {
                boss.gameObject.SetActive(true);
                particles.gameObject.SetActive(true);
                elevator.transform.position = elevatorTarget.position;
                elevator.GetComponent<MovingObject>().enabled = false;

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
