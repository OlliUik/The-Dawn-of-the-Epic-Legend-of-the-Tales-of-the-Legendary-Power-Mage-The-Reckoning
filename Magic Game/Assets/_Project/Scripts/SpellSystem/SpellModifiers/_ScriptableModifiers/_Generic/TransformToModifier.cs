using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TransformTo", menuName = "SpellSystem/Modifiers/TransformTo")]
public class TransformToModifier : SpellScriptableModifier
{

    [SerializeField] private List<GameObject> transformToPrefabs = new List<GameObject>();
    [SerializeField] private float duration = 5f;
    [SerializeField] private GameObject transformationParticles = null;

    public override void AddSpellModifier(Spell spell)
    {
        var comp = spell.GetComponent<TransformTo>();
        if(comp != null)
        {
            comp.duration += this.duration;
            return;
        }

        var compo = spell.gameObject.AddComponent<TransformTo>();
        compo.transformPrefab = transformToPrefabs[Random.Range(0, transformToPrefabs.Count)];
        compo.duration = duration;
        compo.transformationParticles = transformationParticles;
    }
}
