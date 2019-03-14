using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : Spell
{

    [Header("-- Beam --")]
    [SerializeField] private float range = 150.0f;

    public override void CastSpell(Spellbook spellbook, int spellIndex, Vector3 direction)
    {
        Quaternion rot = Quaternion.LookRotation(direction, Vector3.up);
        Beam beam = Instantiate(this, spellbook.spellPos.position, rot);
        beam.transform.parent = spellbook.transform;

        ApplyModifiers(beam.gameObject, spellIndex, spellbook);

        beam.StartCoroutine(CastBeam(beam.gameObject, spellbook, spellIndex));
    }

    IEnumerator CastBeam(GameObject self, Spellbook spellbook, int spellIndex)
    {
        while (true)
        {
            print("Casting beam");

            Vector3 direction = spellbook.GetDirection2();
            Ray ray = new Ray(spellbook.spellPos.position, direction * range);
            RaycastHit hit;

            // if beam hits something do this
            if (Physics.Raycast(ray, out hit))
            {
                Debug.DrawRay(spellbook.spellPos.position, hit.point, Color.green);
                print("Beam hits something");

                // apply beam effects here to target we hit
                if(hit.transform.GetComponent<Rigidbody>() != null)
                {
                    OnCollision[] collisionMods = GetComponents<OnCollision>();
                    foreach (OnCollision mod in collisionMods)
                    {
                        mod.Hit(hit.collider.gameObject, spellbook);
                    }
                }
            }
            else
            {
                Debug.DrawRay(spellbook.spellPos.position, direction * range, Color.red);
            }


            if(Input.GetKeyUp((spellIndex + 1).ToString()) || !Input.GetKey((spellIndex + 1).ToString()))
            {
                print("Beam cast ended");
                break;
            }

            yield return null;
        }

        spellbook.StopCasting();
        spellbook.SetCooldown();
        Destroy(self);
    }

}
