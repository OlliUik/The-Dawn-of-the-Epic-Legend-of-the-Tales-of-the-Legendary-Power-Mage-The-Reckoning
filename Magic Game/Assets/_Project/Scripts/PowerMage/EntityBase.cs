//using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PowerMage
{
    [RequireComponent(typeof(CharacterController))]
    [DisallowMultipleComponent]
    public abstract class EntityBase : MonoBehaviour
    {
        #region HEALTH

        public int health { get; private set; } = 100;
        public int maxHealth = 100;

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

        public int mana { get; private set; } = 100;
        public int maxMana = 100;

        public void UseMana(int amount)
        {
            mana -= amount;

            if (mana < 0)
            {
                mana = 0;
            }

            OnUseMana();
        }

        #endregion

        #region TEAM

        [SerializeField] protected Teams.Team team = Teams.Team.NONE;
        private Color teamColor = Color.gray;

        #endregion

        #region STATUS_EFFECTS

        [System.Flags]
        public enum StatusEffects
        {
            None = 0,
            SetOnFire = 1,
            Confused = 2,
            Frozen = 4,
            Ragdolled = 8
        }
        
        private StatusEffects effects = StatusEffects.None;
        
        public bool GetEffect(StatusEffects se)
        {
            return (effects & se) == se;
        }

        public void SetEffect(StatusEffects se, bool b)
        {
            if (GetEffect(se) != b)
            {
                effects = effects ^ se;

                switch (se)
                {
                    case StatusEffects.SetOnFire: OnSetOnFire(); break;
                    case StatusEffects.Confused: OnConfused(); break;
                    case StatusEffects.Frozen: OnFrozen(); break;
                    case StatusEffects.Ragdolled: OnRagdolled(); break;
                    default: break;
                }
            }
        }

        #endregion

        #region MOVEMENT

        private Vector3 velocity = Vector3.zero;

        #endregion

        #region VIEW_POINT

        [SerializeField] private Vector3 viewPivot = Vector3.zero;
        private Vector3 viewLookAt = Vector3.forward;

        #endregion

        #region CALLBACKS

        //These should be implemented in child classes, but aren't necessary.

        protected virtual void OnHurt() { }
        protected virtual void OnHeal() { }
        protected virtual void OnUseMana() { }

        protected virtual void OnSetOnFire() { }

        protected virtual void OnConfused()
        {
            Teams.RemoveFromTeam(this, team);
            Teams.Team newTeam;
            switch (team)
            {
                case Teams.Team.GOODGUYS: newTeam = Teams.Team.BADBOYS; break;
                case Teams.Team.BADBOYS: newTeam = Teams.Team.GOODGUYS; break;
                default: newTeam = Teams.Team.NONE; break;
            }
            Teams.AddToTeam(this, newTeam);
            team = newTeam;
        }

        protected virtual void OnFrozen() { }
        protected virtual void OnRagdolled() { }

        protected virtual void OnDeath()
        {
            Teams.RemoveFromTeam(this, team);
            Destroy(gameObject);
        }

        #endregion

        private void UpdateTeamColor()
        {
            switch (team)
            {
                case Teams.Team.BADBOYS: teamColor = Color.red; break;
                case Teams.Team.GOODGUYS: teamColor = Color.green; break;
                default: teamColor = Color.gray; break;
            }
        }

        public virtual void Awake()
        {
            health = maxHealth;
            mana = maxMana;
            Teams.AddToTeam(this, team);
        }

        public virtual void OnValidate()
        {
            UpdateTeamColor();
        }

        public virtual void OnDrawGizmos()
        {
            #if UNITY_EDITOR
            
            if (Selection.activeGameObject != gameObject)
            {
                //Draw gizmos when NOT selected

                //Draw a faded circle at the pivot point
                Handles.color = Color.Lerp(teamColor, Color.clear, 0.9f);
                Handles.DrawSolidDisc(transform.position, transform.up, 1.0f);

                Handles.color = Color.Lerp(teamColor, Color.clear, 0.3f);
                Handles.DrawWireDisc(transform.position, transform.up, 1.0f);
            }
            else
            {
                //Draw gizmos when selected

                //Draw a circle at the pivot point
                Handles.color = Color.Lerp(teamColor, Color.clear, 0.8f);
                Handles.DrawSolidDisc(transform.position, transform.up, 1.0f);

                Handles.color = Color.Lerp(teamColor, Color.clear, 0.0f);
                Handles.DrawWireDisc(transform.position, transform.up, 1.0f);

                //Draw camera's pivot point
                Gizmos.color = Color.magenta;
                Gizmos.DrawCube(transform.position + viewPivot, Vector3.one * 0.2f);
            }

            #endif
        }
    }
}
