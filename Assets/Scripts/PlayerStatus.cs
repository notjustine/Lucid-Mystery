using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private bool isInviciible = false;
    private float currHealth;
    private Attack attack;

    public HealthBar healthBar;
    // Start is called before the first frame update
    public void Start()
    {
        currHealth = maxHealth;
        healthBar.SetSliderMax(maxHealth);
        attack = FindObjectOfType<Attack>();
    }

    public void TakeDamage(float amount)
    {
        AudioManager.instance.PlayOneShot(SoundRef.Instance.dmgTaken, gameObject.transform.position);
        if (isInviciible)
            return;
        
        currHealth -= amount;
        healthBar.SetSlider(currHealth);
        attack.UpdateCombo(Attack.ComboChange.DECREASE2);
        if (currHealth <= 0)
            Die();
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
        // if (Input.GetKeyDown(KeyCode.K))
        // {
        //     TakeDamage(20f);
        // } else if (Input.GetKeyDown(KeyCode.H))
        // {
        //     Heal(10f);
        // }
        //
        // if (currHealth <= 0)
        // {
        //     Die();
        // }
    }
    private void Die()
    {
        // Ending.PlayerLoss();
        DeathMenu.PlayerLoss();
        SceneManager.LoadScene("EndMenu", LoadSceneMode.Additive);
    }
}
