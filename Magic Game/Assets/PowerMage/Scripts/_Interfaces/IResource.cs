namespace PowerMage
{
    public interface IResource
    {
        void Heal(float amount);
        void Hurt(float amount);
        void Burn(float amount, float seconds, float rate);
        void Initialize(Entity e);
    }
}
