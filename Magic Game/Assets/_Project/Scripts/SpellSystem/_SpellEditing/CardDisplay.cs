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

    void Start()
    {
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

    private void OnMouseEnter()
    {
        print("Mouse Enter");

        Vector3 startScale = gameObject.transform.localScale;
        Vector3 endScale = Vector3.one * 1.1f;

        float percent = 0f;

        while(percent < 1f)
        {
            transform.localScale = Vector3.Lerp(startScale, endScale, percent);
            percent += Time.unscaledDeltaTime;
        }
    }

    private void OnMouseExit()
    {
        print("Mouse Exit");

        Vector3 startScale = gameObject.transform.localScale;
        Vector3 endScale = Vector3.one;

        float percent = 0f;

        while (percent < 1f)
        {
            transform.localScale = Vector3.Lerp(startScale, endScale, percent);
            percent += Time.unscaledDeltaTime;
        }
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
            editorController.highlighedSpell.data.cards.Add(card);

            // DESTROY THE REMAINING CARDS AND GENERATE NEW 3
            editorController.DestroyCards();

            // IF PLAYER HAS MORE CRYSTALS ALLOW MORE CARDS
            editorController.useCrystalButton.gameObject.SetActive(true);
            editorController.useCrystalButton.interactable = editorController.crystalsLeft > 0 ? true : false;

            if(editorController.crystalsLeft <= 0)
            {
                editorController.CloseSpellEditionMenu();
            }

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
            if(card.types[i] == editorController.highlighedSpell.data.type)
            {
                return true;
            }
        }

        print("Cards type is not fitting for the spell...");
        return false;
    }

}
