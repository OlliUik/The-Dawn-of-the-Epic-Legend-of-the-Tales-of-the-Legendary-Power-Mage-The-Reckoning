using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Haste : OnSelfModifier
{

    [SerializeField] private float speedIncreaseAmount = 0.2f;
    private Spellbook spellbook;

    public override void Apply(GameObject go)
    {
        go.AddComponent<Haste>();
    }

    private void Start()
    {
        spellbook = GetComponentInParent<Spellbook>();
        spellbook.playerCore.cMovement.accelerationMultiplier += speedIncreaseAmount;
        print("INCREASE SPEED");
    }

    private void OnDestroy()
    {
        spellbook.playerCore.cMovement.accelerationMultiplier -= speedIncreaseAmount;
        print("DECREASE SPEED");
    }

}
