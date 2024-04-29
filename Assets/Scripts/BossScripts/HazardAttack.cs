using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.VFX;

public class HazardAttack : MonoBehaviour
{
    [SerializeField] private ArenaInitializer arenaInitializer;
    [SerializeField] private GameObject hazardPrefab;
    [SerializeField] private GameObject rotatableHead_GEO;
    [SerializeField] private GameObject Nails;
    [SerializeField] private float hazardTiming;
    [SerializeField] private float timeToLand = 3f;
    [SerializeField] private int numberOfHazards;
    [SerializeField] private VisualEffect dustEffect;
    private WarningManager warningManager;
    private DifficultyManager difficultyManager;
    private HealingManager healingManager;
    private List<string> activeHazards = new List<string>();

    // The two data structures below allow the healing manager to track whether a healing tile will overlap with a hazard.
    public Dictionary<string, GameObject> nailsMap = new Dictionary<string, GameObject>();
    public List<string> targetedTilesNames;

    private void Start()
    {
        warningManager = WarningManager.Instance;
        difficultyManager = DifficultyManager.Instance;
        healingManager = HealingManager.Instance;
        targetedTilesNames = new List<string>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TriggerAttack();
        }
    }

    public void CleanupHazard(string tileName)
    {
        if (nailsMap.TryGetValue(tileName, out GameObject nailObject))
        {
            Destroy(nailObject);
            nailsMap.Remove(tileName);
        }
    }

    public void TriggerAttack()
    {
        hazardTiming = difficultyManager.GetValue(DifficultyManager.StatName.HAZARD_TIMING);
        numberOfHazards = (int)difficultyManager.GetValue(DifficultyManager.StatName.HAZARD_COUNT);
        StartCoroutine(HazardSequence());
    }

    public void OnHazardLanded(GameObject hazard, GameObject collide, string tileName)
    {
        //PlayDustEffect();
        Vector3 nailSpawnPos = new Vector3(collide.transform.position.x, 0f, collide.transform.position.z);
        GameObject nails = Instantiate(Nails, nailSpawnPos, Quaternion.identity);
        nailsMap[tileName] = nails;
        StartCoroutine(MakeTileNonHazardousAfterDelay(tileName, hazardTiming));
    }

    private IEnumerator MakeTileNonHazardousAfterDelay(string tileName, float delay)
    {
        yield return new WaitForSeconds(delay);
        CleanupHazard(tileName);
    }

    private IEnumerator HazardSequence()
    {
        HashSet<(int, int)> selectedTiles = new HashSet<(int, int)>();
        Dictionary<(int, int), string> mapping = warningManager.GetLogicalToPhysicalTileMapping();

        while (selectedTiles.Count < numberOfHazards)
        {
            int ringIndex = Random.Range(0, arenaInitializer.tilePositions.Count);
            int tileIndex = Random.Range(0, arenaInitializer.tilePositions[ringIndex].Count);

            Vector3 targetPosition = arenaInitializer.tilePositions[ringIndex][tileIndex];
            string tileName = mapping[(ringIndex, tileIndex)];
            bool isHealingTile = healingManager.healingTiles.Contains(tileName);

            while (isHealingTile)
            {
                // reselect because this tile is among healing tiles.
                // Debug.Log("reselect hazard");
                ringIndex = Random.Range(0, arenaInitializer.tilePositions.Count);
                tileIndex = Random.Range(0, arenaInitializer.tilePositions[ringIndex].Count);
                targetPosition = arenaInitializer.tilePositions[ringIndex][tileIndex];
                tileName = mapping[(ringIndex, tileIndex)];
                isHealingTile = healingManager.healingTiles.Contains(tileName);
            }

            if (!selectedTiles.Add((ringIndex, tileIndex)))
                continue;

            GameObject hazardInstance = Instantiate(hazardPrefab, rotatableHead_GEO.transform.position, Quaternion.identity);
            Rigidbody rb = hazardInstance.AddComponent<Rigidbody>();
            Vector3 launchVelocity = CalculateLaunchVelocity(rotatableHead_GEO.transform.position, targetPosition, timeToLand);
            rb.velocity = launchVelocity;
            rb.useGravity = true;

            warningManager.ToggleWarning(new List<string> { tileName }, true, WarningManager.WarningType.HAZARD);

            targetedTilesNames.Add(tileName);
        }
        yield return new WaitForSeconds(timeToLand);
        warningManager.ToggleWarning(targetedTilesNames, false, WarningManager.WarningType.HAZARD);
        targetedTilesNames.Clear();
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
    private void PlayDustEffect()
    {
        if (dustEffect != null)
        {
            dustEffect.Play();
        }
    }
}
