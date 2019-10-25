using UnityEngine;

namespace PowerMage
{
    public interface IAnimatable
    {
        void SetLookAt(Vector3 target);
        void SetMovement(Vector2 velocity);
    }
}
