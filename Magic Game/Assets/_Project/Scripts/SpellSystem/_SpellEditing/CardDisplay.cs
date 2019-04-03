using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{

    SpellEditorController editorController;

    public Card card;
    public Text nameText;
    public Text descriptionText;
    public Image artworkImage;

    private Button button;

    void Start()
    {
        button = GetComponentInChildren<Button>();
        editorController = transform.parent.GetComponentInParent<SpellEditorController>();
    }

    public void InitCard(Vector3 startPosition, Vector3 endPosition, Card card)
    {
        // fill spells data
        this.card = card;
        nameText.text =  "" + card.cardName;
        descriptionText.text =  "" + card.cardDescription;

        // move spell to the correct position
        GetComponent<CardAnimation>().AnimateCard(startPosition, endPosition);
    }

    public void ChooseCard()
    {
        if(editorController.highlighedSpell != null)
        {
            // VALIDATE CARD FOR THE SPELL
            if (!ValidateCard())
            {
                return;
            }

            // ADD THE CARD TO THE SELECTED SPELL
            editorController.highlighedSpell.cards.Add(card);

            // DESTROY THE REMAINING CARDS AND GENERATE NEW 3
            editorController.DestroyCards();
            editorController.StartCoroutine(editorController.GenerateCards());
        }
        else
        {
            print("Spell needs to be selected first");
        }

    }

    private bool ValidateCard()
    {
        if(card.types.Contains(SpellType.GENERIC))
        {
            return true;
        }

        for (int i = 0; i < card.types.Count; i++)
        {
            if(card.types[i] == editorController.highlighedSpell.type)
            {
                return true;
            }
        }

        //if(card.type == SpellType.GENERIC || card.type == editorController.highlighedSpell.type)
        //{
        //    return true;
        //}

        print("Cards type is not fitting for the spell...");
        return false;
    }

}
