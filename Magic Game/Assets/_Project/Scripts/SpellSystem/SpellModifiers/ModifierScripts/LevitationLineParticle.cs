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
            LevitationObject.lineParticle = gameObject;
        }
    }

}
