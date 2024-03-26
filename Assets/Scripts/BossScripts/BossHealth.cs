using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [SerializeField] public float maxHealth = 1000f;
    public float currHealth;
    public bool isInvulnerable = false;
    public bool isPhase2 = false;

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
            DifficultyManager.phase = 0;
        }
        else
        {
           healthBar.SetSliderMax(maxHealth / 2);
           healthBar2.SetSliderMax(maxHealth / 2);
        
            if (DifficultyManager.phase == 2)
            {
                currHealth = maxHealth / 2;
                Debug.Log("SETTING HEALTH TO HALF: " + currHealth);
                healthBar.SetSlider(0f);
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
            healthBar2.SetSlider(0f);
            Die();
        }
        else if (currHealth <= maxHealth / 2)
        {
            if (DifficultyManager.phase <= 1)
            {
                if (!isPhase2)
                {
                    isPhase2 = true;
                }

                PhaseTwo();
                healthBar.SetSlider(0f);
            }
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
        DifficultyManager.phase = 2;
        AudioManager.instance.PhaseMusicChange(2);
    }

    private void Die()
    {
        FindObjectOfType<BossStates>().isSleeping = true;
        DeathMenu.BossLoss();
        AudioManager.instance.PhaseMusicChange(3);
        FadingScreenManager.Instance.DeathMenuTransitionToScene();
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