using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Aoe Duration ", menuName = "SpellSystem/Modifiers/Aoe/Duration")]
public class AoeDurationModifier : SpellScriptableModifier
{

    [SerializeField] private float extraDuration = 0f;

    public override void AddSpellModifier(GameObject spellObject)
    {
        Aoe aoe = spellObject.GetComponent<Aoe>();
        aoe.ModifyDuration(extraDuration);
    }

}
