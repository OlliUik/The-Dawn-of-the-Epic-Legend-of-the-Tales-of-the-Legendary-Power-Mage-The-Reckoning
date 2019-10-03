using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Direction : SpellModifier
{

    #region Variables

    public float rotationSpeed = 5f;

    private Spellbook spellbook;
    private Vector3 direction;
    private RaycastHit hit;
    Projectile pro;

    #endregion

    #region Unity_Methods

    private void Start()
    {
        pro = GetComponent<Projectile>();
        spellbook = pro.caster.GetComponent<Spellbook>();
    }

    void FixedUpdate()
    {

        direction = spellbook.GetDirection();

        // Draws a ray and sees what player is aiming at
        // rotates projectile to face the target
        if (Physics.Raycast(spellbook.transform.position, direction, out hit))
        {
            float step = rotationSpeed * Time.deltaTime;
            Vector3 targetDir = hit.point - transform.position;
            pro.direction = Vector3.RotateTowards(pro.direction, targetDir, step, 0.0f);
        }
        else
        {
            hit.point = direction.normalized * 100f; // 100 units forward
            float step = rotationSpeed * Time.deltaTime;
            Vector3 targetDir = hit.point - transform.position;
            pro.direction = Vector3.RotateTowards(pro.direction, targetDir, step, 0.0f);
        }
    }

    #endregion

    #region Other_Methods

    public override void OnSpellCast(Spell spell)
    {
        spellbook = spell.caster.GetComponent<Spellbook>();
        direction = spellbook.GetDirection();
    }

    // Debug the result
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(hit.point != null)
        {
            Gizmos.DrawWireSphere(hit.point, 0.5f);
        }
    }

    #endregion

}
