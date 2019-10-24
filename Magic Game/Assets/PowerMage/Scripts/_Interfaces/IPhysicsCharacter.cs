namespace PowerMage
{
    public interface IPhysicsCharacter
    {
        void Move(float moveX, float moveY, bool jump, bool dash);
        void Teleport(UnityEngine.Vector3 position);
    }
}
