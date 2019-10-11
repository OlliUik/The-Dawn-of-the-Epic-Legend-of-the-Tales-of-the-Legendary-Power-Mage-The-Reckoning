using UnityEngine;

public abstract class SpellScriptableModifier : ScriptableObject
{

    [SerializeField] public GameObject projectileGraphics;
    [SerializeField] public GameObject beamGraphics;
    [SerializeField] public GameObject aoeGraphics;
    public abstract void AddSpellModifier(Spell spell);

}
