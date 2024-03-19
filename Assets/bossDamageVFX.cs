using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class bossDamageVFX : MonoBehaviour
{
    public BossHealth bossHealth;
    public VisualEffect[] effects;
    private bool lightDamagePlayed = false;
    private bool heavyDamagePlayed = false;
    public float lightDamageThreshold = 50f;
    public float heavyDamageThreshold = 20f;
    
    void Start()
    {
        foreach(VisualEffect effect in effects)
        {
            effect.Stop();
        }

    }

    void Update()
    {
        if(bossHealth != null)
        {
            float healthPercentage = (bossHealth.currHealth / bossHealth.maxHealth) * 100f;
            if (healthPercentage <= lightDamageThreshold && healthPercentage >= heavyDamageThreshold && lightDamagePlayed == false)
            {
                lightDamage();
                lightDamagePlayed = true;
            }

            if (healthPercentage <= heavyDamageThreshold && heavyDamagePlayed == false)
            {
                heavyDamage();
                heavyDamagePlayed = true;
            }
        }
    }

    void lightDamage()
    {
        /*foreach (VisualEffect effect in effects)
        {
            effect.Play();
        }*/

        for(int i = 0; i < 2; i++)
        {
            effects[i].Play();
        }
        
    }

    void heavyDamage()
    {
        for (int i = 2; i < effects.Length; i++)
        {
            effects[i].Play();
        }
    }
}
