using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorite : SpellModifier
{

    public float meteorScale = 1;
    public float explosionForce = 20f;
    public GameObject meteorPrefab;

    private void Start()
    {
        MeteorManager.Instance.explosionForce = explosionForce;
    }

    public override void OnSpellCast(Spell spell)
    {
        if (spell.spellType == SpellType.AOE)
        {
            //TODO: AoE
            //GameObject meteor = Instantiate(meteorPrefab, spell.caster.transform.position, spell.caster.transform.rotation);
        }
    }

    public override void ProjectileCollide(Collision collision, Vector3 direction)
    {
        GameObject meteor = Instantiate(meteorPrefab, collision.contacts[0].point, Quaternion.identity);
    }

}
