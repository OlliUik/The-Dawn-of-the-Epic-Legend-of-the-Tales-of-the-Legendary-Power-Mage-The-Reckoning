using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellEditorController : MonoBehaviour
{

    [Header("Spells")]
    public SpellSlot[] spellsSlots                  = new SpellSlot[3];
    public SpellSlot highlighedSpell                = null;

    private Transform canvasBackground              = null;
    private Spellbook playersSpellbook              = null;

    [Header("Cards")]
    public List<Card> allCards                      = new List<Card>();
    public GameObject cardPrefab                    = null;
    public Transform spawnPosition                  = null;
    public Transform[] availableCardPositions       = new Transform[3];
    private GameObject[] currentCards               = new GameObject[3];

    private void Awake()
    {
        //GameObject player = FindObjectOfType<PlayerCore>().gameObject;
        //playersSpellbook = player.GetComponent<Spellbook>();

        canvasBackground = transform.GetChild(0);
    }

    void Start()
    {
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
            yield return new WaitForSeconds(0.2f);
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
