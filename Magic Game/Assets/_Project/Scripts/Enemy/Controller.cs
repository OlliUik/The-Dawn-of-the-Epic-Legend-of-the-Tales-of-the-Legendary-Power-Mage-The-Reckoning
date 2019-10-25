using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class Controller : MonoBehaviour
{

    public NavMeshAgent agent;

    public ThirdPersonCharacter character;

    //public Rigidbody rb;

    bool isGrounded;


    private void Start()
    {
        //agent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {   

        if (agent.remainingDistance > agent.stoppingDistance)
        {
            character.Move(agent.desiredVelocity, false, false);
        }
        else
        {
            character.Move(Vector3.zero, false, false);
        }


        //Debug.Log(isGrounded.ToString());
        //if (agent.isOnOffMeshLink && isGrounded)
        //{
            //Debug.Log("is On Off Mesh Link");
            //Jump();
            //agent.updatePosition = true;

        //}

    }
}


