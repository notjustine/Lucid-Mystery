using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using TMPro;
using UnityEngine.VFX;

public class SteamAttack : MonoBehaviour
{
    public Material idleMaterial; // Transparent material
    public Material warningMaterial; // Red, slightly transparent material
    public Material attackMaterial;
    public MeshRenderer attackAreaRenderer; // Assign the MeshRenderer of the cylinder object
    public TextMeshProUGUI warningText;
    public float warningDuration = 2.5f; // Duration of the warning phase
    private bool playerInAttackArea = false;
    private PlayerStatus playerStatus;
    public GameObject steamAttackVFX;
    public EventInstance steamAttackSound;

    private void Start()
    {
        // Initially set to idle material
        warningText.gameObject.SetActive(false);
        attackAreaRenderer.material = idleMaterial;
        VisualEffect[] effects = steamAttackVFX.GetComponentsInChildren<VisualEffect>();
        playerStatus = FindObjectOfType<PlayerStatus>();
        foreach (VisualEffect effect in effects)
        {
            effect.Stop();
        }

  
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.U))
        // {
        //    TriggerAttack();
        // }
    }
    // Call this method to start the attack sequence
    public void TriggerAttack()
    {
        StartCoroutine(AttackSequence());
    }

    public System.Collections.IEnumerator AttackSequence()
    {
        VisualEffect[] effects = steamAttackVFX.GetComponentsInChildren<VisualEffect>();
        StudioEventEmitter[] emitter = steamAttackVFX.GetComponentsInChildren<StudioEventEmitter>();
        warningText.gameObject.SetActive(true);
        warningText.text = "Back Up!!";
        StartCoroutine(FlashWarningText());
        // Switch to warning material
        attackAreaRenderer.material = warningMaterial;
        // Wait for the warning duration
        yield return new WaitForSeconds(warningDuration);
        attackAreaRenderer.material = attackMaterial;
        yield return new WaitForSeconds(0.1f);
        warningText.gameObject.SetActive(false);
        // emitter[1].Play();
        foreach (StudioEventEmitter em in emitter)
        {
            em.Play();
        }
        foreach (VisualEffect effect in effects)
        {
            effect.Play();
        };
        yield return new WaitForSeconds(0.5f);
        StopCoroutine(FlashWarningText());;
        if (playerInAttackArea){
            playerStatus.TakeDamage(20f);
        }
        foreach (VisualEffect effect in effects)
        {
            effect.Stop();
        }

        foreach (StudioEventEmitter em in emitter)
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
    private System.Collections.IEnumerator FlashWarningText()
    {
        float flashDuration = warningDuration; // Using the same duration as the warning for simplicity.
        float startTime = Time.time;
        while (Time.time - startTime < flashDuration)
        {
            float alpha = Mathf.Abs(Mathf.Sin(Time.time * 2));
            warningText.color = new Color(warningText.color.r, warningText.color.g, warningText.color.b, alpha);
            yield return null;
        }
        warningText.color = new Color(warningText.color.r, warningText.color.g, warningText.color.b, 1);
    }

}
