using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MultiCastProjectile", menuName = "SpellSystem/Modifiers/Projectile/MultiCastProjectile")]
public class MultiCastProjectile : SpellScriptableModifier
{

    [SerializeField] private int copyCounts = 2;
    [SerializeField] private Vector2 leftRightRotation = new Vector2(-15f, 15f);
    [SerializeField] private Vector2 upDownRotation = new Vector2(-15f, 5f);

    public override void AddSpellModifier(GameObject spellObject)
    {
        MultiProjectile component = spellObject.AddComponent<MultiProjectile>();
        component.projectileCount = copyCounts;
        component.leftRightRotation = leftRightRotation;
        component.upDownRotation = upDownRotation;
    }
}
