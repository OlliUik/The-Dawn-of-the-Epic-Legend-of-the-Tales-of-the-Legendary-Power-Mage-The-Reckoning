using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aoe : Spell
{

    [Header("AoE varialbes")]
    [SerializeField] protected float radius = 7.0f;
    [SerializeField] protected float duration = 5.0f;

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
        aoe.transform.parent = spellbook.transform;

        aoe.ApplyModifiers(aoe.gameObject, data);

        aoe.StartCoroutine(DestroyAoe(aoe.gameObject, duration));
        spellbook.StopCasting();

    }

    private void Update()
    {
        // find out what is inside the radius
        var auraArea = Physics.OverlapSphere(transform.position, radius);
        foreach (var objectHit in auraArea)
        {
            // check if objectHit is enemy
            if (objectHit.gameObject.CompareTag("Enemy"))
            {
                // apply all modifiers here to the enemy inside radius
                OnCollision[] collisionModifiers = GetComponents<OnCollision>();
                foreach (OnCollision modifier in collisionModifiers)
                {
                    //modifier.Apply(objectHit.gameObject);
                }
            }
        }
    }

    public void ModifyRange(float amount)
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

}
