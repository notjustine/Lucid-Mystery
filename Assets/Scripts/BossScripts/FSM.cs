using System;
using System.Collections;
using UnityEngine;


public class FSM : MonoBehaviour
{
    [SerializeField] private ArenaInitializer arenaInitializer;
    public enum BossState { Idle, PreparingAttack, Attacking, Cooldown }
    public enum BossAttackType { None, Slam, Steam,}

    BossState currentState = BossState.Idle;
    BossAttackType nextAttack = BossAttackType.None;


    void Update()
    {
        switch (currentState)
        {
            case BossState.Idle:
                DecideNextAttack();
                break;
            case BossState.PreparingAttack:
                // for cues
                currentState = BossState.Attacking;
                break;
            case BossState.Attacking:
                PerformAttack(nextAttack);
                currentState = BossState.Cooldown;
                break;
            case BossState.Cooldown:
                // Implement cooldown logic
                currentState = BossState.Idle;
                break;
        }
    }

    // Placeholder methods for tracking player position and beat
    bool IsPlayerInSpecificRing() { /* Implement logic here */ return true; }
    bool IsPlayerCloseToBoss() { /* Implement logic here */ return true; }
    bool DidPlayerMissBeat() { /* Implement logic here, possibly with an event listener */ return false; }


    void DecideNextAttack()
    {
        // Assuming a method IsPlayerClose() that returns true if the player is within a certain distance
        if (IsPlayerCloseToBoss())
        {
            nextAttack = BossAttackType.Steam;
        }
        else // Default to Slam if not close enough, and no specific condition for Punishing in this example
        {
            nextAttack = BossAttackType.Slam;
        }

        if (nextAttack != BossAttackType.None)
        {
            currentState = BossState.PreparingAttack;
        }
    }



    void PerformAttack(BossAttackType attackType)
    {
        switch (attackType)
        {
            case BossAttackType.Slam:
                PerformSlamAttack();
                break;
            case BossAttackType.Steam:
                PerformSteamAttack();
                break;
        }

        // Reset nextAttack after performing it and start cooldown
        nextAttack = BossAttackType.None;
        StartCoroutine(CooldownRoutine(2f)); // Assuming a 2-second cooldown
    }

    void PerformSlamAttack()
    {
        // Calculate the boss's current tile index in its ring (assuming the boss is in the top ring for simplification)
        int bossTileIndex = FindClosestTileIndex(transform.position, arenaInitializer.tilePositions[0]);

        // Iterate through all rings to apply the slam effect on the corresponding tile
        for (int ringIndex = 0; ringIndex < arenaInitializer.tilePositions.Count; ringIndex++)
        {
            var ring = arenaInitializer.tilePositions[ringIndex];
            if (bossTileIndex < ring.Count)
            {
                Vector3 targetTilePosition = ring[bossTileIndex];
                // Assuming a method to check player position and apply effects if they are on the target tile
                CheckAndApplyPlayerEffect(targetTilePosition, "Slam");
            }
        }
    }

    private int FindClosestTileIndex(Vector3 position, object p)
    {
        throw new NotImplementedException();
    }

    void PerformSteamAttack()
    {
        Debug.Log("Performing Steam Attack");

        // Target a 3x4 area around the player
        // For simplicity, let's assume we calculate the player's position and target accordingly
        // You'll need to adapt this logic based on your actual game's rules and structure
    }


    IEnumerator CooldownRoutine(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);
        currentState = BossState.Idle;
    }

}