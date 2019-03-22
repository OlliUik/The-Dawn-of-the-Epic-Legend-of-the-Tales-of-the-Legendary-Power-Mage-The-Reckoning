using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Homing", menuName = "SpellSystem/Modifiers/Projectile/Homing")]
public class HomingModifier : SpellScriptableModifier
{

    [SerializeField] private float rotationSpeed = 2.0f;

    public override void AddSpellModifier(GameObject spellObject)
    {
        Homing component = spellObject.AddComponent<Homing>();
        component.rotationSpeed = rotationSpeed;
    }
}
