using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Animator animator = null;
    [SerializeField] private int stateNumber = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        switch (stateNumber)
        {
            case 0:
                animator.SetInteger("StateNumber", 0);
                break;
            case 1:
                animator.SetInteger("StateNumber", 1);
                break;
            case 2:
                animator.SetInteger("StateNumber", 2);
                break;
        }
    }
}