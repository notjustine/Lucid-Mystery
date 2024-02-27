using UnityEngine;
using System.Collections;

public class SlamAttack : MonoBehaviour
{
    [SerializeField] private ArenaInitializer arenaInitializer;
    [SerializeField] private Material warningMaterial;
    [SerializeField] private Material attackMaterial;
    [SerializeField] private Material idleMaterial;
    public GameObject attackIndicatorPrefab;
    public float warningDuration = 1.0f;
    public float attackDuration = 1f;

    public void TriggerAttack(int tileIndex)
    {
        StartCoroutine(AttackSequence(tileIndex));
    }

    // Assume this part is inside the SlamAttackVisual script

    private IEnumerator AttackSequence(int tileIndex)
    {
        // Display warning at target tiles
        var indicators = new GameObject[arenaInitializer.tilePositions.Count];
        for (int i = 0; i < arenaInitializer.tilePositions.Count; i++)
        {
            if (tileIndex < arenaInitializer.tilePositions[i].Count)
            {
                Vector3 targetPosition = arenaInitializer.tilePositions[i][tileIndex];
                var indicator = Instantiate(attackIndicatorPrefab, targetPosition, Quaternion.identity);
                indicator.GetComponent<MeshRenderer>().material = warningMaterial;
                indicators[i] = indicator;
            }
        }

        yield return new WaitForSeconds(warningDuration);

        // Switch to attack visual and check for player damage
        foreach (var indicator in indicators)
        {
            if (indicator != null)
            {
                indicator.GetComponent<MeshRenderer>().material = attackMaterial;
                // Check if the player is within the attack area (this part needs to be adjusted according to your game's logic)
                Collider[] hitColliders = Physics.OverlapSphere(indicator.transform.position, 1.0f); // Adjust the radius as needed
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.CompareTag("Player")) // Make sure your player GameObject is tagged "Player"
                    {
                        GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
                        playerGameObject.GetComponent<PlayerStatus>().TakeDamage(20f);
                    }
                }
            }
        }

        yield return new WaitForSeconds(attackDuration);

        // Cleanup: revert to idle state and destroy indicators
        foreach (var indicator in indicators)
        {
            if (indicator != null)
            {
                Destroy(indicator);
            }
        }
    }
}
