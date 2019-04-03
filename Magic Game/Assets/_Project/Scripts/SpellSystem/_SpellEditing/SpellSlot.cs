using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellSlot : MonoBehaviour
{
    public SpellType type = SpellType.GENERIC;
    public List<Card> cards = new List<Card>();

    private Text spellSlotText;

    private void Start()
    {
        spellSlotText = GetComponentInChildren<Text>();
        spellSlotText.text = type.ToString();
    }
}
