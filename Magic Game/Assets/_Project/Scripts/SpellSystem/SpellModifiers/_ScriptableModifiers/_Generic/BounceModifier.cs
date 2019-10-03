using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bounce", menuName = "SpellSystem/Modifiers/Bounce")]
public class BounceModifier : SpellScriptableModifier
{

    [SerializeField] private int bounceCount = 0;

    public override void AddSpellModifier(Spell spell)
    {
        var compo = spell.GetComponent<Bounce>();
        if(compo != null)
        {
            compo.bounceCount += 2;
            return;
        }

        Bounce component = spell.gameObject.AddComponent<Bounce>();
        component.bounceCount = bounceCount;
    }
}
