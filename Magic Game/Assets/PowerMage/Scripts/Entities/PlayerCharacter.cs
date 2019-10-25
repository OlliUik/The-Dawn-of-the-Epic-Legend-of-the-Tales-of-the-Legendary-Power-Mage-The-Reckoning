using UnityEngine;

namespace PowerMage
{
    [RequireComponent(typeof(InputPlayer))]
    [RequireComponent(typeof(ThirdPersonCamera))]
    [RequireComponent(typeof(RagdollableModel))]
    [RequireComponent(typeof(Spellbook))]
    [AddComponentMenu("PowerMage/Player Character")]
    public class PlayerCharacter : Entity
    {
        protected override void Awake()
        {
            base.Awake();
            canRagdoll = true;
        }
    }
}
