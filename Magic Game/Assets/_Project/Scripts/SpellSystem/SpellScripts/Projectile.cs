using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Spell
{

    #region Variables

    [Header("Projectile variables")]
    [SerializeField] protected float baseDamage         = 50.0f;
    public float baseSpeed                              = 15.0f;

    public GameObject graphics                          = null;
    public GameObject explosionParticle                 = null;

    public Vector3 direction                            { get; set; }
    private Vector3 lastPos                             = Vector3.zero;
    private float distanceTravelled                     = 0.0f;

    public SpellModifier[] modifiers;
    public bool isMaster = false;

    private bool inited = false;

    #endregion

    #region Unitys_Methods

    void Start()
    {
        if (!inited) Init();
    }

    void FixedUpdate()
    {
        distanceTravelled += Vector3.Distance(transform.position, lastPos);
        lastPos = transform.position;
        transform.position += direction * baseSpeed * Time.fixedDeltaTime;

        if (distanceTravelled > range)
        {
            print("Out of range");
            DestroyProjectile();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // DEAL DAMAGE
        var health = collision.gameObject.GetComponent<Health>();
        if (health != null)
        {
            base.DealDamage(health, baseDamage);
        }

        // APPLY STATUSEFFECTS
        var effectManager = collision.gameObject.GetComponent<StatusEffectManager>();
        if (effectManager != null)
        {
            base.ApplyStatusEffects(effectManager, statusEffects);
        }
        else
        {
            // loop all effects and if there is water spawn them
            foreach (StatusEffect effect in statusEffects)
            {
                effect.HitNonlivingObject(collision);
            }
        }

        // APPLY ALL COLLISION MODIFIERS
        if(modifiers.Length > 0)
        {
            foreach (SpellModifier modifier in modifiers)
            {
                modifier.ProjectileCollide(collision, direction);
            }
        }

        // DESTROY ORGINAL
        DestroyProjectile();
    }

    #endregion

    #region Custom_Methods

    private void Init()
    {
        spellType = SpellType.PROJECTILE;
        GameObject graphicsCopy = Instantiate(graphics, transform.position, transform.rotation);
        graphicsCopy.transform.SetParent(gameObject.transform);
        lastPos = transform.position;
        modifiers = GetComponents<SpellModifier>();
    }

    private void DestroyProjectile()
    {
        Instantiate(explosionParticle, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public override void CastSpell(Spellbook spellbook, SpellData data)
    {

        ///<summary>
        ///
        ///                                 PROJECTILE SPELLS
        /// 
        ///     • Projectile it self only moves forwards until it's reached maxRange
        ///     • Projectiles can have multiple types of effects
        ///         • Projectiles can home towards closest enemy target
        ///         • Projectiles can have OnCollision modifiers that take effect when projectile collides with something (Split, Bounce, etc.)
        /// 
        /// </summary>


        // get the direction from the spellbook and spawn projectile accodring to that
        direction = spellbook.GetDirection();
        Quaternion rot = Quaternion.LookRotation(direction, Vector3.up);
        Projectile projectile = Instantiate(this, spellbook.spellPos.position, rot);
        projectile.transform.rotation = Quaternion.FromToRotation(projectile.transform.forward, direction);
        projectile.direction = direction;
        projectile.caster = spellbook.gameObject;
        projectile.isMaster = true;

        // apply all modifiers to the projectile ( this is inherited from spell class )
        ApplyModifiers(projectile.gameObject, data);

        // casting is done
        spellbook.StopCasting();

    }

    // THESE ARE USED TO MODIFY PROJECTILES BASE VALUES
    public void ModifyDamage(float amount)
    {
        baseDamage += amount;
    }

    public void ModifySpeed(float amount)
    {
        baseSpeed += amount;
    }

    #endregion

}
