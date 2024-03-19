using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class bossDamageVFX : MonoBehaviour
{
    public BossHealth bossHealth;
    public VisualEffect[] effects;
    private bool VFXplayed = false;
    
    void Start()
    {
        foreach(VisualEffect effect in effects)
        {
            effect.Stop();
        }

    }

    void Update()
    {
        if(VFXplayed == false && bossHealth != null)
        {
            float healthPercentage = (bossHealth.currHealth / bossHealth.maxHealth) * 100f;
            if (healthPercentage <= 80f)
            {
                playVFX();
                VFXplayed = true;
            }
        }
    }

    void playVFX()
    {
        foreach (VisualEffect effect in effects)
        {
            effect.Play();
        }
        
    }
}
