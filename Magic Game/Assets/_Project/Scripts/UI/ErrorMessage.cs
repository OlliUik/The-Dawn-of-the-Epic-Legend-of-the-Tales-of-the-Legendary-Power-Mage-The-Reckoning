using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorMessage : MonoBehaviour
{
    [SerializeField] private Text errorText = null;
    [SerializeField] private Text pressAnyKeyText = null;
    [SerializeField] private float keyPressDelay = 3.0f;
    private float keyPressDelayTemp;

    void Start()
    {
        keyPressDelayTemp = keyPressDelay;
        errorText.text = "Uh oh! Something went wrong!\n\nError code: " + GlobalVariables.errorCode;
    }

    void Update()
    {
        if (keyPressDelayTemp > 0.0f)
        {
            keyPressDelayTemp -= Time.deltaTime;
        }
        else
        {
            pressAnyKeyText.text = "Press any key to quit...";

            if (Input.anyKeyDown)
            {
                Application.Quit();
            }
        }
    }
}
