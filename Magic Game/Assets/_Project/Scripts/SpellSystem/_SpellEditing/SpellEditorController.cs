using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellEditorController : MonoBehaviour
{

    public int crystalsLeft                         = 2;
    public Button useCrystalButton                  = null;

    [Header("Spells")]
    public SpellSlot[] spellsSlots                  = new SpellSlot[3];
    public SpellSlot highlighedSpell                = null;

    private Transform canvasBackground              = null;
    public Spellbook playersSpellbook               { get; private set; }

    [Header("Cards")]
    public List<Card> allCards                      = new List<Card>();
    public GameObject cardPrefab                    = null;
    public Transform spawnPosition                  = null;
    public Transform selectedPosition               = null;
    public Transform[] availableCardPositions       = new Transform[3];
    public GameObject[] currentCards                = new GameObject[3];
    public GameObject[] hideWhenCardSelected        = null;
    public GameObject[] showWhenCardSelected        = null;

    private GameObject selectedCard                 = null;
    
    private void Awake()
    {
        GameObject player = FindObjectOfType<PlayerCore>().gameObject;
        playersSpellbook = player.GetComponent<Spellbook>();
        canvasBackground = transform.GetChild(0);
    }

    void Start()
    {
        foreach (GameObject obj in hideWhenCardSelected)
        {
            obj.SetActive(true);
        }

        foreach (GameObject obj in showWhenCardSelected)
        {
            obj.SetActive(false);
        }

        // testing purposes
        //StartCoroutine(GenerateCards());

        // get all references to spells on player and cards they currently have
        for (int i = 0; i < spellsSlots.Length; i++)
        {
            spellsSlots[i].Init(playersSpellbook.spells[i]);
        }
    }

    public void UseCrustalButton()
    {
        crystalsLeft--;
        useCrystalButton.gameObject.SetActive(false);
        StartCoroutine(GenerateCards());
    }

    public IEnumerator GenerateCards()
    {
        for (int i = 0; i < availableCardPositions.Length; i++)
        {
            GameObject cardDisplay = Instantiate(cardPrefab, spawnPosition.position, spawnPosition.rotation);
            cardDisplay.transform.SetParent(canvasBackground);
            cardDisplay.transform.position = availableCardPositions[i].position;

            CardDisplay display = cardDisplay.GetComponent<CardDisplay>();
            display.InitCard(spawnPosition.localPosition, availableCardPositions[i].localPosition, allCards[Random.Range(0, allCards.Count)]);
            
            currentCards[i] = cardDisplay;
            yield return new WaitForSecondsRealtime(0.2f);
        }
    }

    public void HighlightSpellSlot(GameObject go)
    {
        //if (highlighedSpell != null)
        //{
        //    highlighedSpell.GetComponent<Image>().color = Color.white;
        //}

        highlighedSpell = go.GetComponent<SpellSlot>();
        //highlighedSpell.GetComponent<Image>().color = Color.red;

        if (selectedCard.GetComponent<CardDisplay>().ValidateCard())
        {
            selectedCard.GetComponent<CardDisplay>().ChooseCard();
            foreach (GameObject obj in hideWhenCardSelected)
            {
                obj.SetActive(true);
            }

            foreach (GameObject obj in showWhenCardSelected)
            {
                obj.SetActive(false);
            }
        }
    }

    public void HighlightCard(GameObject card)
    {
        if (selectedCard == null)
        {
            foreach (GameObject obj in hideWhenCardSelected)
            {
                obj.SetActive(false);
            }

            foreach (GameObject obj in showWhenCardSelected)
            {
                obj.SetActive(true);
            }

            selectedCard = card;
            foreach (GameObject go in currentCards)
            {
                if (go == card)
                {
                    go.GetComponent<CardAnimation>().AnimateCard(go.transform.localPosition, selectedPosition.localPosition, Vector3.one, Vector3.one * 0.5f);
                }
                else
                {
                    go.GetComponent<CardAnimation>().AnimateCard(go.transform.localPosition, spawnPosition.localPosition, Vector3.one, Vector3.zero);
                }
            }
        }
        else
        {
            foreach (GameObject obj in hideWhenCardSelected)
            {
                obj.SetActive(true);
            }

            foreach (GameObject obj in showWhenCardSelected)
            {
                obj.SetActive(false);
            }

            selectedCard = null;
            for (int i = 0; i < currentCards.Length; i++)
            {
                currentCards[i].GetComponent<CardAnimation>().AnimateCard(currentCards[i].transform.localPosition, availableCardPositions[i].localPosition, currentCards[i].transform.localScale, Vector3.one);
            }
        }
    }

    public void DestroyCards()
    {
        for (int i = 0; i < currentCards.Length; i++)
        {
            Destroy(currentCards[i]);
            currentCards[i] = null;
        }
    }

    public void CloseSpellEditionMenu()
    {
        playersSpellbook.GetComponent<PlayerCore>().ToggleSpellEditingUI();
    }

}
