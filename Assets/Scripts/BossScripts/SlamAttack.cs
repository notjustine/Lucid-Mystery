using UnityEngine;
using System.Collections;
using TMPro;

public class SlamAttack : MonoBehaviour
{
    [SerializeField] private ArenaInitializer arenaInitializer;
    [SerializeField] private Material warningMaterial;
    [SerializeField] private GameObject attackIndicatorPrefab;
    public float warningDuration = 2.5f; // Duration before the attack hits
    public float attackDuration = 1f; // Duration of the attack visual effect
    public TextMeshProUGUI warningText;
    private PlayerControl playerControl;
    private PlayerStatus playerStatus;

    private void Start()
    {
        warningText.gameObject.SetActive(false);
        playerControl = FindObjectOfType<PlayerControl>();
        playerStatus = FindObjectOfType<PlayerStatus>();
    }

    public void TriggerAttack(int tileIndex)
    {
        StartCoroutine(AttackSequence(tileIndex));
    }

    private IEnumerator AttackSequence(int tileIndex)
    {
        warningText.gameObject.SetActive(true);
        warningText.text = "Avoid tiles with Indicator!!";
        StartCoroutine(FlashWarningText());
        foreach (var ring in arenaInitializer.tilePositions)
        {
            if (tileIndex < ring.Count)
            {
                Vector3 targetPosition = ring[tileIndex];
                targetPosition.y = 0f;
                var indicator = Instantiate(attackIndicatorPrefab, targetPosition, Quaternion.identity);
                StartCoroutine(HandleIndicatorLifecycle(indicator));
            }
        }
        yield return new WaitForSeconds(warningDuration);
        CheckForPlayerDamage(tileIndex);
    }


    private IEnumerator HandleIndicatorLifecycle(GameObject indicator)
    {
        indicator.GetComponent<MeshRenderer>().material = warningMaterial;
        yield return new WaitForSeconds(warningDuration);
        yield return new WaitForSeconds(attackDuration);
        Destroy(indicator);
    }

    private void CheckForPlayerDamage(int tileIndex)
    {
        if (playerControl.currentTileIndex == tileIndex)
        {
            playerStatus.TakeDamage(5f);
        }
    }
    private System.Collections.IEnumerator FlashWarningText()
    {
        float flashDuration = warningDuration;
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

