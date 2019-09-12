public class TestScript : PowerMage.Entity
{
    public override void Awake()
    {
        base.Awake();

        UnityEngine.Debug.Log("Confused is " + GetEffect(StatusEffects.Confused));
        SetEffect(StatusEffects.Confused, false);
        UnityEngine.Debug.Log("Confused is " + GetEffect(StatusEffects.Confused));
        SetEffect(StatusEffects.Confused, true);
        UnityEngine.Debug.Log("Confused is " + GetEffect(StatusEffects.Confused));

        UnityEngine.Debug.Log("Health is: " + health);
    }
}
