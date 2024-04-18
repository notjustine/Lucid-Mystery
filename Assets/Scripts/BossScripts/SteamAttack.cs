using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using TMPro;
using UnityEngine.VFX;
using System.Collections.Generic;
using Unity.VisualScripting;

public class SteamAttack : MonoBehaviour, IWarningGenerator
{
    public Material idleMaterial; // Transparent material
    public Material warningMaterial; // Red, slightly transparent material
    public Material attackMaterial;
    public MeshRenderer attackAreaRenderer; // Assign the MeshRenderer of the cylinder object
    // public TextMeshProUGUI warningText;
    public float warningDuration = 2.5f; // Duration of the warning phase
    [SerializeField] float steamDamage;
    private DifficultyManager difficultyManager;
    private WarningManager warningManager;
    private bool playerInAttackArea = false;
    private PlayerStatus playerStatus;
    public GameObject steamAttackVFX;
    private PlayerControl playerControl;
    private StudioEventEmitter[] emitters;
    private VisualEffect[] effects;

    private void Start()
    {
        // Initially set to idle material
        difficultyManager = DifficultyManager.Instance;
        steamDamage = difficultyManager.GetValue(DifficultyManager.StatName.STEAM_DAMAGE);  // get default on startup
        // For flashing warnings on tiles
        warningManager = WarningManager.Instance;

        // warningText.gameObject.SetActive(false);
        attackAreaRenderer.material = idleMaterial;
        effects = steamAttackVFX.GetComponentsInChildren<VisualEffect>();
        emitters = steamAttackVFX.GetComponentsInChildren<StudioEventEmitter>();
        playerControl = FindObjectOfType<PlayerControl>();
        playerStatus = FindObjectOfType<PlayerStatus>();
        foreach (VisualEffect effect in effects)
        {
            effect.Stop();
        }
    }

    private void Update()
    {
    }

    private void OnDestroy()
    {
        foreach (StudioEventEmitter em in emitters)
        {
            em.Stop();
        }
    }


    // A setter, currently used by DifficultyManager when it notices the player changed the difficulty.
    public void SetSteamDamage(float damage)
    {
        steamDamage = damage;
    }


    // Call this method to start the attack sequence
    public void TriggerAttack()
    {
        StartCoroutine(AttackSequence());
    }


    public System.Collections.IEnumerator AttackSequence()
    {
        // VisualEffect[] effects = steamAttackVFX.GetComponentsInChildren<VisualEffect>();
      

        warningManager.ToggleWarning(GetWarningObjects(), true, WarningManager.WarningType.STEAM);
        // Switch to warning material
        attackAreaRenderer.material = warningMaterial;
        // Wait for the warning duration
        yield return new WaitForSeconds(warningDuration);
        warningManager.ToggleWarning(GetWarningObjects(), false, WarningManager.WarningType.STEAM);
        attackAreaRenderer.material = attackMaterial;
        yield return new WaitForSeconds(0.1f);

        foreach (StudioEventEmitter em in emitters)
        {
            em.Play();
        }
        foreach (VisualEffect effect in effects)
        {
            effect.Play();
        }
        if (playerInAttackArea)
        {
            playerControl.MoveToBackTile();
            playerStatus.TakeDamage(steamDamage);

        }
        yield return new WaitForSeconds(0.5f);

        foreach (VisualEffect effect in effects)
        {
            effect.Stop();
        }

        foreach (StudioEventEmitter em in emitters)
        {
            em.SetParameter("steamattack_end", 1);
        }
        attackAreaRenderer.material = idleMaterial;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInAttackArea = true;
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInAttackArea = false;
        }
    }


    /**
        Return the GameObject names of all tiles that need to be highlighted for this warning.
    */
    public List<string> GetWarningObjects()
    {
        return new List<string> {
            "R1_01",
            "R1_02",
            "R1_03",
            "R1_04",
            "R1_05",
            "R1_06",
            "R1_07",
            "R1_08",
            "R1_09",
            "R1_10",
            "R1_11",
            "R1_12",
            "R1_13",
            "R1_14",
            "R1_15",
            "R1_16",
            "R1_17",
            "R1_18",
            "R1_19",
            "R1_20",
            "R1_21",
            "R1_22",
            "R1_23",
            "R1_24",

            "R2_01",
            "R2_02",
            "R2_03",
            "R2_04",
            "R2_05",
            "R2_06",
            "R2_07",
            "R2_08",
            "R2_09",
            "R2_10",
            "R2_11",
            "R2_12",
            "R2_13",
            "R2_14",
            "R2_15",
            "R2_16",
            "R2_17",
            "R2_18",
            "R2_19",
            "R2_20",
            "R2_21",
            "R2_22",
            "R2_23",
            "R2_24",
            };
    }
}
