//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseCrystalButtonText : MonoBehaviour
{
    public SpellEditorController editor = null;

    private void OnEnable()
    {
        if (editor != null) 
        {
            if (editor.crystalsLeft <= 0)
            {
                GetComponent<Text>().text = "No crystals left!";
            }
            else 
            {
                GetComponent<Text>().text = "Consume crystal\n(" + editor.crystalsLeft + " left)";
            }
        }
    }
}
