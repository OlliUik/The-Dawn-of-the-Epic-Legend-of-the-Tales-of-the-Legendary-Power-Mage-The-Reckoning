using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderVariables : MonoBehaviour
{

    public float duration = 0f;
    SphereCollider sphereChecker;

    public float extraManaCost = 0f;
    public Mana playerMana;
    public GameObject electricParticlePrefab;

    Spellbook spellbook;

    bool isSetUp = false;

    bool originalSpellbookEnableCasting = true;

    SphereCollider triggerChecker;

    private void Start()
    {
        sphereChecker = gameObject.AddComponent<SphereCollider>();
        spellbook = GetComponent<Spellbook>();
        if (spellbook != null) originalSpellbookEnableCasting = spellbook.enableCasting;
    }

    public void Init(float duration, float extraManaCost, Mana playerMana, GameObject electricParticlePrefab)
    {
        this.duration = duration;
        this.extraManaCost = extraManaCost;
        this.playerMana = playerMana;
        if(electricParticlePrefab != null)
        {
            this.electricParticlePrefab = Instantiate(electricParticlePrefab, transform.position + Vector3.up, Quaternion.FromToRotation(-electricParticlePrefab.transform.up, Vector3.up));
            this.electricParticlePrefab.transform.SetParent(transform);
        }
        triggerChecker = gameObject.AddComponent<SphereCollider>();
        triggerChecker.isTrigger = true;
        triggerChecker.radius = 3f;
        isSetUp = true;
    }

    private void Update()
    {
        if (isSetUp)
        {
            if (spellbook != null) spellbook.enableCasting = false;
            duration -= Time.deltaTime;
            if (duration <= 0f)
            {
                Destroy(this);
            }
        }
    }

    private void OnDestroy()
    {
        if (spellbook != null) spellbook.enableCasting = originalSpellbookEnableCasting;
        if (electricParticlePrefab != null) Destroy(electricParticlePrefab);
        if (triggerChecker != null) Destroy(triggerChecker);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<EnemyCore>() != null)
        {
            // Must not affected if other already have thunder
            if (other.GetComponent<ThunderVariables>() == null)
            {
                if (playerMana.mana > extraManaCost)
                {
                    playerMana.UseMana(extraManaCost);
                    other.gameObject.AddComponent<ThunderVariables>();
                    other.GetComponent<ThunderVariables>().Init(duration, extraManaCost, playerMana, electricParticlePrefab);
                }
            }
            else
            {
                if (duration < other.GetComponent<ThunderVariables>().duration)
                {
                    duration = other.GetComponent<ThunderVariables>().duration;
                }
            }
        }
    }

}
