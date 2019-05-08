using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Electricity", menuName = "SpellSystem/StatusEffects/Electric")]
public class ElectricModifier : SpellScriptableModifier
{

    [SerializeField] private float duration = 10f;
    [SerializeField] private GameObject graphics;


    public override void AddSpellModifier(GameObject spellObject)
    {
        Spell spell = spellObject.GetComponent<Spell>();

        // check if electric already exist if so --> modify values only
        

        // Apply as new
        spell.statusEffects.Add(new ElectricEffect(duration, graphics));
    }
}
