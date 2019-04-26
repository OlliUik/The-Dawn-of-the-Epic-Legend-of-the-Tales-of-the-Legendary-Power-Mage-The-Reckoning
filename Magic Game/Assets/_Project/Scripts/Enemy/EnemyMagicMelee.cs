using UnityEngine;

[RequireComponent(typeof(Spellbook))]
public class EnemyMagicMelee : EnemyMelee
{
    public Spellbook cSpellBook { get; private set; } = null;

    [Header("Magic -> Attacking")]
    //Ranged / Magic Ranged / Magic Melee
    [SerializeField] protected bool castInBursts = false;
    [SerializeField] protected float castingTime = 2.0f;
    [SerializeField] protected int burstCount = 3;
    [SerializeField] protected float timeBetweenCasts = 0.2f;

    protected override void Start()
    {
        base.Start();
        cSpellBook = GetComponent<Spellbook>();
    }

    protected override void Update()
    {
        base.Update();

        //shootIntervalTimer -= shootIntervalTimer > 0.0f ? time : 0.0f;
        //castingTimer -= castingTimer > 0.0f ? time : 0.0f;

        //if (castingTimer <= 0.0f)
        //{
        //}
    }
}
