//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class SkipCrystalButton : MonoBehaviour
{
    private void OnEnable()
    {
        UnityEngine.UI.Button button = GetComponent<UnityEngine.UI.Button>();

        if (button != null)
        {
            if (button.interactable)
            {
                button.onClick.Invoke();
            }
        }
    }
}
