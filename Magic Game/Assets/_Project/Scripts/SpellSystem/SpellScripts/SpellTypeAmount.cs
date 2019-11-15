using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellTypeAmount : MonoBehaviour
{
    public bool projectile;
    public bool beam;
    public bool aura;
    public int amount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (projectile == false && beam == false && aura == false)
        { amount = 0; }
        if (projectile == true && beam == false && aura == false)
        { amount = 1; }
        if (projectile == false && beam == true && aura == false)
        { amount = 1; }
        if (projectile == false && beam == false && aura == true)
        { amount = 1; }

        if (projectile == true && beam == true && aura == false)
        {
            amount = 2;
            GameObject.Find("ScoreUI").GetComponent<ScoreUI>().notificationString = "Double Trouble!";
        }
        if (projectile == false && beam == true && aura == true)
        {
            amount = 2;
            GameObject.Find("ScoreUI").GetComponent<ScoreUI>().notificationString = "Double Trouble!";
        }
        if (projectile == true && beam == false && aura == true)
        {
            amount = 2;
            GameObject.Find("ScoreUI").GetComponent<ScoreUI>().notificationString = "Double Trouble!";
        }

        if (projectile == true && beam == true && aura == true)
        {
            amount = 3;
            GameObject.Find("ScoreUI").GetComponent<ScoreUI>().notificationString = "Triple Trouble!";
        }
    }

    public void ActivateProjectile()
    {
        projectile = true;
    }
    public void DeactivateProjectile()
    {
        projectile = false;
    }
    public void ActivateBeam()
    {
        projectile = true;
    }
    public void DeactivateBeam()
    {
        projectile = false;
    }
    public void ActivateAura()
    {
        projectile = true;
    }
    public void DeactivateAura()
    {
        projectile = true;
    }
}
