using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.VFX;

public class Attack : MonoBehaviour
{
    private BossStates bossStates;
    private DifficultyManager difficultyManager;
    public float playerDamage;
    private float maxPlayerDamage;
    [SerializeField] private int combo = 0;
    [SerializeField] private int maxCombo = 5;
    [SerializeField] private float comboScaler = 0.1f;
    private Image comboSlider;
    [SerializeField] private Sprite[] comboSprites;
    public VisualEffectAsset vfxAsset;
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
       comboSlider.sprite = comboSprites[combo];
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
            ComboChange.DECREASE => Math.Max(combo - 1, 0),
            ComboChange.DECREASE2 => Math.Max(combo - 2, 0),
            ComboChange.RESET => 0,
            _ => 0
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
            // Instantiate(vfxPrefab, collision.contacts[0].point, Quaternion.identity); 
            GameObject vfxInstance = new GameObject("VFX Instance");
            vfxInstance.transform.position = collision.contacts[0].point + new Vector3(0, 1f, 0);

            // Add a VisualEffect component
            VisualEffect vfx = vfxInstance.AddComponent<VisualEffect>();
            vfx.visualEffectAsset = vfxAsset;

            // Play the effect
            vfx.Play();

            // Optionally destroy the effect after a duration:
            Destroy(vfxInstance, 2f);
            
            bossHealth.TakeDamage(playerDamage);
            UpdateCombo(ComboChange.RESET);
            AudioManager.instance.PlayOneShotAttached(SoundRef.Instance.attackSound, gameObject);
        }
    }
}
