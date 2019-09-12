using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Direction", menuName = "SpellSystem/Modifiers/Projectile/Direction")]
public class DirectionModifier : SpellScriptableModifier
{

    [SerializeField] private float rotationSpeed = 5f;

    public override void AddSpellModifier(Spell spell)
    {
        var direction = spell.GetComponent<Direction>();
        if(direction != null)
        {
            rotationSpeed += this.rotationSpeed;
            return;
        }

        Direction dir = spell.gameObject.AddComponent<Direction>();
        dir.rotationSpeed = rotationSpeed;
    }

}
