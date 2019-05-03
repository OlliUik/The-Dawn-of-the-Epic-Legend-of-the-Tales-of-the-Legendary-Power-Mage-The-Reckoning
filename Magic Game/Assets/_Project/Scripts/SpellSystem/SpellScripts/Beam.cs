using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : Spell
{

    #region Variables

    [Header("-- Beam --")]
    [SerializeField] private float baseDamage       = 1.0f;
    [SerializeField] private float baseRange        = 150.0f;
    [SerializeField] private float baseRadius       = 1f;
    public float angle;
    public float distanceTravelled;

    public float Range
    {
        get { return baseRange; }
        set { baseRange = value; }
    }

    [SerializeField] private GameObject graphics    = null;

    public Vector3 startPos                         = Vector3.zero;
    public Vector3 endPos                           = Vector3.zero;

    public Vector3 direction                        = Vector3.zero;

    private Spellbook spellbook;
    private RaycastHit hit;
    int spellIndex                                  = 0;

    public bool isMaster                            = false;
    SpellModifier[] modifiers;

    private bool colliding                          = false;
    private bool collEndCalled                      = false;

    #endregion

    public override void CastSpell(Spellbook spellbook, SpellData data)
    {
        // get the look direction from spellbook and spawn new beam according to that // also child it to player to follow pos and rot
        direction = spellbook.GetDirection();
        Quaternion rot = Quaternion.LookRotation(direction, Vector3.up);
        Beam beam = Instantiate(gameObject, spellbook.spellPos.position, rot).GetComponent<Beam>();
        beam.caster = spellbook.gameObject;
        beam.isMaster = true;
        
        // apply all spellmodifiers to the beam
        ApplyModifiers(beam.gameObject, data);

    }

    private void Start()
    {
        if(isMaster)
        {
            spellbook = caster.GetComponent<Spellbook>();
        }

        modifiers = GetComponents<SpellModifier>();
    }

    private void Update()
    {
        if (isMaster)
        {
            direction = Quaternion.Euler(0, angle, 0) * spellbook.GetDirection();
            startPos = spellbook.spellPos.position;
            spellbook.mana.UseMana(ManaCost * Time.deltaTime);
        }

        if (Physics.SphereCast(startPos, baseRadius, direction, out hit, baseRange))
        {
            endPos = hit.point;
            distanceTravelled = (hit.point - startPos).magnitude;

            var health = hit.collider.gameObject.GetComponent<Health>();
            if (health != null)
            {
                base.DealDamage(health, baseDamage * Time.deltaTime);
            }

            var effectManager = hit.collider.gameObject.GetComponent<StatusEffectManager>();
            if (effectManager != null)
            {
                base.ApplyStatusEffects(effectManager, statusEffects);
            }

            foreach (SpellModifier modifier in modifiers)
            {
                modifier.BeamCollide(hit, direction, distanceTravelled);
            }

            colliding = true;
            collEndCalled = false;
        }
        else
        {
            colliding = false;
            endPos = startPos + (direction * baseRange);

            if (!collEndCalled)
            {
                CollisionEnd();
                collEndCalled = true;
            }
        }

        UpdateBeam(startPos, direction);

        // stop casting here
        if((Input.GetMouseButtonUp(0) || !Input.GetMouseButton(0)) || (isMaster && spellbook.mana.mana <= 0f))
        {
            CastingEnd();

            if (isMaster)
            {
                spellbook.StopCasting();
            }
            Destroy(gameObject);
        }

    }

    public void CollisionEnd()
    {
        foreach (SpellModifier modifier in modifiers)
        {
            modifier.BeamCollisionEnd();
        }
    }

    public void CastingEnd()
    {
        foreach (SpellModifier modifier in modifiers)
        {
            modifier.BeamCastingEnd();
        }
    }

    public void UpdateBeam(Vector3 startPosition, Vector3 direction)
    {
        // position
        Vector3 offset = endPos - startPos;
        Vector3 position = startPos + (offset * 0.5f);
        graphics.transform.position = position;

        // scale
        Vector3 localScale = graphics.transform.localScale;
        localScale.y = (endPos - startPos).magnitude * 0.5f;
        graphics.transform.localScale = localScale;
        
        graphics.transform.rotation = Quaternion.FromToRotation(Vector3.up, offset);
    }

    //IEnumerator CastBeam(GameObject self, Spellbook spellbook, SpellData data)
    //{

    //    print("Started beam cast");

    //    int spellIndex = 0;
    //    for (int i = 0; i < spellbook.spells.Length; i++)
    //    {
    //        if(spellbook.spells[i].spell == data.spell)
    //        {
    //            spellIndex = i;
    //            break;
    //        }
    //    }

    //    SpellModifier[] modifiers = self.GetComponents<SpellModifier>();

    //    while (true)
    //    {

    //        // if radius is samller than X limit do beam collision check with ray
    //        // else if radius is bigger make capsule cast from spellcast position to look direction with range, returns collider[]
    //        // if collider[].length > 0     compare distances and get the closest one to the caster we hit...

    //        // keep updating the direction the player is looking and check if our beam hits something


    //        //if (baseRadius > 1.0f)
    //        //{
    //        //    hitObject = CapsuleBeam(spellbook, self);
    //        //}
    //        //else
    //        //{
    //        //    hitInfo = RaycastBeam(spellbook, self);
    //        //}

    //        //if(hitObject.CompareTag("Player") || hitObject.CompareTag("Enemy"))
    //        //{
    //        //    // deal damage
    //        //    print("Deal damage");
    //        //}


    //        print("castin beam");

    //        Vector3 direction = spellbook.GetDirection();

    //        Ray ray = new Ray(spellbook.spellPos.position, direction * baseRange);
    //        RaycastHit hitInfo;

    //        // if beam hits something apply all collision modifiers to the hitObject
    //        if (Physics.Raycast(ray, out hitInfo, baseRange))
    //        {
    //            Debug.DrawRay(spellbook.spellPos.position, (hitInfo.point - spellbook.spellPos.position), Color.red);
    //            foreach (SpellModifier modifier in modifiers)
    //            {
    //                modifier.BeamCollide(hitInfo, direction);
    //            }
    //        }
    //        else
    //        {
    //            // do max range beam if nothing is hit
    //            Debug.DrawRay(spellbook.spellPos.position, ray.direction * baseRange, Color.green);
    //        }

    //        // if player is not pressing or releases the beam key stop the cast
    //        if(Input.GetKeyUp((spellIndex + 1).ToString()) || !Input.GetKey((spellIndex + 1).ToString()))
    //        {
    //            print("Beam cast ended");
    //            break;
    //        }

    //        yield return null;
    //    }

    //    // stop the spellcast and set the cooldown for the spell
    //    spellbook.StopCasting();
    //    Destroy(self);

    //}

    private RaycastHit RaycastBeam(Spellbook spellbook, GameObject self)
    {

        direction = spellbook.GetDirection();
        Ray ray = new Ray(spellbook.spellPos.position, direction * baseRange);
        RaycastHit hit;

        // if beam hits something do this
        if (Physics.Raycast(ray, out hit, baseRange))
        {
            Debug.DrawRay(spellbook.spellPos.position, (hit.point - spellbook.spellPos.position), Color.red);
            return hit;
        }
        else
        {
            // do max range beam if nothing is hit
            Debug.DrawRay(spellbook.spellPos.position, ray.direction * baseRange, Color.green);
            return hit;
        }
    }

    private GameObject CapsuleBeam(Spellbook spellbook, GameObject self)
    {

        Collider[] objectsHit = Physics.OverlapCapsule(spellbook.spellPos.position, direction * baseRange, baseRadius);
        GameObject closest = null;
      
        for (int i = 0; i < objectsHit.Length; i++)
        {
            if(closest != null)
            {
                if((objectsHit[i].gameObject.transform.position - spellbook.transform.position).magnitude < (closest.transform.position - spellbook.transform.position).magnitude)
                {
                    closest = objectsHit[i].gameObject;
                }
            }
            else
            {
                closest = objectsHit[i].gameObject;
            }
        }
        return closest;
    }

    // USED TO MODIFY BASE VALUES
    public void ModifyDamage(float amount)
    {
        baseDamage += amount;
    }

    public void ModifyRange(float amount)
    {
        baseRange += amount;
    }

    public void ModifyRadius(float amount)
    {
        baseRadius += amount;
    }

    //for debugging
    private void OnDrawGizmos()
    {
        DebugBeam(startPos, endPos);
    }
    private void DebugBeam(Vector3 startPos, Vector3 endPos)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(endPos, baseRadius * 0.5f);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(startPos, baseRadius * 0.5f);
    }

}
