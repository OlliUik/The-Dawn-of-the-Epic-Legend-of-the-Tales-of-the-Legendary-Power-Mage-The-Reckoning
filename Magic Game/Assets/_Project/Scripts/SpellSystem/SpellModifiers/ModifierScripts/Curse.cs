using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curse : SpellModifier
{

    public float duration = 0f;
    public float damageIncreasedPercentage = 0f;
    public float beamAndAoeMiniCooldown = 2f;

    public GameObject curseGraphics;

    // base collision modifiers
    public override void ProjectileCollide(Collision collision, Vector3 direction) {
        AddCurse(collision.gameObject, SpellType.PROJECTILE);
    }

    public override void BeamCollide(RaycastHit hitInfo, Vector3 direction, float distance) {
        AddCurse(hitInfo.collider.gameObject, SpellType.BEAM);
    }

    public override void AoeCollide(GameObject hitObject) {
        AddCurse(hitObject, SpellType.AOE);
    }

    private void AddCurse(GameObject target, SpellType spellType)
    {
        CurseVariables targetCurse = target.GetComponent<CurseVariables>();
        if (targetCurse != null)
        {
            targetCurse.DamageIncreasedPercentage += damageIncreasedPercentage;
            targetCurse.duration = duration;
            targetCurse.spellType = spellType;
            targetCurse.beamAndAoeMiniCoolDown = beamAndAoeMiniCooldown;
        }
        else
        {
            CurseVariables newTargetCurse = target.AddComponent<CurseVariables>();
            newTargetCurse.SetVariables(duration, spellType, damageIncreasedPercentage);
            newTargetCurse.beamAndAoeMiniCoolDown = beamAndAoeMiniCooldown;
            if(curseGraphics != null) {
                GameObject instantiatedCurseGraphics = Instantiate(curseGraphics, target.transform.position, target.transform.rotation);
                instantiatedCurseGraphics.transform.SetParent(target.transform);
                instantiatedCurseGraphics.transform.localPosition = new Vector3(0, 0, 0);
                newTargetCurse.graphics = instantiatedCurseGraphics;
            }
        }
    }

}
