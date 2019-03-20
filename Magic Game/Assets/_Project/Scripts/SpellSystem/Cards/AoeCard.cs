using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AoeCard", menuName = "Card/Aoe")]
public class AoeCard : Card
{
    public List<GameObject> spellModifiers = new List<GameObject>();
}
