using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevitationObject : MonoBehaviour
{

    public static GameObject holdingObject;

    public static GameObject lineParticle;
    public static GameObject holdingParticlePrefab;
    static GameObject holdingParticle;
    NavMeshAgent navMeshAgent;
    bool originalIsKinematic = false;
    bool originalUseGravity = false;

    void Start()
    {
        StartCoroutine(destroySelf());
    }

    IEnumerator destroySelf()
    {
        yield return new WaitForSeconds(0.1f);
        if(holdingObject == null)
        {
            Destroy(gameObject);
        }
        yield return new WaitForSeconds(4.9f);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (holdingObject != null)
        {
            if (navMeshAgent != null)
            {
                //TODO: Enable agent
                navMeshAgent.enabled = true;
            }
            holdingObject.GetComponent<Rigidbody>().isKinematic = originalIsKinematic;
            holdingObject.GetComponent<Rigidbody>().useGravity = originalUseGravity;
        }
        Destroy(lineParticle);
        Destroy(holdingParticle);
        holdingObject = null;
        //Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(holdingObject != null)
        {
            AddForceWithMultiplier(2);
            if(navMeshAgent != null)
            {
                //navMeshAgent.velocity = holdingObject.GetComponent<Rigidbody>().velocity;
                AddForceWithMultiplier(1000);
            }
            //holdingObject.GetComponent<Rigidbody>().MovePosition(transform.position);
            //holdingObject.transform.position = transform.position;
            if(holdingParticle != null)
            {
                holdingParticle.transform.position = holdingObject.transform.position;
                holdingParticle.transform.rotation = new Quaternion();
            }
        }
    }

    void AddForceWithMultiplier(float multiplier)
    {
        Rigidbody targetRb = holdingObject.GetComponent<Rigidbody>();
        Vector3 targetPosition = holdingObject.transform.position;
        float errorNumber = 0.2f;
        float reducingVelocityFactor = 0.8f;

        targetRb.AddForce((transform.position - targetPosition) * multiplier);
        if(transform.position.x - targetPosition.x < errorNumber && transform.position.x - targetPosition.x > -errorNumber)
        {
            targetRb.velocity = new Vector3(targetRb.velocity.x * reducingVelocityFactor, targetRb.velocity.y, targetRb.velocity.z);
        }
        if (transform.position.y - targetPosition.y < errorNumber && transform.position.y - targetPosition.y > -errorNumber)
        {
            targetRb.velocity = new Vector3(targetRb.velocity.x, targetRb.velocity.y * reducingVelocityFactor, targetRb.velocity.z);
        }
        if (transform.position.z - targetPosition.z < errorNumber && transform.position.z - targetPosition.z > -errorNumber)
        {
            targetRb.velocity = new Vector3(targetRb.velocity.x, targetRb.velocity.y, targetRb.velocity.z * reducingVelocityFactor);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(holdingObject == null && collision.gameObject.GetComponent<Rigidbody>() != null)
        {
            holdingObject = collision.gameObject;
            if (holdingObject.transform.parent != null)
            {
                if (holdingObject.transform.root.gameObject.GetComponent<Rigidbody>())
                {
                    holdingObject = collision.gameObject.transform.root.gameObject;
                }
            }
            /*
            if(collision.gameObject.transform.parent == null)
            {
                holdingObject = collision.gameObject;
            }
            else
            {
                holdingObject = collision.gameObject.transform.root.gameObject;
            }
            */
            if (holdingObject.GetComponent<NavMeshAgent>())
            {
                //TODO: Disable agent
                navMeshAgent = holdingObject.GetComponent<NavMeshAgent>();
                navMeshAgent.enabled = false;
            }
            originalIsKinematic = holdingObject.GetComponent<Rigidbody>().isKinematic;
            originalUseGravity = holdingObject.GetComponent<Rigidbody>().useGravity;
            holdingObject.GetComponent<Rigidbody>().isKinematic = false;
            holdingObject.GetComponent<Rigidbody>().useGravity = false;
            holdingParticle = Instantiate(holdingParticlePrefab, holdingObject.transform.position, holdingObject.transform.rotation);
        }
    }

    public GameObject GetHoldingObject()
    {
        return holdingObject;
    }

}
