//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health cHealth = null;
    [SerializeField] private Image hpBarImage = null;

    private float currentHealth = 100.0f;

    public void Initialize(Health hp, Image img)
    {
        cHealth = hp;
        hpBarImage = img;
    }

    private void Start()
    {
        if (cHealth == null)
        {
            Debug.LogWarning(this.gameObject + " has missing health component!");
        }

        if (hpBarImage == null)
        {
            Debug.LogWarning(this.gameObject + " has missing health bar image component!");
        }

        currentHealth = cHealth.maxHealth;
    }

    private void Update()
    {
        if (!Mathf.Approximately(currentHealth, cHealth.health))
        {
            hpBarImage.rectTransform.localScale = new Vector3(cHealth.health / cHealth.maxHealth, 1.0f, 1.0f);
        }
    }
}
