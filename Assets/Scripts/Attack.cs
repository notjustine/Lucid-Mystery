using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.VFX;

public class Attack : MonoBehaviour
{
    
    private DifficultyManager difficultyManager;
    private BossVisuals bossVisuals;
    private AnimationStateController animationStateController;
    public float playerDamage;
    private float maxPlayerDamage;
    private readonly int minCombo = 1;
    [SerializeField] private int combo;
    private readonly int maxCombo = 5;
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
        combo = minCombo;
        bossVisuals = FindObjectOfType<BossVisuals>();
        animationStateController = FindObjectOfType<AnimationStateController>();
        comboSlider = GameObject.FindGameObjectWithTag("ComboMeter").GetComponent<Image>();
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
            ComboChange.DECREASE => Math.Max(combo - 1, minCombo),
            ComboChange.DECREASE2 => Math.Max(combo - 2, minCombo),
            ComboChange.RESET => minCombo,
            _ => minCombo,
        };

        playerDamage = (comboScaler * combo);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            BossHealth bossHealth = collision.gameObject.GetComponent<BossHealth>();

            if (!MusicEventHandler.beatCheck)
            {
                UpdateCombo(ComboChange.RESET);
                return;
            }
            
            // Maybe changes this if we revamp tutorial? 
            bossHealth.TakeDamage(TutorialManager.tutorialActive ? 0 : playerDamage);
            UpdateCombo(ComboChange.INCREASE);
            // Make the boss flash white-ish
            bossVisuals.FlashDamageColor();
            
            GameObject vfxInstance = new GameObject("VFX Instance");
            vfxInstance.transform.position = collision.contacts[0].point + new Vector3(0, 1f, 0);

            // Add a VisualEffect component
            VisualEffect vfx = vfxInstance.AddComponent<VisualEffect>();
            vfx.visualEffectAsset = vfxAsset;

            // Play the effect
            vfx.Play();

            // Optionally destroy the effect after a duration:
            Destroy(vfxInstance, 2f);
        }
    }
}
