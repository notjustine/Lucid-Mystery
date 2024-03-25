using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public int getCombo() { return combo; }

    public enum ComboChange
    {
        INCREASE,
        DECREASE,
        DECREASE2,
        RESET
    }

    void Start()
    {
        bossStates = FindObjectOfType<BossStates>();
        comboSlider = GameObject.FindGameObjectWithTag("ComboMeter").GetComponent<Image>();
        // comboSlider = GameObject.FindGameObjectWithTag("ComboMeter").GetComponent<Image>();
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
        combo = change switch
        {
            ComboChange.INCREASE => Math.Min(combo + 1, maxCombo),
            ComboChange.DECREASE => Math.Max(combo - 1, 1),
            ComboChange.DECREASE2 => Math.Max(combo - 2, 1),
            ComboChange.RESET => 1,
            _ => 1
        };

        playerDamage = (comboScaler * combo);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            BossHealth bossHealth = collision.gameObject.GetComponent<BossHealth>();
            if (SceneManager.GetActiveScene().name != "Tutorial")
            {
                if (bossStates.isSleeping && DifficultyManager.phase == 0)
                {
                    AudioManager.instance.PhaseMusicChange(1);
                    DifficultyManager.phase = 1;
                    bossStates.isSleeping = false;
                }
            }
            bossHealth.TakeDamage(playerDamage);
            UpdateCombo(ComboChange.RESET);
            AudioManager.instance.PlayOneShotAttached(SoundRef.Instance.attackSound, gameObject);
        }
    }
}
