using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderVariables : MonoBehaviour
{

    public float duration = 0f;
    SphereCollider sphereChecker;

    public float extraManaCost = 0f;
    public float extraMoistureDamage = 0f;
    public Mana playerMana;
    public Health playerHealth;
    public GameObject electricParticlePrefab;

    Spellbook spellbook;
    EnemyMelee enemyMelee;

    bool isSetUp = false;

    bool originalSpellbookEnableCasting = true;
    bool originalMeleeEnableAttacking = true;

    SphereCollider triggerChecker;

    private void Start()
    {
        spellbook = GetComponent<Spellbook>();
        if (spellbook != null) originalSpellbookEnableCasting = spellbook.enableCasting;
        enemyMelee = GetComponent<EnemyMelee>();
        if (enemyMelee != null) originalMeleeEnableAttacking = enemyMelee.enableAttack;
        if (GetComponent<StatusEffectManager>() != null)
        {
            bool isMoist = false;
            GetComponent<StatusEffectManager>().AppliedEffects.TryGetValue(StatusEffectManager.EffectType.Moisturize, out isMoist);
            if (isMoist)
            {
                // Deal extra damage here (hopefully)
                if(playerHealth != null) playerHealth.Hurt(extraMoistureDamage, true);
            }
        }
    }

    public void HurtFromMoisture()
    {
        if (playerHealth != null) playerHealth.Hurt(extraMoistureDamage, true);
    }

    public void Init(float duration, float extraManaCost, Mana playerMana, GameObject electricParticlePrefab, float extraMoistureDamage)
    {
        this.duration = duration;
        this.extraManaCost = extraManaCost;
        this.playerMana = playerMana;
        this.extraMoistureDamage = extraMoistureDamage;
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
            if (enemyMelee != null) enemyMelee.enableAttack = false;
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
        if (enemyMelee != null) enemyMelee.enableAttack = originalMeleeEnableAttacking;
        if (electricParticlePrefab != null) Destroy(electricParticlePrefab);
        if (triggerChecker != null) Destroy(triggerChecker);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Electric triggered something: " + other.name);
        if(other.GetComponent<EnemyCore>() != null)
        {
            Debug.Log("Player Mana: "+playerMana.mana+" Required Mana: " + extraManaCost);
            // Must not affected if other already have thunder
            if (other.GetComponent<ThunderVariables>() == null)
            {
                if (playerMana.mana > extraManaCost)
                {
                    playerMana.UseMana(extraManaCost);
                    other.gameObject.AddComponent<ThunderVariables>();
                    other.GetComponent<ThunderVariables>().Init(duration, extraManaCost, playerMana, electricParticlePrefab, extraMoistureDamage);
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
