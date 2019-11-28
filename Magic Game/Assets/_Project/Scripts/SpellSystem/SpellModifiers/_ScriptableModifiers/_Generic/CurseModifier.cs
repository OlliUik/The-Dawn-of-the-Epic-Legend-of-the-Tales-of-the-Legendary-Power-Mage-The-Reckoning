using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Curse", menuName = "SpellSystem/Modifiers/Curse")]
public class CurseModifier : SpellScriptableModifier
{

    [SerializeField] private float baseDuration = 3f;
    [SerializeField] private float duration = 2f;
    [SerializeField] private float baseDamageIncreasingPercentage = 5f;
    [SerializeField] private float damageIncreasingPercentage = 3f;
    [SerializeField] private float beamAndAoeMiniCooldown = 2f;

    [SerializeField] private GameObject curseGraphic;

    public override void AddSpellModifier(Spell spell)
    {
        var compo = spell.GetComponent<Curse>();
        if (compo != null)
        {
            compo.duration += duration;
            compo.damageIncreasedPercentage += damageIncreasingPercentage;
            compo.beamAndAoeMiniCooldown = beamAndAoeMiniCooldown;
            return;
        }

        Curse component = spell.gameObject.AddComponent<Curse>();
        component.projectileElementGraphic = projectileGraphics;
        component.aoeElementGraphic = aoeGraphics;
        component.beamElementGraphic = beamGraphics;
        component.projectileExplosionGraphic = projectileExploionGraphics;

        component.duration = baseDuration;
        component.damageIncreasedPercentage = baseDamageIncreasingPercentage;
        component.beamAndAoeMiniCooldown = beamAndAoeMiniCooldown;
        component.curseGraphics = curseGraphic;

    }
}
