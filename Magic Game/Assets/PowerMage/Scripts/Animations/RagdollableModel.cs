using System.Collections.Generic;
using UnityEngine;

namespace PowerMage
{
    public class RagdollableModel : AnimatableModel
    {
        #region VARIABLES

        private bool _isKinematic = false;
        public bool isKinematic
        {
            get { return _isKinematic; }
            set
            {
                if (_isKinematic != value)
                {
                    if (rigidBodies != null)
                    {
                        foreach (Rigidbody rb in rigidBodies)
                        {
                            rb.isKinematic = value;
                        }
                    }
                    _isKinematic = value;
                }
            }
        }
        
        private Rigidbody[] rigidBodies = null;

        #endregion

        #region MONOBEHAVIOUR

        protected override void Awake()
        {
            base.Awake();
            isKinematic = false; //Initialize value
            GetRigidBodies();
            isKinematic = true;
        }

        #endregion

        #region CUSTOM_METHODS

        private void GetRigidBodies()
        {
            List<Rigidbody> rb = new List<Rigidbody>();
            foreach (Transform bone in transform.GetAllChildren())
            {
                Rigidbody boneRigid = bone.GetComponent<Rigidbody>();
                if (boneRigid != null)
                {
                    rb.Add(boneRigid);
                }
            }
            rigidBodies = rb.ToArray();
        }

        #endregion
    }
}
