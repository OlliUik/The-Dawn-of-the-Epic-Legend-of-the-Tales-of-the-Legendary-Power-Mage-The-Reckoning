using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Meteorite", menuName = "SpellSystem/Modifiers/Meteorite")]
public class MeteoriteModifier : SpellScriptableModifier
{

    public GameObject meteorPrefab;
    public float increasingScale = 1;
    public float increasingExplosionForce = 1;

    public override void AddSpellModifier(Spell spell)
    {
        var compo = spell.GetComponent<Meteorite>();
        if (compo != null)
        {
            compo.explosionForce += increasingExplosionForce;
            compo.meteorScale += increasingScale;
            return;
        }

        Meteorite component = spell.gameObject.AddComponent<Meteorite>();
        component.meteorPrefab = meteorPrefab;
    }

}
