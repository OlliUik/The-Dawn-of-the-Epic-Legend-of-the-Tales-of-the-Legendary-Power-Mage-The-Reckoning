using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellIconSwitcher : MonoBehaviour
{
    [SerializeField] private Image[] slots;

    // Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    //void Update()
    //{

    //}

    public void ChangeIcon(int iconNumber)
    {
        Color fadedColor = new Color(1.0f, 1.0f, 1.0f, 0.2f);

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].color = i == iconNumber ? Color.white : fadedColor;

            foreach (Transform item in slots[i].transform)
            {
                item.GetComponent<Image>().color = i == iconNumber ? Color.white : fadedColor;
            }
        }
    }
}
