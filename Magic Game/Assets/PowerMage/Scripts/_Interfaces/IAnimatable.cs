using UnityEngine;

namespace PowerMage
{
    public interface IAnimatable
    {
        void SetLookDirection(Vector3 dir);
        void SetLookAt(Vector3 target);
        void SetMoveVelocity(Vector2 velocity);
    }
}
