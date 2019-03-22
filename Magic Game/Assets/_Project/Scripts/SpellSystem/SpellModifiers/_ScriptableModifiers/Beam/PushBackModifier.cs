using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PushBack", menuName = "SpellSystem/Modifiers/Beam/PushBack")]
public class PushBackModifier : SpellScriptableModifier
{

    [SerializeField] private float pushBackForce = 0f;

    public override void AddSpellModifier(GameObject spellObject)
    {
        PushBack component = spellObject.AddComponent<PushBack>();
        component.pushbackForce = pushBackForce;
    }
}
