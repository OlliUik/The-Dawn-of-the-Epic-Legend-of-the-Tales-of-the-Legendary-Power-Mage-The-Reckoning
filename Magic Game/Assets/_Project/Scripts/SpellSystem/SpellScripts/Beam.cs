using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : Spell
{

    // TODO:: Make the beam collide with explosive casks

    #region Variables

    [Header("-- Beam --")]
    public bool usingCylinder = true;
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

    [SerializeField] private List<GameObject> graphics;
    private List<ParticleSystem> beamParticles;
    private List<ParticleCollisionEvent> collisionEvents;

    public Vector3 startPos                         = Vector3.zero;
    public Vector3 endPos                           = Vector3.zero;

    public Vector3 direction                        = Vector3.zero;

    private Spellbook spellbook;
    private RaycastHit hit;
    int spellIndex                                  = -1;

    public bool isMaster                            = false;
    SpellModifier[] modifiers;

    private bool colliding                          = false;
    private bool collEndCalled                      = false;

    [SerializeField] private GameObject DefaultParticlePrefab;
    [SerializeField] private GameObject FireElementParticlePrefab;
    [SerializeField] private GameObject WaterElementParticlePrefab;
    [SerializeField] private GameObject IceElementParticlePrefab;
    [SerializeField] private GameObject ElectricElementParticlePrefab;

    #endregion

    #region Unity_Methods

    public enum ElementType{
        Default,
        Fire,
        Water,
        Ice,
        Electric
    }

    private void Start()
    {

        graphics = new List<GameObject>();
        beamParticles = new List<ParticleSystem>();

        modifiers = GetComponents<SpellModifier>();

        Debug.Log("Spell modifier count " + modifiers.Length);
        Debug.Log("Spell status effect count " + statusEffects.Count);

        InitElementGraphics();

        for(int i = 0 ; i < graphics.Count ; i++)
        {
            //graphics[i] = Instantiate(graphics[i], transform.position, transform.rotation);
            beamParticles.Add(graphics[i].GetComponent<ParticleSystem>());
        }
        
        collisionEvents = new List<ParticleCollisionEvent>();

        if(isMaster)
        {
            spellbook = caster.GetComponent<Spellbook>();
        }
        
        spellType = SpellType.BEAM;
    }

    private void InitElementGraphics()
    {
        Debug.Log("Initialize beam element particles");
        List<ElementType> elementPrefabs = new List<ElementType>();
        foreach (SpellModifier modifier in modifiers)
        {
            if (modifier.beamElementGraphic != ElementType.Default)
            {
                if (!elementPrefabs.Contains(modifier.beamElementGraphic))
                {
                    elementPrefabs.Add(modifier.beamElementGraphic);
                }
            }
        }
        foreach (StatusEffect statusEffect in statusEffects)
        {
            if (statusEffect.beamElementGraphic != ElementType.Default)
            {
                if (!elementPrefabs.Contains(statusEffect.beamElementGraphic))
                {
                    elementPrefabs.Add(statusEffect.beamElementGraphic);
                }
            }
        }
        if(elementPrefabs.Count == 0)
        {
            Debug.Log("Beam element count is 0");
            if(DefaultParticlePrefab != null)
            {
                graphics.Add(DefaultParticlePrefab);
                DefaultParticlePrefab.SetActive(true);
            }
        }
        else
        {
            Debug.Log("Beam element count is not 0");
            foreach (ElementType elementType in elementPrefabs)
            {
                switch (elementType)
                {
                    case ElementType.Fire:
                        if (FireElementParticlePrefab != null)
                        {
                            graphics.Add(FireElementParticlePrefab);
                            FireElementParticlePrefab.SetActive(true);
                        }
                        break;
                    case ElementType.Ice:
                        if (IceElementParticlePrefab != null)
                        {
                            graphics.Add(IceElementParticlePrefab);
                            IceElementParticlePrefab.SetActive(true);
                        }
                        break;
                    case ElementType.Water:
                        if (WaterElementParticlePrefab != null)
                        {
                            graphics.Add(WaterElementParticlePrefab);
                            WaterElementParticlePrefab.SetActive(true);
                        }
                        break;
                    case ElementType.Electric:
                        if (ElectricElementParticlePrefab != null)
                        {
                            graphics.Add(ElectricElementParticlePrefab);
                            ElectricElementParticlePrefab.SetActive(true);
                        }
                        break;
                }
            }
        }
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

        Debug.DrawLine(startPos, endPos, Color.red);
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

    private void OnParticleCollision(GameObject other)
    {
        var rb = other.GetComponent<Rigidbody>();
        if(rb != null)
        {
            foreach(ParticleSystem particleSystem in beamParticles)
            {
                ParticlePhysicsExtensions.GetCollisionEvents(particleSystem, other, collisionEvents);
            }
            for (int i = 0; i < collisionEvents.Count; i++)
            {
                var health = other.GetComponent<Health>();
                if (health != null)
                {
                    base.DealDamage(health, baseDamage * Time.deltaTime);
                }

                var effectManager = other.GetComponent<StatusEffectManager>();
                if (effectManager != null)
                {
                    base.ApplyStatusEffects(effectManager, statusEffects);
                }
                
                foreach (SpellModifier modifier in modifiers)
                {
                    modifier.BeamCollide(hit, direction, distanceTravelled);
                }
            }
        }
    }

    #endregion

    #region Custom_Methods

    public override void CastSpell(Spellbook spellbook, SpellData data)
    {
        // get the look direction from spellbook and spawn new beam according to that // also child it to player to follow pos and rot
        direction = spellbook.GetDirection();
        Quaternion rot = Quaternion.LookRotation(direction, Vector3.up);
        Beam beam = Instantiate(gameObject, spellbook.spellPos.position, rot).GetComponent<Beam>();
        beam.transform.SetParent(spellbook.transform);
        beam.caster = spellbook.gameObject;
        beam.isMaster = true;
        
        // apply all spellmodifiers to the beam
        ApplyModifiers(beam.gameObject, data);

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
        if(modifiers.Length > 0)
        {
            foreach (SpellModifier modifier in modifiers)
            {
                modifier.BeamCastingEnd();
            }
        }
    }

    public void UpdateBeam(Vector3 startPosition, Vector3 direction)
    {
        foreach(GameObject graphic in graphics)
        {
            if (usingCylinder)
            {
                // position
                Vector3 offset = endPos - startPos;
                Vector3 position = startPos + (offset * 0.5f);
                graphic.transform.position = position;

                // scale
                Vector3 localScale = graphic.transform.localScale;
                localScale.y = (endPos - startPos).magnitude * 0.5f;
                graphic.transform.localScale = localScale;

                graphic.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
            }
            else
            {
                graphic.transform.position = startPos;
                graphic.transform.rotation = Quaternion.FromToRotation(Vector3.right, direction);
            }
        }
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
                    if(objectsHit[i].GetComponent<SpellTypeAmount>() != null) objectsHit[i].GetComponent<SpellTypeAmount>().beam = true; //ScoreUI
                }
            }
            else
            {
                closest = objectsHit[i].gameObject;
                if (objectsHit[i].GetComponent<SpellTypeAmount>() != null) objectsHit[i].GetComponent<SpellTypeAmount>().beam = true; //ScoreUI
            }
        }
        return closest;
    }

    // USED TO MODIFY BASE VALUES
    public void ModifyDamage(float amount)
    {
        baseDamage += amount;
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

    public override void ModifyRange(float amount)
    {
        Range += amount;
    }

    #endregion

}
