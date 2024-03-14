using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class SlamAttack : MonoBehaviour, IWarningGenerator
{
    [SerializeField] private ArenaInitializer arenaInitializer;
    [SerializeField] private Material warningMaterial;
    [SerializeField] private GameObject attackIndicatorPrefab;
    [SerializeField] private GameObject circularWarningPrefab;
    public float warningDuration = 2.5f; // Duration before the attack hits
    public float attackDuration = 1f; // Duration of the attack visual effect
    public TextMeshProUGUI warningText;
    private PlayerControl playerControl;
    private PlayerStatus playerStatus;
    private DifficultyManager difficultyManager;
    private WarningManager warningManager;


    private void Start()
    {
        warningText.gameObject.SetActive(false);
        playerControl = FindObjectOfType<PlayerControl>();
        playerStatus = FindObjectOfType<PlayerStatus>();

        // Set default stat values based on initial difficulty
        difficultyManager = DifficultyManager.Instance;
        // For warning about slam attack
        warningManager = WarningManager.Instance;
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

        List<string> warned = warningManager.ToggleWarning(GetWarningTiles(), true, WarningManager.WarningType.SLAM);
        // foreach (var ring in arenaInitializer.tilePositions)
        // {
        //     if (tileIndex < ring.Count)
        //     {
        //         Vector3 targetPosition = ring[tileIndex];
        //         targetPosition.y = 0.5f;
        //         var indicator = Instantiate(circularWarningPrefab, targetPosition, Quaternion.identity);
        //         StartCoroutine(HandleIndicatorFlash(indicator));
        //     }
        // }
        yield return new WaitForSeconds(warningDuration);

        warningText.gameObject.SetActive(false);
        warningManager.ToggleWarning(warned, false, WarningManager.WarningType.SLAM);  // turn off warnings on those tiles
        // Instantiate spikes
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
        CheckForPlayerDamage(tileIndex);
    }

    // LEAVING COMMENTED-OUT UNTIL I USE THE FLASHING EFFECT ON WARNINGMANAGER
    // private IEnumerator HandleIndicatorFlash(GameObject indicator)
    // {
    //     MeshRenderer meshRenderer = indicator.GetComponent<MeshRenderer>();
    //     Color originalColor = meshRenderer.material.color;
    //     float endTime = Time.time + warningDuration;

    //     // Flash the indicator
    //     while (Time.time < endTime)
    //     {
    //         float alpha = Mathf.Abs(Mathf.Sin(Time.time * 2)); // Flashing effect
    //         meshRenderer.material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
    //         yield return null;
    //     }

    //     Destroy(indicator);
    // }


    private IEnumerator HandleIndicatorLifecycle(GameObject indicator)
    {
        indicator.GetComponent<MeshRenderer>().material = warningMaterial;
        yield return new WaitForSeconds(attackDuration);
        Destroy(indicator);
    }


    private void CheckForPlayerDamage(int tileIndex)
    {
        if (playerControl.currentTileIndex == tileIndex)
        {
            // Get damage from DifficultyManager on the fly beccause no SlamAttack script lives in our scene permanently.
            playerStatus.TakeDamage(difficultyManager.GetValue(DifficultyManager.StatName.SLAM_DAMAGE));
        }
    }


    private IEnumerator FlashWarningText()
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


    // Satisfies IWarningGenerator interface
    public List<string> GetWarningTiles()
    {
        Dictionary<(int, int), string> mapping = warningManager.GetLogicalToPhysicalTileMapping();
        string tilename = mapping[(playerControl.currentRingIndex, playerControl.currentTileIndex)];

        // Pluck out the left-right index of the current tile
        string leftRightIndex = tilename.Substring(3, 2);
        Debug.Log($"slam GetWarning {leftRightIndex}");

        return new List<string> {
            $"R1_{leftRightIndex}",
            $"R2_{leftRightIndex}",
            $"R3_{leftRightIndex}",
            $"R4_{leftRightIndex}",
        };
    }
}
