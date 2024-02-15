using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    public enum BossState { Idle, PreparingAttack, Attacking, Cooldown }
    public enum BossAttackType { None, Slam, SteamPush, Punishing }

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
    bool IsPlayerInSpecificSlice() { /* Implement logic here */ return true; }
    bool IsPlayerCloseToBoss() { /* Implement logic here */ return true; }
    bool DidPlayerMissBeat() { /* Implement logic here, possibly with an event listener */ return false; }


    void DecideNextAttack()
    {
        if (DidPlayerMissBeat())
        {
            nextAttack = BossAttackType.Punishing;
        }
        else if (IsPlayerCloseToBoss())
        {
            nextAttack = BossAttackType.SteamPush;
        }
        else if (IsPlayerInSpecificSlice())
        {
            nextAttack = BossAttackType.Slam;
        }
        else
        {
            nextAttack = BossAttackType.None;
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
                // Implement Slam attack logic
                break;
            case BossAttackType.SteamPush:
                // Implement Steam Push attack logic
                break;
            case BossAttackType.Punishing:
                // Implement Punishing attack logic
                break;
        }
        // Reset nextAttack after performing it
        nextAttack = BossAttackType.None;
    }

    IEnumerator CooldownRoutine(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);
        currentState = BossState.Idle;
    }

}