using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BossStates : MonoBehaviour
{
    [SerializeField] private ArenaInitializer arenaInitializer;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private SteamAttack steamAttack;
    [SerializeField] private SlamAttack slamAttack;
    [SerializeField] private SprayAttackController sprayAttack;
    [SerializeField] private PlayerControl playerControl;
    public bool isSleeping;
    public enum BossState { Idle, PreparingAttack, Attacking, Cooldown }
    public enum BossAttackType { None, Slam, Steam, Spray }

    BossState currentState = BossState.Idle;
    BossAttackType nextAttack = BossAttackType.None;
    BossAttackType lastAttack = BossAttackType.None;
    private int targetTileIndex = 0; // Example target tile index for the Slam Attack
    private float time = 0f;
    private const float coolDownTime = 5f;

    void Start()
    {
        steamAttack = FindObjectOfType<SteamAttack>();
        slamAttack = FindObjectOfType<SlamAttack>();
        sprayAttack = FindObjectOfType<SprayAttackController>();
        switch (PlayerPrefs.GetInt("bossPhase", 0))
        {
            case 0:
                isSleeping = true;
                break;
            case 1:
                isSleeping = false;
                break;
            case 2:
                isSleeping = false;
                break;
            default:
                isSleeping = true;
                break;
        }
    }
    void Update()
    {
        bossStateMachine();
    }

    void bossStateMachine()
    {
        if (isSleeping)
            return;
        
        // switch (currentState)
        // {
        //     case BossState.Idle:
        //         DecideNextAttack();
        //         break;
        //     case BossState.PreparingAttack:
        //         // call animation here
        //         currentState = BossState.Attacking;
        //         break;
        //     case BossState.Attacking:
        //         Debug.Log("Attempting to Perform Attack");
        //         PerformAttack(nextAttack);
        //         currentState = BossState.Cooldown;
        //         break;
        //     case BossState.Cooldown:
        //         // StartCoroutine(CooldownRoutine(3f));
        //         Cooldown();
        //         break;
        //     
        // }
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
                nextAttack = BossAttackType.Steam;
        }

        if (nextAttack == BossAttackType.None)
        {
            if (lastAttack == BossAttackType.Slam)
            {
                nextAttack = BossAttackType.Spray;
            }
            else
            {
                nextAttack = BossAttackType.Slam;
            }
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
            case BossAttackType.Spray:
                PerformSprayAttack();
                break;
        }

        lastAttack = attackType;
        nextAttack = BossAttackType.None;

    }

    void PerformSteamAttack()
    {
        if (steamAttack != null)
        {
            // Debug.Log("Performing Steam Attack");
            steamAttack.TriggerAttack();
        }
    }

    void PerformSlamAttack()
    {
        if (slamAttack != null)
        {
            // Debug.Log("Performing Slam Attack");
            slamAttack.TriggerAttack(playerControl.currentTileIndex);
        }
    }
    
    void PerformSprayAttack()
    {
        if (sprayAttack != null)
        {
            // Debug.Log("Performing Spray Attack");
            sprayAttack.TriggerShootAndRotate();
        }
    }
    
}
