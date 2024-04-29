using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerStatus : MonoBehaviour
{
    public float maxHealth = 100f;
    [SerializeField] public bool isInvincible = false;
    public float currHealth;
    public HealthBar healthBar;
    private Attack attack;
    private PlayerVisuals playerVisuals;
    private PlayerControl playerControl;
    private DifficultyManager difficultyManager;
    private HealingManager healingManager;
    private AnimationStateController animationStateController;


    void Start()
    {
        currHealth = maxHealth;
        healthBar.SetSliderMax(maxHealth);
        attack = FindObjectOfType<Attack>();
        playerVisuals = FindObjectOfType<PlayerVisuals>();
        playerControl = FindObjectOfType<PlayerControl>();
        healingManager = HealingManager.Instance;
        difficultyManager = DifficultyManager.Instance;
        animationStateController = FindObjectOfType<AnimationStateController>();
    }


    public void TakeDamage(float amount)
    {
        if (isInvincible)
            return;
        //AudioManager.instance.PlayOneShot(SoundRef.Instance.dmgTaken, gameObject.transform.position);

        if (PlayerPrefs.GetInt("Rumble", 0) == 1 && Gamepad.current != null)
            StartCoroutine(ControllerRumble(0.2f));
        currHealth -= amount;
        healthBar.SetSlider(currHealth);
        playerVisuals.FlashDamageColor();
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
        AudioManager.instance.PlayOneShotAttached(SoundRef.Instance.heal, gameObject);
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

    public static IEnumerator ControllerRumble(float duration)
    {
        Gamepad.current.SetMotorSpeeds(0.4f, 0.5f);
        yield return new WaitForSecondsRealtime(duration);
        Gamepad.current.SetMotorSpeeds(0f, 0f);
    }
}

