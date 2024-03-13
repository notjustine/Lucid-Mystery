using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private BossStates bossStates;
    private DifficultyManager difficultyManager;
    public float playerDamage;
    

    void Start()
    {
        bossStates = FindObjectOfType<BossStates>();
        difficultyManager = DifficultyManager.Instance;
        if (difficultyManager)
            playerDamage = difficultyManager.GetValue(DifficultyManager.StatName.PLAYER_DAMAGE);  // get default on startup
    }
    
    
    // Allows DifficultyManager to push changes to the player damage.
    public void SetPlayerDamage(float damage) 
    {
        playerDamage = damage;
    }
    

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            BossHealth bossHealth = collision.gameObject.GetComponent<BossHealth>();
            if (bossStates.isSleeping)
            {
                AudioManager.instance.PhaseMusicChange(1);
                PhaseController.Instance.phase = 1;
            }
            bossStates.isSleeping = false;
            bossHealth.TakeDamage(playerDamage);
            AudioManager.instance.PlayOneShotAttached(SoundRef.Instance.attackSound, gameObject);
        }
    }
}
