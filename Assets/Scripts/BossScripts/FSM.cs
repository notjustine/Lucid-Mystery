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
    }

    private int FindClosestTileIndex(Vector3 position, object p)
    {
        throw new NotImplementedException();
    }

    void PerformSteamAttack()
    {
    }


    IEnumerator CooldownRoutine(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);
        currentState = BossState.Idle;
    }

}