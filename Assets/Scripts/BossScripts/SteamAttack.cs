using UnityEngine;
using TMPro;

public class SteamAttack : MonoBehaviour
{
    public Material idleMaterial; // Transparent material
    public Material warningMaterial; // Red, slightly transparent material
    public Material attackMaterial;
    public MeshRenderer attackAreaRenderer; // Assign the MeshRenderer of the cylinder object
    public TextMeshProUGUI warningText;
    public float warningDuration = 3.0f; // Duration of the warning phase
    private bool playerInAttackArea = false;
    private PlayerStatus playerStatus;
    private void Start()
    {
        // Initially set to idle material
        warningText.gameObject.SetActive(false);
        attackAreaRenderer.material = idleMaterial;
        playerStatus = FindObjectOfType<PlayerStatus>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            TriggerAttack();
        }
    }
    // Call this method to start the attack sequence
    public void TriggerAttack()
    {
        StartCoroutine(AttackSequence());
    }

    public System.Collections.IEnumerator AttackSequence()
    {
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

       
        StopCoroutine(FlashWarningText());;
        if (playerInAttackArea){
            playerStatus.TakeDamage(20f);
        }
        attackAreaRenderer.material = idleMaterial;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("collided w/ player");
            playerInAttackArea = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.LogWarning("Left Attack Area.");
            playerInAttackArea = false;
        }
    }
    private System.Collections.IEnumerator FlashWarningText()
    {
        float flashDuration = warningDuration; // Use the same duration as the warning for simplicity.
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
