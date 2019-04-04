using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SummonLava", menuName = "SpellSystem/Modifiers/SummonLava")]
public class SummonLavaModifier : SpellScriptableModifier
{

    [SerializeField] private GameObject lavaPrefab = null;
    [SerializeField] private GameObject aoeLavaFountain = null;

    public override void AddSpellModifier(GameObject spellObject)
    {
        SummonLava component = spellObject.AddComponent<SummonLava>();
        component.spawnPrefab = lavaPrefab;
        component.aoeLavaFountain = aoeLavaFountain;
    }
}
