using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpellData
{
    public SpellType type;
    public Spell spell;
    public List<Card> cards;
}
