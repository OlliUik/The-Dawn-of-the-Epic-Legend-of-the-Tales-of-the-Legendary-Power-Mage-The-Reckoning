using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{

    public bool electric = false;
    public float duration;

    GameObject electricparticles = null;

    void Start()
    {
        electricparticles = GetComponentInChildren<ParticleSystem>().gameObject;
        electricparticles.SetActive(electric);
        electricparticles.transform.localScale = transform.localScale;
    }

    public void SetEletric(bool electric, float duration = default)
    {
        this.electric = electric;
        this.duration = duration;
        electricparticles.SetActive(electric);
    }

    void Update()
    {
        if(duration > 0)
        {
            duration -= Time.deltaTime;
        }
        else
        {
            SetEletric(false);
        }
    }
}
