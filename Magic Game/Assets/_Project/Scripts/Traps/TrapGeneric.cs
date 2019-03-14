using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapGeneric : MonoBehaviour
{
    //works for falling bookcases/pillars, wall of death-traps, spike traps and trap doors
    //goes to the trigger/pivot object
    
    [SerializeField]
    private Animator anim;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private Vector3 direction;

    void OnTriggerEnter(Collider other)
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
                rb.AddForceAtPosition(direction * 200f, rb.position);
                Destroy(gameObject);
            }

            if (gameObject.tag == "WallOfDeath")
            {
                anim.Play("Kill");
                Destroy(gameObject);
            }

            if (gameObject.tag == "WallOfDeathReset")
            {
                anim.Play("Kill");
            }

            if (gameObject.tag == "SpikeTrap")
            {
                anim.Play("Kill");
            }

            if (gameObject.tag == "TrapDoor")
            {
                anim.Play("Kill");
            }
        }
    }
}