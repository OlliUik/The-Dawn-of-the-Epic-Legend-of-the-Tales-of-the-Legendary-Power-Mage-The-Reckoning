using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Aoe Radius ", menuName = "SpellSystem/Modifiers/Aoe/Radius")]
public class AoeRadiusModifier : SpellScriptableModifier
{

    [SerializeField] private float extraRange = 0f;

    public override void AddSpellModifier(GameObject spellObject)
    {
        Aoe aoe = spellObject.GetComponent<Aoe>();
        aoe.ModifyRange(extraRange);
    }

}
