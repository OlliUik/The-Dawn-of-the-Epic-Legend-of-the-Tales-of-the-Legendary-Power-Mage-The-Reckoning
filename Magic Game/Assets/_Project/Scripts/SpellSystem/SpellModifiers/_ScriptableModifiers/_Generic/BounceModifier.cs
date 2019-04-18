using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bounce", menuName = "SpellSystem/Modifiers/Bounce")]
public class BounceModifier : SpellScriptableModifier
{

    [SerializeField] private int bounceCount = 0;

    public override void AddSpellModifier(GameObject spellObject)
    {
        var compo = spellObject.GetComponent<Bounce>();
        if(compo != null)
        {
            compo.bounceCount += 2;
            return;
        }

        Bounce component = spellObject.AddComponent<Bounce>();
        component.bounceCount = bounceCount;
    }
}
