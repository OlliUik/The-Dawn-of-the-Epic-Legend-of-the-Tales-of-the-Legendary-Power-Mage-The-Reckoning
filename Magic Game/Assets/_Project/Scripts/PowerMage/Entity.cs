//using System.Collections.Generic;
using UnityEngine;

namespace PowerMage
{
    public abstract class Entity : MonoBehaviour
    {
        /* ----- Health and mana ----- */

        #region HEALTH
        
        public int maxHealth = 100;
        public int health { get; private set; } = 100;

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

            if (health > maxHealth)
            {
                health = maxHealth;
            }

            OnHeal();
        }

        #endregion

        #region MANA

        public int maxMana = 100;
        public int mana { get; private set; } = 100;

        public void UseMana(int amount)
        {
            mana -= amount;
            if (mana < 0)
            {
                mana = 0;
            }
        }

        #endregion

        /* ----- Status effects ----- */

        #region STATUS_EFFECTS

        [System.Flags]
        public enum StatusEffects
        {
            None = 0,
            OnFire = 1,
            Confused = 2,
            Frozen = 4,
            Ragdolled = 8
        }
        
        public StatusEffects effects { get; private set; } = StatusEffects.None;
        
        public bool GetEffect(StatusEffects se)
        {
            return (effects & se) == se;
        }

        public void SetEffect(StatusEffects se, bool b)
        {
            if (GetEffect(se) != b)
            {
                effects = effects ^ se;
            }
        }

        #endregion

        /* ----- Virtual functions ----- */

        public virtual void Awake()
        {
            health = maxHealth;
            mana = maxMana;
        }

        public virtual void OnHurt()
        {

        }

        public virtual void OnHeal()
        {

        }

        public virtual void OnDeath()
        {
            Destroy(gameObject);
        }
    }
}
