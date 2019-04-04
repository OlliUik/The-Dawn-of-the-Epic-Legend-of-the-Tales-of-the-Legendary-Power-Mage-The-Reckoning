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
    public Transform[] availableCardPositions       = new Transform[3];
    public GameObject[] currentCards               = new GameObject[3];


    private void Awake()
    {
        GameObject player = FindObjectOfType<PlayerCore>().gameObject;
        playersSpellbook = player.GetComponent<Spellbook>();
        canvasBackground = transform.GetChild(0);
    }

    void Start()
    {
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
        if(highlighedSpell != null)
        {
            highlighedSpell.GetComponent<Image>().color = Color.white;
        }

        highlighedSpell = go.GetComponent<SpellSlot>();
        highlighedSpell.GetComponent<Image>().color = Color.red;
    }

    public void DestroyCards()
    {
        for (int i = 0; i < currentCards.Length; i++)
        {
            Destroy(currentCards[i]);
            currentCards[i] = null;
        }
    }

}
