using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HazardAttack : MonoBehaviour
{
    [SerializeField] private ArenaInitializer arenaInitializer;
    [SerializeField] private GameObject hazardPrefab;
    [SerializeField] private GameObject rotatableHead_GEO;
    [SerializeField] private GameObject Nails;
    [SerializeField] private float hazardTiming;
    [SerializeField] private float timeToLand = 3f;
    [SerializeField] private int numberOfHazards;
    private WarningManager warningManager;
    private DifficultyManager difficultyManager;
    private List<string> activeHazards = new List<string>();
    private Dictionary<string, GameObject> nailsMap = new Dictionary<string, GameObject>();

    private void Start()
    {
        warningManager = WarningManager.Instance;
        difficultyManager = DifficultyManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
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
        Vector3 nailSpawnPos = new Vector3(collide.transform.position.x, 0f, collide.transform.position.z);
        GameObject nails = Instantiate(Nails, nailSpawnPos, Quaternion.identity);
        nailsMap[tileName] = nails;
        StartCoroutine(MakeTileNonHazardousAfterDelay(tileName, hazardTiming));
        List<string> targetedTilesNames = new List<string> { tileName };
        warningManager.ToggleWarning(targetedTilesNames, false, WarningManager.WarningType.HAZARD);
    }

    private IEnumerator MakeTileNonHazardousAfterDelay(string tileName, float delay)
    {
        yield return new WaitForSeconds(delay);
        CleanupHazard(tileName);
    }
    private IEnumerator HazardSequence()
    {
        HashSet<(int, int)> selectedTiles = new HashSet<(int, int)>(); 
        List<string> targetedTilesNames = new List<string>(); 

        while (selectedTiles.Count < numberOfHazards)
        {
            int ringIndex = Random.Range(0, arenaInitializer.tilePositions.Count);
            int tileIndex = Random.Range(0, arenaInitializer.tilePositions[ringIndex].Count);

            if (!selectedTiles.Add((ringIndex, tileIndex)))
                continue;

            Vector3 targetPosition = arenaInitializer.tilePositions[ringIndex][tileIndex];
            Dictionary<(int, int), string> mapping = warningManager.GetLogicalToPhysicalTileMapping();
            string tileName = mapping[(ringIndex, tileIndex)];

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
}
