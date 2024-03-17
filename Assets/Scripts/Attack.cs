using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attack : MonoBehaviour
{
    private BossStates bossStates;
    private DifficultyManager difficultyManager;
    public float playerDamage;
    private float maxPlayerDamage;
    [SerializeField] private int combo = 1;
    [SerializeField] private int maxCombo = 5;
    [SerializeField] private float comboScaler = 0.1f;
    private Image comboSlider;
    [SerializeField] private Sprite[] comboSprites;

    public enum ComboChange
    {
        INCREASE,
        DECREASE,
        RESET
    }

    void Start()
    {
        bossStates = FindObjectOfType<BossStates>();
        comboSlider = GameObject.FindGameObjectWithTag("ComboMeter").GetComponent<Image>();
        difficultyManager = DifficultyManager.Instance;
        if (difficultyManager)
            SetMaxPlayerDamage(difficultyManager.GetValue(DifficultyManager.StatName.PLAYER_DAMAGE));
        comboScaler = maxPlayerDamage / maxCombo;
    }

    void Update()
    {
        comboSlider.sprite = comboSprites[combo - 1];
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
            combo = 1;
        else if (change == ComboChange.INCREASE)
            combo = Math.Min(combo + 1, maxCombo);
        else if (change == ComboChange.DECREASE)
            combo = Math.Max(combo - 2, 1);
        
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
        }
    }
}
