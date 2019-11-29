using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorite : SpellModifier
{

    public float meteorScale = 1;
    public float explosionForce = 20f;
    public float aoeRadius = 5f;
    public GameObject meteorPrefab;
    public int aoeAmount = 16;
    public float beamMiniCooldown = 0.2f;

    private float currentBeamCooldown = 0f;
    private bool canBeam = true;

    private void Start()
    {
        MeteorManager.Instance.explosionForce = explosionForce;
        MeteorManager.Instance.meteorScale = meteorScale;
    }

    private void Update()
    {
        if(currentBeamCooldown >= beamMiniCooldown)
        {
            canBeam = true;
        }
        else
        {
            currentBeamCooldown += Time.deltaTime;
        }
    }

    public override void OnSpellCast(Spell spell)
    {
        if (spell.spellType == SpellType.AOE)
        {
            //GameObject meteor = Instantiate(meteorPrefab, spell.caster.transform.position, spell.caster.transform.rotation);
            for (int i = 0; i < aoeAmount; i++)
            {
                float angle = i * Mathf.PI * 2f / aoeAmount;
                Vector3 newPos = new Vector3(((Mathf.Cos(angle) * aoeRadius) * meteorScale) , 0, Mathf.Sin(angle) * aoeRadius) + transform.position + new Vector3(0, transform.position.y + 16, 0);
                GameObject meteor = Instantiate(meteorPrefab, newPos, Quaternion.identity);
                meteor.transform.localScale = new Vector3(meteorScale, meteorScale, meteorScale);
            }
        }
    }

    public override void ProjectileCollide(Collision collision, Vector3 direction)
    {
        GameObject meteor = Instantiate(meteorPrefab, collision.contacts[0].point + new Vector3(0,16,0), Quaternion.identity);
        meteor.transform.localScale = new Vector3(meteorScale, meteorScale, meteorScale);
    }

    public override void BeamCollide(RaycastHit hitInfo, Vector3 direction, float distance)
    {
        if (canBeam)
        {
            currentBeamCooldown = 0f;
            canBeam = false;
            GameObject meteor = Instantiate(meteorPrefab, hitInfo.point + new Vector3(0, 16, 0), Quaternion.identity);
            meteor.transform.localScale = new Vector3(meteorScale, meteorScale, meteorScale);
        }
    }

}
