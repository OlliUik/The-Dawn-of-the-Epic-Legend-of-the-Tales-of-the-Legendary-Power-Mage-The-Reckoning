using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationChanger : MonoBehaviour
{
    [SerializeField] private Animator anim = null;
    [SerializeField] private int stateNumber = 0;

    private void Start()
    {
        anim.SetInteger("StateNumber", stateNumber);
    }
}
