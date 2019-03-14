using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aoe : Spell
{

    [Header("-- AoE --")]
    [SerializeField] protected float radius = 7.0f;
    [SerializeField] protected float duration = 5.0f;

    public override void CastSpell(Spellbook spellbook, int spellIndex, Vector3 direction)
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

        aoe.StartCoroutine(DestroyAoe(aoe.gameObject, spellbook, duration));
        spellbook.StopCasting();
        spellbook.SetCooldown();

    }

    private void Update()
    {
        // find out what is inside the radius
        var auraArea = Physics.OverlapSphere(transform.position, radius);
        foreach (var objectHit in auraArea)
        {
            // if object has health component / rb do something
            //Character character = objectHit.GetComponent<Character>();
            //if (character != null)
            //{
            //    // apply all modifiers here to the enemy inside radius
            //    AoeModifier[] spellModifiers = GetComponents<AoeModifier>();
            //    foreach (AoeModifier mod in spellModifiers)
            //    {
            //        mod.Apply(character);
            //    }
            //}
        }

    }

    // Destroying spell here
    private IEnumerator DestroyAoe(GameObject self, Spellbook spellbook, float duration)
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
