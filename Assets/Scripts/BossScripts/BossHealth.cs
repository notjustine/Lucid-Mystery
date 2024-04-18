using UnityEngine;
using UnityEngine.SceneManagement;

public class BossHealth : MonoBehaviour
{
    [SerializeField] public float maxHealth = 1000f;
    public float currHealth;
    public bool isInvulnerable = false;
    public HealthBar healthBar;
    private PlayerStatus playerStatus;
    private AnimationStateController animationStateController;
    private BossStates bossStates;

    public GameObject furnace;
    public Material furnaceOn;
    public Material furnaceOff;


    // Start is called before the first frame update
    public void Start()
    {
        playerStatus = FindObjectOfType<PlayerStatus>();
        bossStates = FindObjectOfType<BossStates>();
        currHealth = maxHealth;
        animationStateController = FindObjectsOfType<AnimationStateController>()[1];
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            DifficultyManager.phase = 0;
        }
        else
        {
            healthBar.SetSliderMax(maxHealth);
        }
        
    }
    public void TakeDamage(float amount)
    {
        if (isInvulnerable)
            return;
        if (amount > 0 && currHealth == maxHealth)
        {
            AudioManager.instance.PhaseMusicChange(1);
            animationStateController.TriggerAwaken();
            DifficultyManager.phase = 1;
            bossStates.isSleeping = false;
            furnace.GetComponent<MeshRenderer>().material = furnaceOn;
        }
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
        animationStateController.TriggerFlinch();   
    }

    private void Update()
    {
        // for testing hp bar
        // if (Input.GetKeyDown(KeyCode.Q))
        // {
            // TakeDamage(100f);
            // Die();
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
        furnace.GetComponent<MeshRenderer>().material = furnaceOff;
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