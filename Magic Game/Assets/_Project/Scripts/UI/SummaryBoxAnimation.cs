using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryBoxAnimation : MonoBehaviour, AnimationElement
{
    [SerializeField] private GameObject[] fadedGroup = null;

    // Start is called before the first frame update
    void Start()
    {
        FadeOutAllCanvas();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Animator());
    }

    void FadeOutAllCanvas()
    {
        foreach (GameObject g in fadedGroup)
        {
            g.GetComponent<CanvasGroup>().alpha = 0;
        }
    }

    void FadeInCanvas(CanvasGroup cg)
    {
        float cAlpha = cg.alpha;
        float nAlpha = Mathf.Lerp(cAlpha, 1f, Time.deltaTime);

        cg.alpha = nAlpha;
    }

    public IEnumerator Animator()
    {
        foreach (GameObject g in fadedGroup)
        {
            yield return new WaitForSeconds(0.8f);
            FadeInCanvas(g.GetComponent<CanvasGroup>());

            AnimationElement ae = g.GetComponentInChildren<AnimationElement>();

            if (ae != null)
            {
                yield return StartCoroutine(ae.Animator());
            }
        }
    }
}
