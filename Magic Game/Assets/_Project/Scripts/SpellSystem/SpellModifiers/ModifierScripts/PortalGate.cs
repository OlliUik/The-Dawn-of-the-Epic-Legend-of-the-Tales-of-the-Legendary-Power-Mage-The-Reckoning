using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGate : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerCore>() || other.gameObject.GetComponent<EnemyCore>())
        {
            PortalGateManager.Instance.StepIn(other.gameObject, gameObject);
        }
    }

    /*
    private void OnTriggerExit(Collider other)
    {
        PortalGateManager.Instance.StepOut(other.gameObject);
    }
    */

}
