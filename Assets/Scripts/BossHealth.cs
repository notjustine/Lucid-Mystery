using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 1000f;
    private float currHealth;
    public bool isInvulnerable = false;

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
        Debug.Log("decreasing boss health.");
        healthBar.SetSlider(currHealth);
    }

    private void Update()
    {
        // for testing hp bar
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TakeDamage(10f);
        }
        if (currHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Debug.Log("Boss is dead");
    }
    public void SetInvulnerability(bool state)
    {
        isInvulnerable = state;
    }
}