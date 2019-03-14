using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashTimer : MonoBehaviour
{
    [SerializeField] private float splashTimer = 5.0f;
    private float splashTimerTemp;

    void Start()
    {
        splashTimerTemp = splashTimer;
    }

    void Update()
    {
        if (splashTimerTemp > 0.0f)
        {
            splashTimerTemp -= Time.deltaTime;
        }
        else
        {
            GlobalVariables.loadLevel = "MainMenu";
            UnityEngine.SceneManagement.SceneManager.LoadScene("Loading");
        }
    }
}
