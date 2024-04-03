using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [SerializeField] public float maxHealth = 1000f;
    public float currHealth;
    public bool isInvulnerable = false;
    public HealthBar healthBar;
    private PlayerControl playerControl;
    private PlayerStatus playerStatus;
    private FadingScreen fade;
    private AnimationStateController animationStateController;

    // Start is called before the first frame update
    public void Start()
    {
        fade = FindObjectOfType<FadingScreen>();
        playerStatus = FindObjectOfType<PlayerStatus>();
        currHealth = maxHealth;
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            DifficultyManager.phase = 0;
        }
        else
        {
            healthBar.SetSliderMax(maxHealth);

            if (DifficultyManager.phase == 2)
            {
                currHealth = maxHealth / 2;
                healthBar.SetSlider(maxHealth / 2);
                PhaseTwo();
            }
        }
        playerControl = FindObjectOfType<PlayerControl>();
        animationStateController = FindObjectOfType<AnimationStateController>();

    }
    public void TakeDamage(float amount)
    {
        if (isInvulnerable)
            return; 
        currHealth -= amount;

        if (currHealth <= 0)
        {
            currHealth = 0;
            healthBar.SetSlider(0f);
            playerStatus.isInvincible = true;
            Die();
        }
        else if (currHealth <= maxHealth / 2)
        {
            if (DifficultyManager.phase <= 1)
                PhaseTwo();
        }
        healthBar.SetSlider(currHealth);
    }

    private void Update()
    {
        // for testing hp bar
        // if (Input.GetKeyDown(KeyCode.Q))
        // {
        // TakeDamage(100f);
        //     Die();
        // }
    }

    private void PhaseTwo()
    {
        DifficultyManager.phase = 2;
        AudioManager.instance.PhaseMusicChange(2);
        animationStateController.TriggerPhase2();
    }

    private void Die()
    {
        isInvulnerable = true;
        FindObjectOfType<BossStates>().isSleeping = true;
        DeathMenu.BossLoss();
        AudioManager.instance.PhaseMusicChange(3);
        FadingScreenManager.Instance.DeathMenuTransitionToScene();
        animationStateController.TriggerDeath();
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