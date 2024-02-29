using UnityEngine;
using UnityEngine.SceneManagement;
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
        healthBar.SetSlider(currHealth);
    }

    private void Update()
    {
        // for testing hp bar
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TakeDamage(100f);
        }
        if (currHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        int phase = PlayerPrefs.GetInt("bossPhase");
        switch (phase)
        {
            case 0:
                PlayerPrefs.SetInt("bossPhase", 1);
                currHealth = maxHealth;
                break;
            case 1:
                Ending.BossLoss();
                SceneManager.LoadScene("EndMenu");
                break;
            default:
                Debug.Log("Boss state is not a valid one. This shouldn't happen");
                break;
        }
        
    }
    public void SetInvulnerability(bool state)
    {
        isInvulnerable = state;
    }
}