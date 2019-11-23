//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TriggerActivateEnemyAI : MonoBehaviour
{
    [SerializeField] private bool disableAfterTrigger = true;
    [SerializeField] private EnemyCore enemy = null;
    //[SerializeField] private EnemyCore.EState stateAfterTrigger = EnemyCore.EState.ATTACK;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (enemy != null)
            {
                //enemy.currentState = stateAfterTrigger;
                enemy.cVision.targetLocation = other.transform.position;
                if (disableAfterTrigger)
                {
                    GetComponent<BoxCollider>().enabled = false;
                }
            }
            else
            {
                Debug.LogWarning("Trigger " + this.gameObject + " was set, but it has no enemy attached to it!");
            }
        }
    }
}
