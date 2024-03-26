using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private bool isInvincible = false;
    private float currHealth;
    public HealthBar healthBar;
    private Attack attack;
    private PlayerControl playerControl;
    private HealingManager healingManager;
    private const float HEAL_RATE = 5f;  // healing per second

    void Start()
    {
        currHealth = maxHealth;
        healthBar.SetSliderMax(maxHealth);
        attack = FindObjectOfType<Attack>();
        playerControl = FindObjectOfType<PlayerControl>();
        healingManager = HealingManager.Instance;
    }

    void Update()
    {
        // Find out if you're on a healing tile
        if (healingManager.IsHealing(playerControl.currentRingIndex, playerControl.currentTileIndex))
        {
            Heal(HEAL_RATE * Time.deltaTime);
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
        Debug.Log($"healed by: {amount}");
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

