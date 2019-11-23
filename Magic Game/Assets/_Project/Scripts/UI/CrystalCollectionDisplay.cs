using UnityEngine;

public class CrystalCollectionDisplay : MonoBehaviour
{
    public PairObject[] pairs = null;

    private int _crystalsCollected = 0;
    public int CrystalsCollected
    {
        get
        {
            return _crystalsCollected;
        }
        set
        {
            _crystalsCollected = value;
            int i = 1;
            foreach (PairObject o in pairs)
            {
                bool b = i <= _crystalsCollected ? false : true;
                o.first.SetActive(b);
                o.second.SetActive(!b);
                i++;
            }
        }
    }

    private void Start()
    {
        CrystalsCollected = 0;
    }
}
