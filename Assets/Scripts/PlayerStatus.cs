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

    void Start()
    {
        currHealth = maxHealth;
        healthBar.SetSliderMax(maxHealth);
        attack = FindObjectOfType<Attack>();

    }

    void Update()
    {

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

