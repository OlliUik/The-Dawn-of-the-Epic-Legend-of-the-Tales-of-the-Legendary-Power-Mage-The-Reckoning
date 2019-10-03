using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MultiCast", menuName = "SpellSystem/Modifiers/MultiCast")]
public class MultiCastModifier : SpellScriptableModifier
{

    [SerializeField] private int copyCounts             = 2;
    [SerializeField] private Vector2 leftRightRotation  = new Vector2(-15f, 15f);
    [SerializeField] private Vector2 upDownRotation     = new Vector2(-15f, 5f);

    public override void AddSpellModifier(Spell spell)
    {
        var compo = spell.GetComponent<MultiCast>();
        if (compo != null)
        {
            compo.copyCount += 2;
            return;
        }

        MultiCast component         = spell.gameObject.AddComponent<MultiCast>();
        component.copyCount         = copyCounts;
        component.leftRightRotation = leftRightRotation;
        component.upDownRotation    = upDownRotation;
    }
}
