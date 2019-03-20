using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellEditorController : MonoBehaviour
{

    [Header("Cards")]
    public List<Card> allCards = new List<Card>();
    public List<Card> chosenCards = new List<Card>();


    // let player choose from these random cards to his spells
    private Card[] randomCards = new Card[3]; 

    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
