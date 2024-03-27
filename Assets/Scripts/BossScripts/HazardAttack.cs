using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HazardAttack : MonoBehaviour
{
    [SerializeField] private ArenaInitializer arenaInitializer;
    [SerializeField] private GameObject hazardPrefab;
    [SerializeField] private GameObject rotatableHead_GEO;
    [SerializeField] private float hazardTiming;
    [SerializeField] private int numberOfHazards;
    private WarningManager warningManager;
    private DifficultyManager difficultyManager;
    private PlayerControl playerControl;
    private PlayerStatus playerStatus;
    private HashSet<string> activeHazards = new HashSet<string>();
    private float timeToLand = 3f;

    private void Start()
    {
        warningManager = WarningManager.Instance;
        difficultyManager = DifficultyManager.Instance;
        playerControl = FindObjectOfType<PlayerControl>();
        playerStatus = FindObjectOfType<PlayerStatus>();
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.M))
        // {
        //    TriggerAttack();
        //}
    }

    public void TriggerAttack()
    {
        hazardTiming = DifficultyManager.Instance.GetValue(DifficultyManager.StatName.HAZARD_TIMING);
        numberOfHazards = (int)DifficultyManager.Instance.GetValue(DifficultyManager.StatName.HAZARD_COUNT);
        StartCoroutine(HazardSequence());
    }
    public void OnHazardLanded(GameObject hazard, string tileName)
    {
        List<string> targetedTilesNames = new List<string>();
        targetedTilesNames.Add(tileName);
        RegisterHazardTile(tileName);
        warningManager.ToggleWarning(targetedTilesNames, true, WarningManager.WarningType.HAZARD);
        StartCoroutine(CheckPlayerOnHazardousTile(tileName));
        StartCoroutine(MakeTileNonHazardousAfterDelay(tileName, hazardTiming));
    }

    private IEnumerator MakeTileNonHazardousAfterDelay(string tileName, float delay)
    {
        yield return new WaitForSeconds(delay);
        UnregisterHazardTile(tileName);
        warningManager.ToggleWarning(new List<string> { tileName }, false, WarningManager.WarningType.HAZARD);
    }
    private IEnumerator HazardSequence()
    {
        HashSet<int> selectedIndices = new HashSet<int>();
        List<Vector3> allTilePositions = new List<Vector3>();
        List<string> targetedTilesNames = new List<string>();


        foreach (var ring in arenaInitializer.tilePositions)
        {
            allTilePositions.AddRange(ring);
        }

        float timeToLand = 2f;

        for (int i = 0; i < numberOfHazards && selectedIndices.Count < allTilePositions.Count; i++)
        {
            int randomIndex = Random.Range(0, allTilePositions.Count);
            while (!selectedIndices.Add(randomIndex))
            {
                randomIndex = Random.Range(0, allTilePositions.Count);
            }

            Vector3 targetPosition = allTilePositions[randomIndex];
            string tileName = arenaInitializer.GetTileNameByPosition(targetPosition);
            targetedTilesNames.Add(tileName);

            GameObject hazardInstance = Instantiate(hazardPrefab, rotatableHead_GEO.transform.position, Quaternion.identity);
            Rigidbody rb = hazardInstance.AddComponent<Rigidbody>();
            Vector3 launchVelocity = CalculateLaunchVelocity(rotatableHead_GEO.transform.position, targetPosition, timeToLand);
            rb.velocity = launchVelocity;
            rb.useGravity = true;

        }

        yield return new WaitForSeconds(timeToLand + hazardTiming);
    }
    Vector3 CalculateLaunchVelocity(Vector3 start, Vector3 end, float timeToTarget)
    {
        Vector3 toTarget = end - start;
        Vector3 toTargetXZ = new Vector3(toTarget.x, 0, toTarget.z);
        float distanceXZ = toTargetXZ.magnitude;
        float velocityXZ = distanceXZ / timeToTarget;
        float velocityY = (toTarget.y / timeToTarget) + 0.5f * Physics.gravity.magnitude * timeToTarget;
        Vector3 velocity = toTargetXZ.normalized * velocityXZ;
        velocity.y = velocityY;
        return velocity;
    }

    private IEnumerator CheckPlayerOnHazardousTile(string tileName)
    {
        while (activeHazards.Contains(tileName))
        {
            Dictionary<(int, int), string> mapping = warningManager.GetLogicalToPhysicalTileMapping();
            string currentPlayerTileName = mapping[(playerControl.currentRingIndex, playerControl.currentTileIndex)];
            if (activeHazards.Contains(currentPlayerTileName))
            {
                playerStatus.TakeDamage(difficultyManager.GetValue(DifficultyManager.StatName.HAZARD_DAMAGE));
            }
            yield return new WaitForSeconds(1.5f);
        }
    }

    public void RegisterHazardTile(string tileName)
    {
        activeHazards.Add(tileName);
    }

    public void UnregisterHazardTile(string tileName)
    {
        activeHazards.Remove(tileName);
    }

    public bool IsPositionHazardous(Vector3 position)
    {
        string tileName = arenaInitializer.GetTileNameByPosition(position);
        return activeHazards.Contains(tileName);
    }
}
