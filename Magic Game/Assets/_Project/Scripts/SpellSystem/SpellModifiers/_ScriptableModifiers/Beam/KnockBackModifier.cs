using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KnockBack", menuName = "SpellSystem/Modifiers/Beam/KnockBack")]
public class KnockBackModifier : SpellScriptableModifier
{

    [SerializeField] private float knockbackForce = 0f;

    public override void AddSpellModifier(GameObject spellObject)
    {
        KnockBack component = spellObject.AddComponent<KnockBack>();
        component.knockbackForce = knockbackForce;
    }
}
