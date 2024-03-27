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
    private PlayerControl playerControl;
    private PlayerStatus playerStatus;
    private DifficultyManager difficultyManager;
    private WarningManager warningManager;


    private void Start()
    {
        playerControl = FindObjectOfType<PlayerControl>();
        playerStatus = FindObjectOfType<PlayerStatus>();

        // Set default stat values based on initial difficulty
        difficultyManager = DifficultyManager.Instance;
        // For warning about slam attack
        warningManager = WarningManager.Instance;
    }

    private void Update()
    {

    }

    public void TriggerAttack(int tileIndex)
    {
        StartCoroutine(AttackSequence(tileIndex));
    }


    private IEnumerator AttackSequence(int tileIndex)
    {
        // Trigger warnings for the affected tiles
        List<string> warned = warningManager.ToggleWarning(GetWarningObjects(), true, WarningManager.WarningType.SLAM);
        yield return new WaitForSeconds(warningDuration);

        // Turn off warnings on those tiles
        warningManager.ToggleWarning(warned, false, WarningManager.WarningType.SLAM);

        int tilesPerRing = arenaInitializer.tilesPerRing;
        int leftTileIndex = (tileIndex - 1 + tilesPerRing) % tilesPerRing;
        int rightTileIndex = (tileIndex + 1) % tilesPerRing;

        List<int> targetIndices = new List<int> { leftTileIndex, tileIndex, rightTileIndex };

        foreach (var ring in arenaInitializer.tilePositions)
        {
            foreach (var index in targetIndices)
            {
                if (index < ring.Count)
                {
                    Vector3 targetPosition = ring[index];
                    targetPosition.y = 0f;
                    var indicator = Instantiate(attackIndicatorPrefab, targetPosition, Quaternion.identity);
                    StartCoroutine(HandleIndicatorLifecycle(indicator));
                }
            }
        }

        foreach (var index in targetIndices)
        {
            CheckForPlayerDamage(index);
        }
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
    public List<string> GetWarningObjects()
    {
        Dictionary<(int, int), string> mapping = warningManager.GetLogicalToPhysicalTileMapping();
        string tilename = mapping[(playerControl.currentRingIndex, playerControl.currentTileIndex)];
        List<string> warningTiles = new List<string>();
        int tilesPerRing = arenaInitializer.tilesPerRing;
        int leftIndex = (playerControl.currentTileIndex - 1 + tilesPerRing) % tilesPerRing;
        int rightIndex = (playerControl.currentTileIndex + 1) % tilesPerRing;

        for (int ring = 0; ring <= 4; ring++)
        {
            if (mapping.ContainsKey((ring, playerControl.currentTileIndex)))
                warningTiles.Add(mapping[(ring, playerControl.currentTileIndex)]);

            if (mapping.ContainsKey((ring, leftIndex)))
                warningTiles.Add(mapping[(ring, leftIndex)]);

            if (mapping.ContainsKey((ring, rightIndex)))
                warningTiles.Add(mapping[(ring, rightIndex)]);
        }

        return warningTiles;
    }
}