using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurseVariables : MonoBehaviour
{

    public SpellType spellType = SpellType.GENERIC;
    public float duration = 10f;
    public float beamAndAoeMiniCoolDown = 2f;

    [SerializeField] private float damageIncreasedPercentage = 0f;

    private float currentBeamAndAoeMiniCoolDown = 0f;
    private bool isReadyBeamOrAoe = true;

    public GameObject graphics;

    public void SetVariables(float duration, SpellType spellType, float damageIncreasedPercentage)
    {
        this.duration = duration;
        this.spellType = spellType;
        this.damageIncreasedPercentage = damageIncreasedPercentage;
    }

    private void Update()
    {
        float tempTime = Time.deltaTime;
        if (!isReadyBeamOrAoe)
        {
            currentBeamAndAoeMiniCoolDown += tempTime;
            if (currentBeamAndAoeMiniCoolDown >= beamAndAoeMiniCoolDown)
            {
                isReadyBeamOrAoe = true;
            }
        }
        duration -= tempTime;
        if (duration <= 0)
        {
            if(graphics != null)
            {
                Destroy(graphics);
            }
            Destroy(this);
        }
    }

    public float DamageIncreasedPercentage
    {
        get
        {
            return damageIncreasedPercentage;
        }
        set
        {
            if (spellType == SpellType.PROJECTILE || spellType == SpellType.GENERIC)
            {
                damageIncreasedPercentage = value;
                Debug.Log("Peojectile cursed: " + damageIncreasedPercentage);
            }
            else
            {
                if (isReadyBeamOrAoe)
                {
                    damageIncreasedPercentage = value;
                    currentBeamAndAoeMiniCoolDown = 0f;
                    isReadyBeamOrAoe = false;
                    Debug.Log("Beam or AoE cursed: " + damageIncreasedPercentage);
                }
            }
        }
    }

}
