using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
using System;

public class HealKitTutorialController : MonoBehaviour
{
    GameObject player;
    PlayerStatus playerStatus;
    DifficultyManager difficultyManager;
    public string tilename;
    Animator animator;
    public event Action OnHealEvent;


    void Start()
    {
        playerStatus = FindObjectOfType<PlayerStatus>();
        difficultyManager = DifficultyManager.Instance;
        animator = GetComponentInChildren<Animator>();
    }


    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("FakeExit"))
        {
            Destroy(gameObject);
            OnHealEvent?.Invoke();
        }
    }

    /**
        If the player bumps into a heal kit, trigger healing and animate the healkit.
    */
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Weapon")
        {
            VisualEffect[] effects = GetComponentsInChildren<VisualEffect>();
            player = GameObject.FindGameObjectWithTag("Player");
            playerStatus = player.GetComponent<PlayerStatus>();
            if (playerStatus != null)
            {
                playerStatus.Heal(difficultyManager.GetValue(DifficultyManager.StatName.HEALING_AMOUNT));
                foreach (VisualEffect effect in effects)
                {
                    if (effect.name == "Heal_Burst")
                    {
                        effect.Play();
                    }
                }
                StartCoroutine(HealAnimation());
                if (animator.GetBool("isActive"))
                {
                    animator.SetBool("isActive", false);
                }
            }
        }
    }


    IEnumerator HealAnimation()
    {
        GameObject player = GameObject.Find("healParticles");
        VisualEffect healing = player.GetComponentInChildren<VisualEffect>();
        healing.Play();
        yield return new WaitForSeconds(0.5f);
        healing.Stop();
    }
}
