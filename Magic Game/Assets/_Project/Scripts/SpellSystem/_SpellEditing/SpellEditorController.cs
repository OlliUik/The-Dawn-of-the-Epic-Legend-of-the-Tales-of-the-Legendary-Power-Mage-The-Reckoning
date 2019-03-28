using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellEditorController : MonoBehaviour
{

    [Header("Cards")]
    public List<Card> allCards                      = new List<Card>();
    public List<Card> chosenCards                   = new List<Card>();

    public GameObject cardPrefab                    = null;
    public Transform spawnPosition                  = null;
    public Transform[] availableCardPositions       = new Transform[3];
    private CardDisplay[] availableCards            = new CardDisplay[3];


    void Start()
    {

        // make each card a prefab and make spell editing menu with game world objects aka. NO UI ELEMENTS


        for (int i = 0; i < availableCardPositions.Length; i++)
        {

            GameObject newCard = Instantiate(cardPrefab, spawnPosition.position, spawnPosition.rotation);

            CardAnimation anim = newCard.GetComponent<CardAnimation>();
            anim.startPos = spawnPosition.position;
            anim.endPos = availableCardPositions[i].position;
            anim.AnimateCard();

        }

    }

    public void ChooseCard(CardDisplay cardDisplay)
    {

        if(!chosenCards.Contains(cardDisplay.card))
        {
            chosenCards.Add(cardDisplay.card);
            print("You chose: " + cardDisplay.card);
        }

        cardDisplay.ChooseCard();

    }

}
