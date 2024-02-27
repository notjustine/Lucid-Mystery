using System.Collections;
using UnityEngine;

public class BossStates : MonoBehaviour
{
    [SerializeField] private ArenaInitializer arenaInitializer;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private SteamAttack steamAttack;
    [SerializeField] private SlamAttack slamAttack;

    public enum BossState { Idle, PreparingAttack, Attacking, Cooldown }
    public enum BossAttackType { None, Slam, Steam }

    BossState currentState = BossState.Idle;
    BossAttackType nextAttack = BossAttackType.None;
    private int targetTileIndex = 0; // Example target tile index for the Slam Attack


    void Update()
    {
        switch (currentState)
        {
            case BossState.Idle:
                DecideNextAttack();
                break;
            case BossState.PreparingAttack:
                // call animation here
                currentState = BossState.Attacking;
                break;
            case BossState.Attacking:
                PerformAttack(nextAttack);
                currentState = BossState.Cooldown;
                break;
            case BossState.Cooldown:
                currentState = BossState.Idle;
                break;
        }
    }

    bool IsPlayerInSpecificRing(out int ringIndex)
    {
        ringIndex = -1;
        float playerDistanceFromCenter = Vector3.Distance(playerTransform.position, arenaInitializer.transform.position);
        for (int i = 0; i < arenaInitializer.ringRadii.Length; i++)
        {
            if (playerDistanceFromCenter <= arenaInitializer.ringRadii[i])
            {
                ringIndex = i;
                break;
            }
        }
        return ringIndex != -1;
    }

    void DecideNextAttack()
    {
        Debug.Log("Boss Idling");
        int ringIndex;
        if (IsPlayerInSpecificRing(out ringIndex))
        {
            if (ringIndex == 0)
            {
                nextAttack = BossAttackType.Steam;
            }
            else
            {
                nextAttack = BossAttackType.Slam;
            }
        }
        else
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
        nextAttack = BossAttackType.None;
        StartCoroutine(CooldownRoutine(2f)); // Assuming a 2-second cooldown
    }

    void PerformSteamAttack()
    {
        if (steamAttack != null)
        {
            Debug.Log("Performing Steam Attack");
            steamAttack.TriggerAttack();
        }
    }

    void PerformSlamAttack()
    {
        if (slamAttack != null)
        {
            Debug.Log("Performing Steam Attack");
            slamAttack.TriggerAttack(targetTileIndex); 
        }
    }

    IEnumerator CooldownRoutine(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);
        Debug.Log("Boss Attack Finished");
        currentState = BossState.Idle;
    }
}
