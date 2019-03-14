using UnityEngine;
using UnityEngine.UI;

public class HurtFlash : MonoBehaviour
{
    [SerializeField] private Image hurtFlashObject = null;
    private float reduceAmount = 0.5f;
    private float maxAlpha = 0.2f;

    // Update is called once per frame
    void Update()
    {
        if (hurtFlashObject.color.a > 0.0f)
        {
            hurtFlashObject.color = new Color(1.0f, 0.0f, 0.0f, hurtFlashObject.color.a - reduceAmount * Time.deltaTime);
        }
        else
        {
            this.enabled = false;
        }
    }

    public void Flash()
    {
        this.enabled = true;
        hurtFlashObject.color = new Color(1.0f, 0.0f, 0.0f, maxAlpha);
    }
}
