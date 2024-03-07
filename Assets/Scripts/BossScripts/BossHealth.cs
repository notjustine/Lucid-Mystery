using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 2000f;
    private float currHealth;
    public bool isInvulnerable = false;
    [SerializeField] private int phase = 0;

    public HealthBar healthBar;

    // Start is called before the first frame update
    public void Start()
    {
        currHealth = maxHealth;
        healthBar.SetSliderMax(maxHealth);
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
        }

        healthBar.SetSlider(currHealth);
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