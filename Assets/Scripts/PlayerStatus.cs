using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] public bool isInvincible = false;
    private float currHealth;
    public HealthBar healthBar;
    private Attack attack;
    private PlayerControl playerControl;
    private DifficultyManager difficultyManager;
    private HealingManager healingManager;
    private AnimationStateController animationStateController;

    void Start()
    {
        healthBar = GameObject.Find("Worker").GetComponentInChildren<HealthBar>();
        currHealth = maxHealth;
        healthBar.SetSliderMax(maxHealth);
        attack = FindObjectOfType<Attack>();
        playerControl = FindObjectOfType<PlayerControl>();
        healingManager = HealingManager.Instance;
        difficultyManager = DifficultyManager.Instance;
        animationStateController = FindObjectOfType<AnimationStateController>();
    }

    void Update()
    {
        // Find out if you're on a healing tile
        if (SceneManager.GetActiveScene().name != "Tutorial" && healingManager.IsHealing(playerControl.currentRingIndex, playerControl.currentTileIndex))
        {
            Heal(difficultyManager.GetValue(DifficultyManager.StatName.HEALING_RATE) * Time.deltaTime);
        }
    }

    public void TakeDamage(float amount)
    {
        if (isInvincible)
            return;
        AudioManager.instance.PlayOneShot(SoundRef.Instance.dmgTaken, gameObject.transform.position);

        currHealth -= amount;
        healthBar.SetSlider(currHealth);
        attack.UpdateCombo(Attack.ComboChange.DECREASE2);
        animationStateController.TriggerStumble();

        if (currHealth <= 0)
        {
            if (SceneManager.GetActiveScene().name == "Tutorial")
                {
                    currHealth = maxHealth;
                    healthBar.SetSlider(currHealth);
                }
            else
            {
                Die();
            }
        }
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

    private void Die()
    {
        isInvincible = true;
        DeathMenu.PlayerLoss();
        FadingScreenManager.Instance.DeathMenuTransitionToScene(1f);
        FindObjectOfType<BossStates>().isSleeping = true;
        // SceneManager.LoadScene("EndMenu", LoadSceneMode.Additive);
    }
}

