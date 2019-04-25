using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VortexGenerator : MonoBehaviour
{
    private enum EAffectedEntities
    {
        BADBOYS,
        GOODGUYS,
        BOTH
    };

    [SerializeField] private EAffectedEntities affectedEntities = EAffectedEntities.BOTH;
    [SerializeField] private bool pullEntities = true;
    [SerializeField] private float ragdollInterval = 1.0f;
    [SerializeField] private float radius = 20.0f;
    [SerializeField] private float pullStrength = 400.0f;
    [SerializeField] private float explosionStrength = 100.0f;
    [SerializeField] private float explosionMaxVelocity = 30.0f;

    //True if explosion has happened
    private bool pullEntitiesExplosion = false;

    private List<GameObject> listOfObjects = new List<GameObject>();
    private List<GameObject> excludeObjects = new List<GameObject>();

    void Start()
    {
        InvokeRepeating("SearchForRagdollables", 0.0f, ragdollInterval);
    }

    void FixedUpdate()
    {
        if (pullEntities || !pullEntitiesExplosion)
        {
            foreach (GameObject entity in listOfObjects)
            {
                Rigidbody rigid;
                if (entity.tag == "Player")
                {
                    rigid = entity.GetComponent<PlayerCore>().ragdollPosition.GetComponent<Rigidbody>();
                }
                else
                {
                    rigid = entity.GetComponent<EnemyCore>().ragdollPosition.GetComponent<Rigidbody>();
                }

                Vector3 difference = transform.position - rigid.transform.position;
                if (pullEntities)
                {
                    if (difference.sqrMagnitude < radius * radius)
                    {
                        rigid.velocity = difference.normalized * (pullStrength * difference.magnitude + 5.0f) * Time.fixedDeltaTime;
                    }
                }
                else if (!pullEntitiesExplosion)
                {
                    rigid.velocity = Vector3.ClampMagnitude(-difference.normalized * explosionStrength / difference.magnitude, explosionMaxVelocity);
                }
            }

            if (!pullEntities && !pullEntitiesExplosion)
            {
                pullEntitiesExplosion = true;
            }
            else if (pullEntities)
            {
                pullEntitiesExplosion = false;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void ExcludeObject(GameObject go)
    {
        excludeObjects.Add(go);
    }

    void SearchForRagdollables()
    {
        if (pullEntities && !pullEntitiesExplosion)
        {
            if (affectedEntities == EAffectedEntities.BADBOYS || affectedEntities == EAffectedEntities.BOTH)
            {
                foreach (GameObject entity in GlobalVariables.teamBadBoys)
                {
                    if (excludeObjects.Contains(entity))
                    {
                        return;
                    }

                    if ((transform.position - entity.transform.position).sqrMagnitude < radius * radius)
                    {
                        if (!listOfObjects.Contains(entity))
                        {
                            listOfObjects.Add(entity);
                        }

                        if (entity.tag == "Player")
                        {
                            entity.GetComponent<PlayerCore>().EnableRagdoll(true);
                        }
                        else
                        {
                            entity.GetComponent<EnemyCore>().EnableRagdoll(true);
                        }
                    }
                }
            }
            if (affectedEntities == EAffectedEntities.GOODGUYS || affectedEntities == EAffectedEntities.BOTH)
            {
                foreach (GameObject entity in GlobalVariables.teamGoodGuys)
                {
                    if (excludeObjects.Contains(entity))
                    {
                        return;
                    }

                    if ((transform.position - entity.transform.position).sqrMagnitude < radius * radius)
                    {
                        if (!listOfObjects.Contains(entity))
                        {
                            listOfObjects.Add(entity);
                        }

                        if (entity.tag == "Player")
                        {
                            entity.GetComponent<PlayerCore>().EnableRagdoll(true);
                        }
                        else
                        {
                            entity.GetComponent<EnemyCore>().EnableRagdoll(true);
                        }
                    }
                }
            }
        }
    }
}
