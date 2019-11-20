using UnityEngine;

[System.Serializable]
public struct PairObject
{
    public GameObject first, second;

    public PairObject(GameObject f, GameObject s)
    {
        first = f;
        second = s;
    }
}
