using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PowerMage
{
    //[RequireComponent(typeof(CharacterController))]
    [DisallowMultipleComponent]
    public abstract class Entity : MonoBehaviour
    {
        #region HEALTH

        public float health { get; private set; } = 100.0f;
        public float maxHealth = 100.0f;
        public bool allowHealthRegen = false;
        public float healthRegenAmount = 5.0f;
        public float healthRegenCooldown = 2.0f;
        public float healthRegenInterval = 1.0f;
        
        public void Hurt(float amount)
        {
            health -= Mathf.Abs(amount);

            if (health > 0)
            {
                OnHurt();
            }
            else
            {
                OnDeath();
            }
        }

        public void Heal(float amount)
        {
            RegenerateHealth(amount);
            OnHeal();
        }

        private void RegenerateHealth(float amount)
        {
            health += Mathf.Abs(amount);

            if (health > maxHealth)
            {
                health = maxHealth;
            }
        }

        /* ----- CALLBACKS ----- */

        protected virtual void OnHurt() { }
        protected virtual void OnHeal() { }

        protected virtual void OnDeath()
        {
            Teams.RemoveFromTeam(this, team);
            Destroy(gameObject);
        }

        /* ----- COROUTINES ----- */

        private System.Collections.IEnumerator RegenerateHealth()
        {
            while (true)
            {
                if (allowHealthRegen)
                {
                    RegenerateHealth(healthRegenAmount * healthRegenInterval);
                    yield return new WaitForSeconds(healthRegenInterval);
                }
            }
        }

        #endregion

        #region MANA

        public float mana { get; private set; } = 100.0f;
        public float maxMana = 100.0f;
        public bool allowManaRegen = true;
        public float manaRegenAmount = 10.0f;
        public float manaRegenCooldown = 2.0f;
        public float manaRegenInterval = 0.2f;
        
        public void UseMana(float amount)
        {
            mana -= amount;

            if (mana < 0)
            {
                mana = 0;
            }

            OnUseMana();
        }

        public void GiveMana(float amount)
        {
            RegenerateMana(amount);
            OnGiveMana();
        }

        private void RegenerateMana(float amount)
        {
            mana += amount;

            if (mana > maxMana)
            {
                mana = maxMana;
            }
        }

        /* ----- CALLBACKS ----- */

        protected virtual void OnUseMana() { }
        protected virtual void OnGiveMana() { }

        /* ----- COROUTINES ----- */

        private System.Collections.IEnumerator RegenerateMana()
        {
            while (true)
            {
                if (allowManaRegen)
                {
                    RegenerateMana(manaRegenAmount * manaRegenInterval);
                    yield return new WaitForSeconds(manaRegenInterval);
                }
            }
        }

        #endregion

        #region TEAM

        [SerializeField] protected Teams.Team team = Teams.Team.NONE;
        private Color teamColor = Color.gray;

        private void UpdateTeamColor()
        {
            switch (team)
            {
                case Teams.Team.BADBOYS: teamColor = Color.red; break;
                case Teams.Team.GOODGUYS: teamColor = Color.green; break;
                default: teamColor = Color.gray; break;
            }
        }

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

        /* ----- CALLBACKS ----- */

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

        #endregion

        #region INPUT

        public Vector2 inputMove
        {
            get { return inputMove; }
            set { inputMove = value.normalized; }
        }

        public Vector2 inputLook
        {
            get { return inputLook; }
            set { inputLook = value.normalized; }
        }

        public bool inputJump = false;
        public bool inputDash = false;
        public bool inputAttack = false;

        #endregion

        #region PHYSICS

        private CharacterController controller;

        private void GetCharacterController()
        {
            if (controller == null)
            {
                controller = GetComponent<CharacterController>();

                //CharacterController hasn't been added yet
                if (controller == null)
                {
                    controller = gameObject.AddComponent<CharacterController>();
                    controller.center = Vector3.up;
                }
            }
        }
        
        #endregion

        #region VIEW_AND_CAMERA

        [SerializeField] private Vector3 viewPivot = Vector3.zero;
        private Vector3 viewLookAt = Vector3.forward;

        #endregion

        #region MONOBEHAVIOUR

        public virtual void Awake()
        {
            health = maxHealth;
            mana = maxMana;
            GetCharacterController();
            Teams.AddToTeam(this, team);
        }

        public virtual void Start()
        {
            StartCoroutine("RegenerateHealth");
            StartCoroutine("RegenerateMana");
        }

        public virtual void OnValidate()
        {
            GetCharacterController();
            UpdateTeamColor();
        }

        public virtual void OnDrawGizmos()
        {
#if UNITY_EDITOR

            if (controller == null)
            {
                return;
            }

            if (Selection.activeGameObject != gameObject)
            {
                //Draw when NOT selected

                //Draw a faded circle at the pivot point
                Handles.color = Color.Lerp(teamColor, Color.clear, 0.9f);
                Handles.DrawSolidDisc(transform.position, transform.up, controller.radius);

                Handles.color = Color.Lerp(teamColor, Color.clear, 0.3f);
                Handles.DrawWireDisc(transform.position, transform.up, controller.radius);

                Gizmos.color = Handles.color;
                Gizmos.DrawCube(transform.position, Vector3.up * 0.01f + (Vector3.one - Vector3.up) * 0.15f);
            }
            else
            {
                //Draw when selected

                //Draw a circle at the pivot point

                Handles.color = Color.Lerp(teamColor, Color.clear, 0.8f);
                Handles.DrawSolidDisc(transform.position, transform.up, controller.radius);

                Handles.color = Color.Lerp(teamColor, Color.clear, 0.0f);
                Handles.DrawWireDisc(transform.position, transform.up, controller.radius);

                Gizmos.color = Handles.color;
                Gizmos.DrawCube(transform.position, Vector3.up * 0.01f + (Vector3.one - Vector3.up) * 0.15f);

                //Draw camera's pivot point

                Gizmos.color = Color.magenta;
                Gizmos.DrawCube(transform.position + viewPivot, Vector3.one * 0.2f);
                
                /*

                //Draw movement input

                Camera cam = Camera.current;

                Handles.color = Color.Lerp(Color.black, Color.clear, 0.8f);
                Handles.DrawSolidDisc(
                    cam.ScreenToWorldPoint(new Vector3(100.0f, 100.0f, cam.nearClipPlane + 0.01f), Camera.MonoOrStereoscopicEye.Mono),
                    -cam.transform.forward,
                    0.15f * (cam.nearClipPlane + 0.01f)
                    );
                
                Handles.color = Color.Lerp(Color.black, Color.clear, 0.3f);
                Handles.DrawWireDisc(
                    cam.ScreenToWorldPoint(new Vector3(100.0f, 100.0f, cam.nearClipPlane + 0.01f), Camera.MonoOrStereoscopicEye.Mono),
                    -cam.transform.forward,
                    0.15f * (cam.nearClipPlane + 0.01f)
                    );

                Handles.color = Color.Lerp(Color.red, Color.clear, 0.3f);
                Handles.DrawSolidDisc(
                    cam.ScreenToWorldPoint(new Vector3(100.0f, 100.0f, cam.nearClipPlane + 0.01f), Camera.MonoOrStereoscopicEye.Mono),
                    -cam.transform.forward,
                    0.01f * (cam.nearClipPlane + 0.01f)
                    );

                //Draw look input

                Handles.color = Color.Lerp(Color.black, Color.clear, 0.8f);
                Handles.DrawSolidDisc(
                    cam.ScreenToWorldPoint(new Vector3(300.0f, 100.0f, cam.nearClipPlane + 0.01f), Camera.MonoOrStereoscopicEye.Mono),
                    -cam.transform.forward,
                    0.15f * (cam.nearClipPlane + 0.01f)
                    );

                Handles.color = Color.Lerp(Color.black, Color.clear, 0.3f);
                Handles.DrawWireDisc(
                    cam.ScreenToWorldPoint(new Vector3(300.0f, 100.0f, cam.nearClipPlane + 0.01f), Camera.MonoOrStereoscopicEye.Mono),
                    -cam.transform.forward,
                    0.15f * (cam.nearClipPlane + 0.01f)
                    );

                Handles.color = Color.Lerp(Color.red, Color.clear, 0.3f);
                Handles.DrawSolidDisc(
                    cam.ScreenToWorldPoint(new Vector3(300.0f, 100.0f, cam.nearClipPlane + 0.01f), Camera.MonoOrStereoscopicEye.Mono),
                    -cam.transform.forward,
                    0.01f * (cam.nearClipPlane + 0.01f)
                    );

                //Draw other input
                Handles.color = Color.Lerp(Color.green, Color.clear, inputJump ? 0.3f : 0.8f);
                Handles.DrawSolidDisc(
                    cam.ScreenToWorldPoint(new Vector3(50.0f, 220.0f, cam.nearClipPlane + 0.01f), Camera.MonoOrStereoscopicEye.Mono),
                    -cam.transform.forward,
                    0.05f * (cam.nearClipPlane + 0.01f)
                    );

                Handles.color = Color.Lerp(Color.yellow, Color.clear, inputDash ? 0.3f : 0.8f);
                Handles.DrawSolidDisc(
                    cam.ScreenToWorldPoint(new Vector3(150.0f, 220.0f, cam.nearClipPlane + 0.01f), Camera.MonoOrStereoscopicEye.Mono),
                    -cam.transform.forward,
                    0.05f * (cam.nearClipPlane + 0.01f)
                    );

                Handles.color = Color.Lerp(Color.red, Color.clear, inputAttack ? 0.3f : 0.8f);
                Handles.DrawSolidDisc(
                    cam.ScreenToWorldPoint(new Vector3(250.0f, 220.0f, cam.nearClipPlane + 0.01f), Camera.MonoOrStereoscopicEye.Mono),
                    -cam.transform.forward,
                    0.05f * (cam.nearClipPlane + 0.01f)
                    );
                    
             */
            }
            
#endif
        }

        #endregion
    }
}
