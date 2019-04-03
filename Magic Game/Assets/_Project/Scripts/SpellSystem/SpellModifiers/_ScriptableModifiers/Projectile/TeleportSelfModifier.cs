using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TeleportSelf", menuName = "SpellSystem/Modifiers/Projectile/TeleportSelf")]
public class TeleportSelfModifier : SpellScriptableModifier
{
    public override void AddSpellModifier(GameObject spellObject)
    {
        spellObject.AddComponent<TeleportSelf>();
    }
}
