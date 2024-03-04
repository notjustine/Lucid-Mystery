using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemicalSprayAttack : MonoBehaviour
{
    public ArenaInitializer arenaInitializer;
    public float attackDelay = 2f;
    public float attackRadius = 1f; // The range within which the attack will affect the player

    // Method to call when triggering the attack
    public void TriggerAttack()
    {
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        // Step 2: Search for the player's position
        PlayerControl playerControl = FindObjectOfType<PlayerControl>(); // Assuming there is only one player

        if (playerControl != null)
        {
            int playerRingIndex = playerControl.getCurrentRingIndex();
            int playerTileIndex = playerControl.getCurrentTileIndex();
            List<Vector3> attackTiles = GetAttackTiles(playerRingIndex, playerTileIndex);

            yield return new WaitForSeconds(attackDelay);

            // Detect whether the player is in the calculated attack area
            if (attackTiles.Contains(playerControl.transform.position))
            {
                PlayerStatus playerStatus = playerControl.GetComponent<PlayerStatus>();
                playerStatus.TakeDamage(5f); // Deal damage
            }
        }
    }

    private List<Vector3> GetAttackTiles(int playerRingIndex, int playerTileIndex)
    {
        List<Vector3> attackTiles = new List<Vector3>();

        // Calculate the indexes for the columns to the left and right of the player
        var currentRing = arenaInitializer.tilePositions[playerRingIndex];
        int leftTileIndex = (playerTileIndex - 1 + currentRing.Count) % currentRing.Count;
        int rightTileIndex = (playerTileIndex + 1) % currentRing.Count;

        // Loop through all rings and add the corresponding tiles to the attackTiles list
        foreach (var ring in arenaInitializer.tilePositions)
        {
            attackTiles.Add(ring[playerTileIndex]); // The column the player is on
            attackTiles.Add(ring[leftTileIndex]);   // The column to the left
            attackTiles.Add(ring[rightTileIndex]);  // The column to the right
        }

        return attackTiles;
    }
}