using UnityEngine;

namespace PowerMage
{
    public class InverseKinematicsHandler : MonoBehaviour
    {
        [HideInInspector] public Vector3 lookAtTarget = Vector3.zero;

        private Animator animator = null;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void OnAnimatorIK(int layerIndex)
        {
            animator.SetLookAtWeight(1.0f);
            animator.SetLookAtPosition(lookAtTarget);
        }
    }
}
