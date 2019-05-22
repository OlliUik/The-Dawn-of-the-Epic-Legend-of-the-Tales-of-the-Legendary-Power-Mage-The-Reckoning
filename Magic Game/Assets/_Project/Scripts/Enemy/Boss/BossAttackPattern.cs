using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BossAttackPattern : ScriptableObject
{
    [Header("Basic Information")]
    public string behaviourName = "Name";
    [TextArea] public string behaviourDescription = "Description";
    public SpellType spellType = SpellType.GENERIC;
    public Spell spell = null;
    public Card[] cards = null;
    public BossLizardKing.EBossPattern attackPattern = BossLizardKing.EBossPattern.PROJECTILE;

    [Header("Attacking Variables")]
    public float attackDistance = 50.0f;

    [Header("Spellcasting Variables")]
    public bool moveWhileCasting = false;
    public float standStillAfterCasting = 4.0f;
    public bool castInBursts = false;
    public float castingTime = 2.0f;
    public int burstCount = 3;
    public float timeBetweenCasts = 0.2f;
    public float castingCooldown = 4.0f;

    [Header("Navigation Variables")]
    public float minDistanceFromAttackTarget = 2.0f;
    public float walkingSpeed = 5.0f;
    public float walkingAcceleration = 8.0f;
    public float runningSpeed = 10.0f;
    public float runningAcceleration = 8.0f;
    public float panicSpeed = 12.0f;
    public float panicAcceleration = 8.0f;
}