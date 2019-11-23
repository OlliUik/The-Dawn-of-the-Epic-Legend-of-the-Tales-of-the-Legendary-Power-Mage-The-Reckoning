using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOverScaler : MonoBehaviour
{
    private void OnMouseEnter()
    {
        if (GetComponent<UnityEngine.UI.Button>().interactable)
        {
            StartCoroutine(Animation(Vector3.one * 1.1f));
        }
    }

    private void OnMouseExit()
    {
        StartCoroutine(Animation(Vector3.one));
    }

    IEnumerator Animation(Vector3 endScl)
    {
        float percent = 0.0f;
        float time = 0.0f;
        float effectTime = 1.0f;

        Vector3 startScale = transform.localScale;
        Vector3 endScale = endScl;

        while (percent < 100.0f)
        {
            percent = Mathf.Clamp01(time / effectTime);
            transform.localScale = Vector3.Lerp(startScale, endScale, percent);
            yield return new WaitForEndOfFrame();
        }
        transform.localScale = endScale;
    }
}
