using System.Collections.Generic;
using UnityEngine;

namespace PowerMage
{
    public abstract class Entity : MonoBehaviour
    {
        [System.Flags]
        public enum StatusEffects
        {
            None        = 0,
            OnFire      = 1,
            Confused    = 2,
            Frozen      = 4,
            Ragdolled   = 8
        }

        [SerializeField] private int health = 100;
        [SerializeField] private int mana = 100;

        protected StatusEffects effects = StatusEffects.None;
        
        public virtual bool GetEffect(StatusEffects se)
        {
            return (effects & se) == se;
        }

        public virtual void SetEffect(StatusEffects se, bool b)
        {
            if (GetEffect(se) != b)
            {
                effects = effects ^ se;
            }
        }

        public void Hurt(int amount)
        {
            health -= amount;

            if (health > 0)
            {
                OnHurt();
            }
            else
            {
                OnDeath();
            }
        }

        public void Heal(int amount)
        {
            health += amount;
        }

        public virtual void OnHurt()
        {

        }

        public virtual void OnDeath()
        {
            Destroy(gameObject);
        }
    }
}
