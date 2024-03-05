using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BossStates : MonoBehaviour
{
    [SerializeField] private ArenaInitializer arenaInitializer;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private SteamAttack steamAttack;
    [SerializeField] private SlamAttack slamAttack;
    [SerializeField] private PlayerControl playerControl;

    public enum BossState { Idle, PreparingAttack, Attacking, Cooldown }
    public enum BossAttackType { None, Slam, Steam }

    BossState currentState = BossState.Idle;
    BossAttackType nextAttack = BossAttackType.None;
    private int targetTileIndex = 0; // Example target tile index for the Slam Attack
    private float time = 0f;
    private const float coolDownTime = 5f;

    void Start()
    {
        steamAttack = FindObjectOfType<SteamAttack>();
        slamAttack = FindObjectOfType<SlamAttack>();
    }
    void Update()
    {
        // Debug.Log($"Current State: {currentState}, Next Attack: {nextAttack}");

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
                Debug.Log("Attempting to Perform Attack");
                PerformAttack(nextAttack);
                currentState = BossState.Cooldown;
                break;
            case BossState.Cooldown:
                // StartCoroutine(CooldownRoutine(3f));
                Cooldown();
                break;
            
        }
    }


    bool IsPlayerInSpecificRing(out int ringIndex)
    {
        ringIndex = -1;
        float playerDistanceFromCenter = Vector3.Distance(playerTransform.position, arenaInitializer.transform.position);
        for (int i = 0; i < arenaInitializer.ringRadii.Length; i++)
        {
            float startingRadius = i == 0 ? 0 : arenaInitializer.ringRadii[i];
            float endingRadius = i == arenaInitializer.ringRadii.Length - 1? float.PositiveInfinity : arenaInitializer.ringRadii[i + 1];
           
            if (startingRadius <= playerDistanceFromCenter && playerDistanceFromCenter <= endingRadius)
            {
                ringIndex = i;
                break;
            }
        }
        return ringIndex != -1;
    }

    void Cooldown()
    {
        if (time < coolDownTime)
        {
            time += Time.deltaTime;
            return;
        }

        time = 0f;
        currentState = BossState.Idle;
    }
    void DecideNextAttack()
    {
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
            Debug.Log("Performing Slam Attack");
            slamAttack.TriggerAttack(playerControl.currentTileIndex);
        }
    }
    

    IEnumerator CooldownRoutine(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);
        currentState = BossState.Idle;
    }
}
