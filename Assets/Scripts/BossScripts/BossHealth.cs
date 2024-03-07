using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [SerializeField] public float maxHealth = 1000f;
    public float currHealth;
    public bool isInvulnerable = false;
    [SerializeField] private int phase = 0;

    public HealthBar healthBar;     // phase 1
    public HealthBar healthBar2;    // phase 2

    // Start is called before the first frame update
    public void Start()
    {
        currHealth = maxHealth;
        healthBar.SetSliderMax(maxHealth / 2);
        healthBar2.SetSliderMax(maxHealth / 2);
        phase = PlayerPrefs.GetInt("bossPhase");
    }

    public void TakeDamage(float amount)
    {
        currHealth -= amount;

        if (currHealth <= 0)
        {
            currHealth = 0;
            Die();
        }
        else if (currHealth <= maxHealth / 2)
        {
            PhaseTwo();
            healthBar.SetSlider(0f);
            healthBar2.SetSlider(currHealth);
        }
        else
        {
            healthBar.SetSlider(currHealth - (maxHealth / 2));
        }
    }

    private void Update()
    {
        // for testing hp bar
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TakeDamage(100f);
        }
    }

    private void PhaseTwo()
    {
        PlayerPrefs.SetInt("bossPhase", 2);
        AudioManager.instance.TriggerPhaseTwoMusic();
    }

    private void Die()
    {
        DeathMenu.BossLoss();
        SceneManager.LoadScene("EndMenu");
    }

    public void resetHealth()
    {
        currHealth = maxHealth;
    }

    public void SetInvulnerability(bool state)
    {
        isInvulnerable = state;
    }
}