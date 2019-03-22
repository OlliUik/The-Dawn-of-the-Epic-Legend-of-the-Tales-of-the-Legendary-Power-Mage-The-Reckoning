using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Spell
{

    [Header("-- Projectile --")]
    [SerializeField] protected float baseDamage         = 50.0f;
    [SerializeField] protected float baseRange          = 1000.0f;
    [SerializeField] protected float baseSpeed          = 15.0f;

    public Vector3 direction                            { get; set; }
    private Vector3 lastPos                             = Vector3.zero;
    private float distanceTravelled                     = 0.0f;

    void Start()
    {
        lastPos = transform.position;
        direction = transform.forward;
    }

    void FixedUpdate()
    {
        distanceTravelled += Vector3.Distance(transform.position, lastPos);
        lastPos = transform.position;

        if(distanceTravelled < baseRange)
        {
            transform.position += direction * baseSpeed * Time.deltaTime;
        }
        else
        {
            print("Out of range");
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // collided with player or enemy deal damage
        if(collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Health>().Hurt(baseDamage);

            foreach (ScriptableEffect effect in effects)
            {
                print("Applied: " + effect.name);
                collision.gameObject.GetComponent<StatusEffectManager>().AddStatusEffect(effect.InitializeEffect(collision.gameObject));
            }
        }

        // if some collision modifier is not ready yet...apply all and return 
        // if all collisions modifiers are ready destroy the projectile
        OnCollision[] collisionModifiers = GetComponents<OnCollision>();
        for (int i = 0; i < collisionModifiers.Length; i++)
        {
            if (!collisionModifiers[i].ready)
            {
                foreach (OnCollision modifier in collisionModifiers)
                {
                    modifier.OnCollide(collision);
                    return;
                }
            }

            print("All collision modifiers ready...destroying");
            Destroy(gameObject);
        }

        print("No collision modifiers...destroying");
        Destroy(gameObject);
    }

    public override void CastSpell(Spellbook spellbook, int spellIndex)
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
        projectile.direction = direction;

        // apply all modifiers to the projectile ( this is inherited from spell class )
        ApplyModifiers(projectile.gameObject, spellIndex, spellbook);

        // casting is done
        spellbook.StopCasting();

    }


    public void ModifyDamage(float amount)
    {
        baseDamage += amount;
    }

    public void ModifyRange(float amount)
    {
        baseRange += amount;
    }

    public void ModifySpeed(float amount)
    {
        baseSpeed += amount;
    }

}
