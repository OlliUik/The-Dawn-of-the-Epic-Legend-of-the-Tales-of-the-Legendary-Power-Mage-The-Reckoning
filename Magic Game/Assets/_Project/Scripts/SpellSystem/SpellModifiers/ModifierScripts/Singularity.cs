using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singularity : SpellModifier
{

    public GameObject singularityPrefab = null;
    public BlackHoleVariables variables;

    bool spawned = false;
    GameObject prefab;

    public float aoeRadius = 5f;
    public int aoeAmount = 4;


    public override void OnSpellCast(Spell spell)
    {
        if(spell.GetType() == typeof(Aoe))
        {
            if (!spawned)
            {
                Aoe aoe = (Aoe)spell;
                for (int i = 0; i < aoeAmount; i++)
                {
                    float angle = i * Mathf.PI * 2f / aoeAmount;
                    Vector3 newPos = new Vector3(((Mathf.Cos(angle) * aoeRadius)), 0, Mathf.Sin(angle) * aoeRadius) + transform.position;
                    prefab = Instantiate(singularityPrefab, newPos, Quaternion.identity);
                    prefab.transform.SetParent(gameObject.transform);
                    BlackHole script = prefab.GetComponent<BlackHole>();
                    script.variables = variables;
                    script.variables.duration = aoe.duration;
                }
                spawned = true;
            }
        }
    }

    public override void BeamCollide(RaycastHit hitInfo, Vector3 direction, float distance)
    {
        // singularity on and pull towards hitInfo.point

        if(!spawned)
        {
            prefab = Instantiate(singularityPrefab);
            prefab.transform.SetParent(gameObject.transform);
            prefab.GetComponent<BlackHole>().variables = variables;
            spawned = true;
        }

        prefab.SetActive(true);
        prefab.transform.position = hitInfo.point;

    }
    public override void BeamCollisionEnd()
    {
        // singularity off
        if(prefab != null)
        {
            prefab.SetActive(false);
        }
    }

    public override void ProjectileCollide(Collision collision, Vector3 direction)
    {
        // Instanciate singularity on the point of collision
        GameObject prefab = Instantiate(singularityPrefab, collision.contacts[0].point, Quaternion.FromToRotation(transform.forward, collision.contacts[0].normal));
        BlackHole script = prefab.GetComponent<BlackHole>();
        script.variables = variables;
        script.destroySelf = true;
    }

}
