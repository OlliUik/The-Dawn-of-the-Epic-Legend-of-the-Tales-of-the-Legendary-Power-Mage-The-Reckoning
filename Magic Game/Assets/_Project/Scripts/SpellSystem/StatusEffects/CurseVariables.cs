using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurseVariables : MonoBehaviour
{

    public bool createdFromBeamOrAoe = true;
    private bool isReadyBeamOrAoe = true;
    public float beamAndAoeMiniCoolDown = 2f;

    private float damageIncreasedPercentage = 0f;

    public float DamageIncreasedPercentage
    {
        get
        {
            return damageIncreasedPercentage;
        }
        set
        {
            if (!createdFromBeamOrAoe)
            {
                damageIncreasedPercentage = value;
            }
            else
            {
                if (isReadyBeamOrAoe)
                {
                    damageIncreasedPercentage = value;
                    StartCoroutine(beamAndAoeMiniCoolDownWait());
                }
            }
        }
    }

    IEnumerator beamAndAoeMiniCoolDownWait()
    {
        isReadyBeamOrAoe = false;
        yield return new WaitForSeconds(beamAndAoeMiniCoolDown);
        isReadyBeamOrAoe = true;
    }

}
