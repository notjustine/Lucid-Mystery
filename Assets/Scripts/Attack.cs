using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private BossStates bossStates;
    private DifficultyManager difficultyManager;
    public float playerDamage;
    private float maxPlayerDamage;
    [SerializeField] private int combo = 0;
    [SerializeField] private int maxCombo = 5;
    [SerializeField] private float comboScaler = 0.1f;
    
    public enum ComboChange { INCREASE, DECREASE, RESET }

    void Start()
    {
        bossStates = FindObjectOfType<BossStates>();
        difficultyManager = DifficultyManager.Instance;
        if (difficultyManager)
            maxPlayerDamage = difficultyManager.GetValue(DifficultyManager.StatName.PLAYER_DAMAGE);  // get default on startup
        comboScaler = maxPlayerDamage / maxCombo;
    }
    
    
    // Allows DifficultyManager to push changes to the player damage.
    public void SetMaxPlayerDamage(float damage) 
    {
        maxPlayerDamage = damage;
        comboScaler = maxPlayerDamage / maxCombo;
    }
    
    public void UpdateCombo(ComboChange change)
    {
        if (change == ComboChange.RESET)
            combo = 0;
        else if (change == ComboChange.INCREASE)
            combo = Math.Min(combo + 1, maxCombo);
        else if (change == ComboChange.DECREASE)
            combo = Math.Max(combo - 1, 0);
        
        if (combo == 0)
            playerDamage = 1;
        else
            playerDamage = (comboScaler * combo);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            BossHealth bossHealth = collision.gameObject.GetComponent<BossHealth>();
            if (bossStates.isSleeping)
            {
                AudioManager.instance.PhaseMusicChange(1);
                DifficultyManager.phase = 1;
                bossStates.isSleeping = false;
            }
            
            bossHealth.TakeDamage(playerDamage);
            AudioManager.instance.PlayOneShotAttached(SoundRef.Instance.attackSound, gameObject);
            UpdateCombo(ComboChange.RESET);
        }
    }
}
