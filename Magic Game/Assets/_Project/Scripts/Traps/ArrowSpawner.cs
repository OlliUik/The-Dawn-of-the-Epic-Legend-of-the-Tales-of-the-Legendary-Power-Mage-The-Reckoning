using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    public Rigidbody arrow;

    Transform spawnPoint;

    [SerializeField, Range(1.0f, 20f)]
    private float forceMultiplier;

    [SerializeField, Range(1.0f, 5.0f)]
    private float setTimer;

    [SerializeField]
    private float timer;

    [SerializeField]
    private Vector3 direction;

    void Start()
    {
        spawnPoint = GetComponent<Transform>();
        timer = setTimer;
        arrow.position = spawnPoint.position;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer > 0)
        {
            arrow.AddForceAtPosition(direction * forceMultiplier, arrow.transform.position);    //adds force to the arrow
        }

        if (timer < 0)
        {
            arrow.gameObject.SetActive(true);
            arrow.position = spawnPoint.position;   //arrow's position is set to original position
            arrow.velocity = Vector3.zero;          //arrow's velocity is set to zero

            timer = setTimer;
        }
    }

    public void CollisionDetected(ArrowScript arrowScript)
    {
        arrow.gameObject.SetActive(false);
    }
}