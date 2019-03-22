using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MultiProjectile", menuName = "SpellSystem/Modifiers/Projectile/MultiProjectile")]
public class MultiProjectileModifier : SpellScriptableModifier
{

    [SerializeField] private int projectileCount = 2;
    [SerializeField] private Vector2 leftRightRotation = new Vector2(-15f, 15f);
    [SerializeField] private Vector2 upDownRotation = new Vector2(-15f, 5f);

    public override void AddSpellModifier(GameObject spellObject)
    {
        MultiProjectile component = spellObject.AddComponent<MultiProjectile>();
        component.projectileCount = projectileCount;
        component.leftRightRotation = leftRightRotation;
        component.upDownRotation = upDownRotation;
    }
}
