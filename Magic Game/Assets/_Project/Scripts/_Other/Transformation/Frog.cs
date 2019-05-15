using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Transformation
{

    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float timeBeteenJumps = 2f;

    private float jumpTimer;
    private Rigidbody rb;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
    }

    // base.Update() calls Transformations Update that runs down the timer
    protected override void Update()
    {
        base.Update();

        if(jumpTimer <= 0)
        {
            jumpTimer = timeBeteenJumps;
            Jump();
        }
        else
        {
            jumpTimer -= Time.deltaTime;
        }
    }

    private void Jump()
    {
        // jump logic
        transform.Rotate(Vector3.up, 360 * UnityEngine.Random.value);
        Vector3 direction = (transform.forward + Vector3.up).normalized;
        rb.AddForce(direction * jumpForce, ForceMode.Impulse);
    }
}