using UnityEngine;
using UnityEngine.InputSystem;
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
    private PlayerControl playerControl;
    private FadingScreen fade;

    // Start is called before the first frame update
    public void Start()
    {
        fade = FindObjectOfType<FadingScreen>();
        currHealth = maxHealth;
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            PlayerPrefs.SetInt("bossPhase", 0);
        }
        else
        {
           
            switch (PhaseController.Instance.phase)
            {
                case 2: 
                    healthBar.SetSlider(0f);
                    healthBar2.SetSlider(currHealth);
                    break;
                default:
                    healthBar.SetSliderMax(maxHealth / 2);
                    healthBar2.SetSliderMax(maxHealth / 2);
                    break;
            }
        }
        playerControl = FindObjectOfType<PlayerControl>();
        
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
        PhaseController.Instance.phase = 2;
        AudioManager.instance.PhaseMusicChange(2);
    }

    private void Die()
    {
        DeathMenu.BossLoss();
        AudioManager.instance.PhaseMusicChange(3);
        StartCoroutine(fade.FadeToBlack());
        playerControl.gameObject.SetActive(false);
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