using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Homing", menuName = "SpellSystem/Modifiers/Projectile/Homing")]
public class HomingModifier : SpellScriptableModifier
{

    [SerializeField] private float maxAngle = 20f;
    [SerializeField] private float rotationSpeed = 2.0f;

    public override void AddSpellModifier(Spell spell)
    {
        Homing comp = spell.GetComponent<Homing>();
        if(comp != null)
        {
            comp.rotationSpeed += rotationSpeed;
        }

        Homing component = spell.gameObject.AddComponent<Homing>();
        component.rotationSpeed = rotationSpeed;
        component.maxAngle = maxAngle;
    }
}
