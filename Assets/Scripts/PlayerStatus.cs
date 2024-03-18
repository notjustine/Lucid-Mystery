using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private bool isInvincible = false;
    private DifficultyManager difficultyManager;
    private float currHealth;
    public HealthBar healthBar;
    private Attack attack;
    private HazardAttack hazardAttack;
    private float hazardDamage;
    private float damageCooldown;
    private float lastDamageTime;

    void Start()
    {
        currHealth = maxHealth;
        healthBar.SetSliderMax(maxHealth);
        attack = FindObjectOfType<Attack>();
        hazardAttack = FindObjectOfType<HazardAttack>();
        difficultyManager = DifficultyManager.Instance;
        hazardDamage = DifficultyManager.Instance.GetValue(DifficultyManager.StatName.HAZARD_DAMAGE);
        damageCooldown = 2f;
        lastDamageTime = 0f;

    }

    void Update()
    {
        if (hazardAttack != null && Time.time >= lastDamageTime + damageCooldown)
        {
            if (hazardAttack.IsPositionHazardous(transform.position))
            {
                TakeDamage(hazardDamage * damageCooldown);
                lastDamageTime = Time.time;
            }
        }
    }

    public void TakeDamage(float amount)
    {
        AudioManager.instance.PlayOneShot(SoundRef.Instance.dmgTaken, gameObject.transform.position);
        if (isInvincible)
            return;

        currHealth -= amount;
        healthBar.SetSlider(currHealth);
        attack.UpdateCombo(Attack.ComboChange.DECREASE2);

        if (currHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currHealth += amount;
        if (currHealth > maxHealth)
        {
            currHealth = maxHealth;
        }
        healthBar.SetSlider(currHealth);
    }

    private void Die()
    {
        DeathMenu.PlayerLoss();
        SceneManager.LoadScene("EndMenu", LoadSceneMode.Additive);
    }
}

