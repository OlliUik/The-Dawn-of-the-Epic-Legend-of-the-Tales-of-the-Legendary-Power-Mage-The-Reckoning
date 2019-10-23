using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

namespace PowerMage
{
    public class RegenResource : MonoBehaviour, IResource
    {
        [SerializeField] private float maxResourceAmount = 100.0f;
        private float resourceAmount = 100.0f;
        
        [SerializeField] private float regenAmount = 2.0f;
        [SerializeField] private float regenRate = 1.0f;
        [SerializeField] private float regenHaltDuration = 3.0f;
        
        protected Entity entity { get; private set; } = null;

        public virtual void Initialize(Entity e)
        {
            entity = e;
            resourceAmount = maxResourceAmount;
        }

        public virtual void Heal(float amount)
        {
            throw new System.NotImplementedException();
        }

        public virtual void Hurt(float amount)
        {
            throw new System.NotImplementedException();
        }

        public virtual void Burn(float amount, float seconds, float rate)
        {
            throw new System.NotImplementedException();
        }

        private IEnumerator BurnCoroutine(float amount, float seconds, float rate)
        {
            for (float i = seconds; i > 0.0; i -= rate)
            {
                Hurt(amount);
                yield return new WaitForSeconds(rate);
            }
        }
    }
}
