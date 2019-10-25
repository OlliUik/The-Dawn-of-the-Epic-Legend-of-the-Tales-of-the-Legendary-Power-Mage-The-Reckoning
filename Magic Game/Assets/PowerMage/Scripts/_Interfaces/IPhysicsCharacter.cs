namespace PowerMage
{
    public interface IPhysicsCharacter
    {
        UnityEngine.Vector3 GetVelocity();
        UnityEngine.Vector3 GetLookDirection();
        void Move(float moveX, float moveY, bool jump, bool dash);
        void Teleport(UnityEngine.Vector3 position);
    }
}
