using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapGeneric : MonoBehaviour
{
    //works for falling bookcases/pillars, wall of death-traps, spike traps and trap doors
    //goes to the trigger/pivot object
    
    [SerializeField] private Animator anim = null;
    [SerializeField] private Rigidbody rb = null;
    [SerializeField] private GameObject damagePart = null;
    [SerializeField] private Vector3 direction = Vector3.zero;

    private void Start()
    {
        if (damagePart)
        {
            damagePart.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (gameObject.tag == "Crushing")
            {
                rb.isKinematic = false;
                Destroy(gameObject);
            }

            if (gameObject.tag == "Falling")
            {
                damagePart.SetActive(true);
                rb.AddForceAtPosition(direction * 200f, rb.position);
                Destroy(gameObject);
            }

            if (gameObject.tag == "WallOfDeath")
            {
                anim.Play("Kill");
                Destroy(gameObject);
            }

            if (gameObject.tag == "WallOfDeathReset" || gameObject.tag == "SpikeTrap" || gameObject.tag == "TrapDoor")
            {
                anim.Play("Kill");
            }
        }
    }
}