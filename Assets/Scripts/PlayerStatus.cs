using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currHealth;

    public HealthBar healthBar;
    // Start is called before the first frame update
    public void Start()
    {
        currHealth = maxHealth;
        healthBar.SetSliderMax(maxHealth);
    }

    public void TakeDamage(float amount)
    {
        currHealth -= amount;
        if (currHealth < 0)
        {
            currHealth = 0;
        }
        Debug.Log("decreasing health.");
        healthBar.SetSlider(currHealth);
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

    private void Update()
    {
        // for testing hp bar
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(20f);
        } else if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(10f);
        }
        
        if (currHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Debug.Log("dead now");
    }
}
