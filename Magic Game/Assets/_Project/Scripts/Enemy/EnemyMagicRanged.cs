using UnityEngine;

[RequireComponent(typeof(Spellbook))]
public class EnemyMagicRanged : EnemyRanged
{
    public Spellbook cSpellBook { get; private set; } = null;

    protected override void Start()
    {
        base.Start();
        cSpellBook = GetComponent<Spellbook>();
    }
}
