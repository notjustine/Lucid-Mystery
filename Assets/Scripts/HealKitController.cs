using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class HealKitController : MonoBehaviour
{
    GameObject player;
    PlayerStatus playerStatus;
    HealingManager healingManager;
    DifficultyManager difficultyManager;
    public string tilename;
    Animator animator;
    private float kitDuration;
    private float kitLifeLength;


    void Start()
    {
        playerStatus = FindObjectOfType<PlayerStatus>();
        healingManager = HealingManager.Instance;
        difficultyManager = DifficultyManager.Instance;
        kitDuration = 6f;
        kitLifeLength = 0f;
        animator = GetComponentInChildren<Animator>();
    }


    void Update()
    {
        kitLifeLength += Time.deltaTime;
        if (kitLifeLength > kitDuration)
        {
            if (animator.GetBool("isActive"))
            {
                animator.SetBool("isActive", false);
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("FakeExit"))
        {
            Destroy(gameObject);
        }
    }



    /**
        If the player bumps into a heal kit, trigger healing and animate the healkit.
    */
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Weapon")
        {
            // GameObject burst = burstTransform.gameObject;
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


    private void OnDestroy()
    {
        // if this gets destroyed, we should also let the HealingManager know to stop making that tile flash
        healingManager.RemoveFromHealingTiles(tilename);
        healingManager.HandleToggleMaterial(tilename);
    }


    IEnumerator HealAnimation()
    {
        // Switch healing on
        GameObject player = GameObject.Find("healParticles");
        VisualEffect healing = player.GetComponentInChildren<VisualEffect>();
        healing.Play();
        yield return new WaitForSeconds(0.5f);
        healing.Stop();
    }
}
