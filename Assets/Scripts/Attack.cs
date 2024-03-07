using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private BossStates bossStates;
    public float playerDamage = 50f;
    // Start is called before the first frame update
    void Start()
    {
        bossStates = FindObjectOfType<BossStates>();
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            BossHealth bossHealth = collision.gameObject.GetComponent<BossHealth>();
            if (bossStates.isSleeping)
            {
                AudioManager.instance.TriggerPhaseOneMusic();
                PlayerPrefs.SetInt("bossPhase", 1);
            }
            bossStates.isSleeping = false;
            bossHealth.TakeDamage(playerDamage);
            AudioManager.instance.PlayOneShotAttached(SoundRef.Instance.attackSound, gameObject);
            Debug.Log("Enemy Hit");

            // Destroy(collision.gameObject);
        }
    }
}
