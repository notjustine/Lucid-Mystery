using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HazardAttack : MonoBehaviour
{
    [SerializeField] private ArenaInitializer arenaInitializer;
    [SerializeField] private GameObject hazardPrefab;
    [SerializeField] private float hazardTiming;
    [SerializeField] private int numberOfHazards;
    private WarningManager warningManager;
    private HashSet<string> activeHazards = new HashSet<string>();

    private void Start()
    {
        warningManager = WarningManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            TriggerAttack();
        }
    }

    public void TriggerAttack()
    {
        hazardTiming = DifficultyManager.Instance.GetValue(DifficultyManager.StatName.HAZARD_TIMING);
        numberOfHazards = (int)DifficultyManager.Instance.GetValue(DifficultyManager.StatName.HAZARD_COUNT);
        StartCoroutine(HazardSequence());
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

        for (int i = 0; i < numberOfHazards && selectedIndices.Count < allTilePositions.Count; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, allTilePositions.Count);
            } while (!selectedIndices.Add(randomIndex));

            Vector3 targetPosition = allTilePositions[randomIndex] + Vector3.up * 10; 
            string tileName = arenaInitializer.GetTileNameByPosition(targetPosition - Vector3.up * 10); 
            targetedTilesNames.Add(tileName);

            Instantiate(hazardPrefab, targetPosition, Quaternion.identity);

            yield return new WaitForSeconds(0.25f);
        }

        warningManager.ToggleWarning(targetedTilesNames, true, WarningManager.WarningType.HAZARD);

        yield return new WaitForSeconds(hazardTiming);

        warningManager.ToggleWarning(targetedTilesNames, false, WarningManager.WarningType.HAZARD);
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
