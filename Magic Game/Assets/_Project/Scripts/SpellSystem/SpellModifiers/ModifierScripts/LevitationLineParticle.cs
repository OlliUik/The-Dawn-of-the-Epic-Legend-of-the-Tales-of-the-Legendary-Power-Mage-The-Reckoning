using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevitationLineParticle : MonoBehaviour
{

    public GameObject caster;

    private void Update()
    {
        if (caster != null && LevitationObject.holdingObject != null)
        {
            transform.position = caster.GetComponent<Spellbook>().transform.position;
            transform.LookAt(LevitationObject.holdingObject.transform);
            transform.GetChild(0).localScale = new Vector3(Vector3.Distance(transform.position, LevitationObject.holdingObject.transform.position) / 10, 1, 1);
            LevitationObject.lineParticle = gameObject;
        }
    }

}
