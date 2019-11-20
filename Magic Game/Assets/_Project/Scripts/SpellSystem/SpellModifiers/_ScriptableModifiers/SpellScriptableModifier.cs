using UnityEngine;

public abstract class SpellScriptableModifier : ScriptableObject
{

    [SerializeField] public GameObject projectileGraphics;
    [SerializeField] public Beam.ElementType beamGraphics;
    [SerializeField] public GameObject aoeGraphics;
    [SerializeField] public GameObject projectileExploionGraphics;
    public abstract void AddSpellModifier(Spell spell);

}
