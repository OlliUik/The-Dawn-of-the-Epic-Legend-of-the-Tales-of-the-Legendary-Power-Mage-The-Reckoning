using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformEnemyTo : SpellModifier
{

    public GameObject transformPrefab { get; set; }
    public float duration;
    public GameObject transformationParticles { get; set; }
    public float extraMana = 0f;
    public Mana castersMana;
    public bool aoeUseExtraMana = true;
    public bool beamUseExtraMana = true;
    public bool projectileUseExtraMana = true;

    public override void OnSpellCast(Spell spell)
    {
        base.OnSpellCast(spell);
        castersMana = spell.caster.GetComponent<Mana>();
    }

    public override void AoeCollide(GameObject hitObject)
    {
        /*
        if ( extraMana != 0 && castersMana != null && hitObject.GetComponent<EnemyCore>() != null)
        {
            if (castersMana.mana >= extraMana)
            {
                Debug.Log("Frog extra mana condition");
                castersMana.UseMana(extraMana);
                InitTransformation(hitObject);
            }
        }
        else
        {
            InitTransformation(hitObject);
        }
        */
        CheckAndInitTransformation(hitObject, aoeUseExtraMana);
    }

    public override void BeamCollide(RaycastHit hitInfo, Vector3 direction, float distance)
    {
        CheckAndInitTransformation(hitInfo.collider.gameObject, beamUseExtraMana);
    }

    public override void ProjectileCollide(Collision collision, Vector3 direction)
    {
        CheckAndInitTransformation(collision.gameObject, projectileUseExtraMana);
    }

    private void CheckAndInitTransformation(GameObject orginal, bool extraCheck)
    {
        if (extraCheck && extraMana != 0 && castersMana != null && orginal.GetComponent<EnemyCore>() != null && orginal.GetComponent<BossLizardKing>() == null)
        {
            if (castersMana.mana >= extraMana)
            {
                Debug.Log("Frog extra mana condition");
                castersMana.UseMana(extraMana);
                InitTransformation(orginal);
            }
        }
        else
        {
            InitTransformation(orginal);
        }
    }

    private void InitTransformation(GameObject orginal)
    {
        if(orginal.GetComponent<EnemyCore>() != null && orginal.GetComponent<BossLizardKing>() == null)
        {
            if (orginal.GetComponent<Rigidbody>() != null && orginal.transform.GetComponent<Transformation>() == null && orginal.transform.parent == null)
            {
                orginal.SetActive(false);

                if (transformPrefab != null)
                {
                    Transformation tempTransform = Instantiate(transformPrefab, orginal.transform.position, Quaternion.identity).GetComponent<Transformation>();
                    tempTransform.TransformedObject = orginal;
                    tempTransform.duration = duration;
                    tempTransform.transformationParticles = transformationParticles;
                }

                if (transformPrefab != null)
                {
                    Instantiate(transformationParticles, orginal.transform.position, Quaternion.identity);
                }
            }
        }
    }

}
