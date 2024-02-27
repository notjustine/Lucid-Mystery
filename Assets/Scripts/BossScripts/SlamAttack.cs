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
    private PlayerStatus playerStatus;
    
    private void Start()
    {
        // Initially set to idle material
        playerStatus = FindObjectOfType<PlayerStatus>();
    }
    public void TriggerAttack(int tileIndex)
    {
        StartCoroutine(AttackSequence(tileIndex));
    }

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

        foreach (var indicator in indicators)
        {
            if (indicator != null)
            {
                indicator.GetComponent<MeshRenderer>().material = attackMaterial;
                Collider[] hitColliders = Physics.OverlapSphere(indicator.transform.position, 3f); 
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.CompareTag("Player")) 
                    {
                        playerStatus.TakeDamage(20f);
                    }
                }
            }
        }

        yield return new WaitForSeconds(attackDuration);

        // revert to idle state and destroy indicators
        foreach (var indicator in indicators)
        {
            if (indicator != null)
            {
                Destroy(indicator);
            }
        }
    }
}
