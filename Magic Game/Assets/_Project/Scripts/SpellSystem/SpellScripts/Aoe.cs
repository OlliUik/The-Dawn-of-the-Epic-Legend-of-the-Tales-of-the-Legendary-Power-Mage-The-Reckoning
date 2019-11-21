using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aoe : Spell
{

    #region Variables

    [Header("AoE varialbes")]
    [SerializeField] public float damagePerSecond = 1.0f;
    [SerializeField] public float radius = 7.0f;
    [SerializeField] public float duration = 10.0f;
    public GameObject graphics = null;
    public float graphicUpScale = 0f;

    private SpellModifier[] modifiers = null;

    #endregion

    #region UnityMethods

    private void Start()
    {
        spellType = SpellType.AOE;
        modifiers = GetComponents<SpellModifier>();
        InitElementGraphics();
    }

    private void InitElementGraphics()
    {
        List<GameObject> elementPrefabs = new List<GameObject>();
        foreach (SpellModifier modifier in modifiers)
        {
            Debug.Log("Current AoE modifier element check: " + modifier.name);
            if (modifier.aoeElementGraphic != null)
            {
                if (!elementPrefabs.Contains(modifier.aoeElementGraphic))
                {
                    elementPrefabs.Add(modifier.aoeElementGraphic);
                    Debug.Log("Current count = " + elementPrefabs.Count);
                }
            }
        }
        foreach (StatusEffect statusEffect in statusEffects)
        {
            Debug.Log("Current AoE status effect element check: " + statusEffect.name);
            if (statusEffect.aoeElementGraphic != null)
            {
                Debug.Log("Added " + statusEffect.name + " graphics");
                if (!elementPrefabs.Contains(statusEffect.aoeElementGraphic))
                {
                    elementPrefabs.Add(statusEffect.aoeElementGraphic);
                    Debug.Log("Current count = " + elementPrefabs.Count);
                }
            }
        }
        Debug.Log("All counts = " + elementPrefabs.Count);
        foreach (GameObject elementPrefab in elementPrefabs)
        {
            Instantiate(elementPrefab, transform.position, transform.rotation).transform.SetParent(transform);
        }
        if (graphics != null && elementPrefabs.Count <= 0)
        {
            GameObject copyGraphics = Instantiate(graphics, transform.position, Quaternion.FromToRotation(Vector3.forward, Vector3.up));
            copyGraphics.transform.localScale += new Vector3(graphicUpScale, graphicUpScale, graphicUpScale);
            copyGraphics.transform.SetParent(gameObject.transform);
        }
    }

    private void Update()
    {
        // find out what is inside the radius
        var auraArea = Physics.OverlapSphere(transform.position, radius);
        foreach (var objectHit in auraArea)
        {
            
            // check if objectHit is enemy
            if (objectHit.transform.tag != caster.tag)
            {
                var health = objectHit.GetComponent<Health>();
                if (health != null)
                {
                    base.DealDamage(health, (damagePerSecond * Time.deltaTime));
                    if(objectHit.GetComponent<SpellTypeAmount>() != null) objectHit.GetComponent<SpellTypeAmount>().aura = true; //ScoreUI
                }

                addStatusEffect(statusEffects, objectHit);

                // apply all modifiers here to the enemy inside radius
                foreach (SpellModifier modifier in modifiers)
                {
                    modifier.AoeCollide(objectHit.gameObject);
                }
            }
        }
    }

    private void addStatusEffect(List<StatusEffect> statusEffects, Collider objectHit)
    {
        
        List<StatusEffect> temp = new List<StatusEffect>();
        foreach (StatusEffect statusEffect in statusEffects)
        {
            temp.Add(statusEffect.Clone());
            //Debug.Log(statusEffect.name + " cloned to AoE");
        }

        var effectManager = objectHit.GetComponent<StatusEffectManager>();
        if (effectManager != null)
        {
            //Debug.Log("AoE Hit " + effectManager.gameObject.name);
            base.ApplyStatusEffects(effectManager, temp);
        }
    }

    // debug stuff
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        DrawCircle(this.transform.position, radius);
    }
    private void DrawCircle(Vector3 position, float radius, int precision = 32)
    {
        Vector3 previousPoint = position + this.transform.right * radius;

        for (int i = 1; i <= precision; i++)
        {
            var angle = 2 * Mathf.PI * (i / (float)precision);

            Vector3 newPoint = position
                + this.transform.right * radius * Mathf.Cos(angle)
                + this.transform.forward * radius * Mathf.Sin(angle);
            Gizmos.DrawLine(newPoint, previousPoint);
            previousPoint = newPoint;
        }
    }

    #endregion

    #region CustomMethods

    public override void CastSpell(Spellbook spellbook, SpellData data)
    {

        ///<summary>
        ///
        ///                                 AOE SPELLS
        /// 
        ///     • Aoe spells have aura like effects in default
        ///     • Aura like effects can be overritten by adding a Placable property that allows player to place them like turrets
        ///     • Auras have range modifier that determinates how far they reach, can be modified
        ///     • Aoe spells duration determinates how long the spell will last, can be modified
        ///     • Aoe spells work well with elements (burn, slow, etc.)
        ///         • Aoe effects can be added to projectiles   ??????
        /// 
        /// </summary>

        // spawn instance in players current position
        Aoe aoe = Instantiate(this, spellbook.transform.position, spellbook.transform.rotation);
        aoe.transform.SetParent(spellbook.transform);
        aoe.caster = spellbook.gameObject;

        aoe.ApplyModifiers(aoe.gameObject, data);

        aoe.StartCoroutine(DestroyAoe(aoe.gameObject, duration));
        spellbook.StopCasting();

    }

    public override void ModifyRange(float amount)
    {
        radius += amount;
        print("Radius increased to " + radius);
    }

    public void ModifyDuration(float amount)
    {
        duration += amount;
        print("Duration increased to " + duration);
    }

    // Destroying spell here
    private IEnumerator DestroyAoe(GameObject self, float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(self);
    }

    #endregion


}