using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGate : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(CountToDestroy(PortalGateManager.Instance.SpellDuration));
    }

    IEnumerator CountToDestroy(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        PortalGateManager.Instance.RemoveOldestPortal();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerCore>() || other.gameObject.GetComponent<EnemyCore>())
        {
            Instantiate(PortalGateManager.Instance.portalActiveParticle, other.transform.position, other.transform.rotation);
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
