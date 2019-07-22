using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour
{
    public float maxReduction;
    public float maxIncrease;
    public float rateDamping;
    public float strength;
    public bool stopFlickering;
    public bool controlEmission;

    private Light lightSource;
    private Material lightMaterial;
    private Renderer rend;
    private Color color;

    private float defaultIntensity;
    private float defaultRange;
    private bool isFlickering;

    private const string EMISSION_COLOR = "_EmissionColor";

    void Reset()
    {
        maxReduction = 0.2f;
        maxIncrease = 0.2f;
        rateDamping = 0.1f;
        strength = 300;
    }

    void OnEnable()
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

        if (controlEmission)
        {
            StartCoroutine(DoFlicker());
        }

        else
        {
            StartCoroutine(DoFlickerWithoutEmission());
        }
    }

    void Update()
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