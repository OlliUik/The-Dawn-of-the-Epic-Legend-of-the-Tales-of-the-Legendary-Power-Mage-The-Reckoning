using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour
{
    public float maxReduction = 0.0f;
    public float maxIncrease = 0.0f;
    public float rateDamping = 0.0f;
    public float strength = 0.0f;
    public bool stopFlickering = false;
    public bool controlEmission = false;

    private Light lightSource = null;
    private Material lightMaterial = null;
    private Renderer rend = null;
    private Color color = Color.clear;

    private float defaultIntensity = 0.0f;
    private float defaultRange = 0.0f;
    private bool isFlickering = false;

    private const string EMISSION_COLOR = "_EmissionColor";

    private void Reset()
    {
        maxReduction = 0.2f;
        maxIncrease = 0.2f;
        rateDamping = 0.1f;
        strength = 300;
    }

    private void OnEnable()
    {
        lightSource = GetComponent<Light>();

        if (controlEmission)
        {
            rend = GetComponentInParent<Renderer>();
            lightMaterial = rend.material;
            color = lightMaterial.GetColor(EMISSION_COLOR);
        }

        if (lightSource == null)
        {
            Debug.LogError("LightFlicker-script needs light source.");
            return;
        }

        defaultIntensity = lightSource.intensity;
        defaultRange = lightSource.range;

        /*

        //Can't be bothered to go through every light in every prefab/scene,
        //so I'll just disable this part to improve performance.

        if (controlEmission)
        {
            StartCoroutine(DoFlicker());
        }

        else
        {
            StartCoroutine(DoFlickerWithoutEmission());
        }

        */
    }

    /*
    
    //Commented out to save on performance.

    private void Update()
    {
        if (!stopFlickering && !isFlickering)
        {
            if (controlEmission)
            {
                StartCoroutine(DoFlicker());
            }

            else
            {
                StartCoroutine(DoFlickerWithoutEmission());
            }
        }
    }
    */

    private IEnumerator DoFlicker()
    {
        isFlickering = true;

        while (!stopFlickering)
        {
            lightMaterial.SetColor(EMISSION_COLOR, color * Mathf.Lerp(0.25f, 10, Time.deltaTime));

            lightSource.intensity = Mathf.Lerp(lightSource.intensity, Random.Range(defaultIntensity - maxReduction, defaultIntensity + maxIncrease), strength * Time.deltaTime);
            lightSource.range = Mathf.Lerp(lightSource.range, Random.Range(defaultRange - maxReduction, defaultRange + maxIncrease), strength * Time.deltaTime);

            yield return new WaitForSeconds(rateDamping);
        }

        isFlickering = false;
    }

    private IEnumerator DoFlickerWithoutEmission()
    {
        isFlickering = true;

        while (!stopFlickering)
        {
            lightSource.intensity = Mathf.Lerp(lightSource.intensity, Random.Range(defaultIntensity - maxReduction, defaultIntensity + maxIncrease), strength * Time.deltaTime);
            lightSource.range = Mathf.Lerp(lightSource.range, Random.Range(defaultRange - maxReduction, defaultRange + maxIncrease), strength * Time.deltaTime);

            yield return new WaitForSeconds(rateDamping);
        }

        isFlickering = false;
    }
}