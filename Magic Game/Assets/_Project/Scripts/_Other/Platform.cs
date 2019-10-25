using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Animator animator = null;
    [SerializeField] private bool isMoving = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isMoving)
        {
            animator.SetBool("IsMoving", true);
        }

        else
        {
            animator.SetBool("IsMoving", false);
        }
    }
}
