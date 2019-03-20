using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Spell
{

    [Header("-- Projectile --")]
    [SerializeField] protected float range = 1000.0f;
    [SerializeField] protected float speed = 15.0f;
    [SerializeField] GameObject explosionVfx = null;

    public Vector3 direction { get; set; }
    private Vector3 lastPos = Vector3.zero;
    private float distanceTravelled = 0.0f;

    void Start()
    {
        lastPos = transform.position;
        direction = transform.forward;
    }

    void FixedUpdate()
    {

        distanceTravelled += Vector3.Distance(transform.position, lastPos);
        lastPos = transform.position;

        if(distanceTravelled < range)
        {
            transform.position += direction * speed * Time.deltaTime;
        }
        else
        {
            print("Out of range");
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Health>() != null)
        {
            // collided with player or enemy
            collision.gameObject.GetComponent<Health>().Hurt(amount);
            Instantiate(explosionVfx, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        OnCollision[] collMods = GetComponents<OnCollision>();
        for (int i = 0; i < collMods.Length; i++)
        {
            if (!collMods[i].ready)
            {
                foreach (OnCollision mod in collMods)
                {
                    mod.OnCollide(collision);
                    return;
                }
            }
        }

        print("No collision modifiers...destroying");
        Destroy(gameObject);
    }

    public override void CastSpell(Spellbook spellbook, int spellIndex, Vector3 direction)
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

        direction = spellbook.GetDirection();
        Quaternion rot = Quaternion.LookRotation(direction, Vector3.up);
        Projectile proj = Instantiate(this, spellbook.spellPos.position, rot);
        proj.direction = direction;

        ApplyModifiers(proj.gameObject, spellIndex, spellbook);

        spellbook.StopCasting();

    }

}
