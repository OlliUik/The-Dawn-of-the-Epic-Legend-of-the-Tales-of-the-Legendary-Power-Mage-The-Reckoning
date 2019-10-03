using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomFPSCounter : MonoBehaviour
{
    public string formatedString = "{value} FPS";
    public float updateRate = 4.0f;

    public Text fpsText = null;

    private int frameCount = 0;
    private float dt = 0.0f;
    private float fps = 0.0f;

    private void Update()
    {
        frameCount++;
        dt += Time.unscaledDeltaTime;

        if (dt > 1.0f / updateRate)
        {
            fps = frameCount / dt;
            frameCount = 0;
            dt -= 1.0f / updateRate;
        }

        fpsText.text = formatedString.Replace("{value}", System.Math.Round(fps, 1).ToString("0.0"));
    }
}
