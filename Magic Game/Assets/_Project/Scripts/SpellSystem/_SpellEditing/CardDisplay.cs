using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{

    public Card card;
    public Text nameText = null;
    public Text descriptionText = null;
    public Image artworkImage;

    private Button button;

    void Start()
    {
        button = GetComponentInChildren<Button>();
    }

    public void InitCard(Vector3 endPosition)
    {
        // fill spells data
        nameText.text = card.cardName;
        descriptionText.text = card.cardDescription;

        // move spell to the correct position
        GetComponent<CardAnimation>().AnimateCard();
    }

    public void ChooseCard()
    {
        button.interactable = false;
        // make the card go to the right spell
    }


}
