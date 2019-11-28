using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashTimer : MonoBehaviour
{
    [SerializeField] private float splashTimer = 5.0f;
    [SerializeField] private float fadeTime = 1.0f;
    [SerializeField] private Image logoImage = null;
    [SerializeField] private Text logoText = null;
    
    private float splashTimerTemp;
    private bool waiting = false;
    
    void Start()
    {
        splashTimerTemp = splashTimer;

        if (logoImage == null || logoText == null)
        {
            Debug.LogWarning(this + " Logo image/text is null! Skipping splash screen...");
            splashTimerTemp = 0.0f;
            return;
        }

        StartCoroutine(Fade(logoImage, Color.clear, Color.white, fadeTime));
        StartCoroutine(Fade(logoText, Color.clear, Color.white, fadeTime));
    }

    void Update()
    {
        if (splashTimerTemp > 0.0f)
        {
            splashTimerTemp -= Time.deltaTime;
        }
        else
        {
            if (logoImage == null || logoText == null)
            {
                GlobalVariables.loadLevel = "MainMenu";
                UnityEngine.SceneManagement.SceneManager.LoadScene("Loading");
                return;
            }

            if (!waiting)
            {
                StartCoroutine(WaitForFade());
            }
        }
    }

    IEnumerator WaitForFade()
    {
        waiting = true;

        StartCoroutine(Fade(logoImage, Color.white, Color.clear, fadeTime));
        StartCoroutine(Fade(logoText, Color.white, Color.clear, fadeTime));

        yield return new WaitForSeconds(fadeTime);

        GlobalVariables.loadLevel = "MainMenu";
        UnityEngine.SceneManagement.SceneManager.LoadScene("Loading");
    }

    IEnumerator Fade(Image image, Color start, Color end, float time)
    {
        float t = 0;
        while (image.color != end)
        {
            image.color = Color.Lerp(start, end, t / time);
            t += Time.unscaledDeltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    IEnumerator Fade(Text text, Color start, Color end, float time)
    {
        float t = 0;
        while (text.color != end)
        {
            text.color = Color.Lerp(start, end, t / time);
            t += Time.unscaledDeltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
