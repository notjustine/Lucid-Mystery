using UnityEngine;
using System.Collections;

public class BossStates : MonoBehaviour
{
    [SerializeField] private ArenaInitializer arenaInitializer;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private SteamAttack steamAttack;
    [SerializeField] private SlamAttack slamAttack;
    [SerializeField] private SpiralAttack spiralAttack;
    [SerializeField] private HazardAttack hazardAttack;
    [SerializeField] private PlayerControl playerControl;
    [SerializeField] private BossHealth bossHealth;
    public bool isSleeping;
    public enum BossState { Idle, PreparingAttack, Attacking, Cooldown }
    public enum BossAttackType { None, Slam, Steam, Spiral}

    BossState currentState = BossState.Idle;
    BossAttackType nextAttack = BossAttackType.None;
    BossAttackType lastAttack = BossAttackType.None;
    private float time = 0f;
    private float coolDownTime = 5f;
    private bool isHazardAttackRunning = false;


    void Start()
    {
        arenaInitializer = FindObjectOfType<ArenaInitializer>();
        steamAttack = FindObjectOfType<SteamAttack>();
        slamAttack = FindObjectOfType<SlamAttack>();
        spiralAttack = FindObjectOfType<SpiralAttack>();
        hazardAttack = FindObjectOfType<HazardAttack>();
        bossHealth = FindObjectOfType<BossHealth>();

        switch (DifficultyManager.phase)
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

        // Check if boss health is less than 35% and hazard attack coroutine is not already running
        if (bossHealth.currHealth / bossHealth.maxHealth <= 0.35f && !isHazardAttackRunning)
        {
            StartCoroutine(TriggerHazardAttack());
            isHazardAttackRunning = true; 
        }
    }


    void bossStateMachine()
    {
        if (isSleeping)
        {
            // StopAllCoroutines();            
            return;
        }

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
            float endingRadius = i == arenaInitializer.ringRadii.Length - 1 ? float.PositiveInfinity : arenaInitializer.ringRadii[i + 1];

            if (startingRadius <= playerDistanceFromCenter && playerDistanceFromCenter <= endingRadius)
            {
                ringIndex = i;
                break;
            }
        }
        return ringIndex != -1;
    }


    /**
        Sets the boss state such that the boss will not attack until the state changes again.
    */
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


    /**
        Uses player location and boss health to determine which attack should be executed next by the boss.
    */
    void DecideNextAttack()
    {
        int ringIndex;
        float healthPercentage = (bossHealth.currHealth / bossHealth.maxHealth) * 100f;

        if (healthPercentage < 100f && healthPercentage > 80)
        {
            nextAttack = BossAttackType.Spiral;
        }

        if (healthPercentage <= 80f && healthPercentage > 65)
        {
            if (IsPlayerInSpecificRing(out ringIndex) && lastAttack != BossAttackType.Steam)
            {
                nextAttack = BossAttackType.Steam;
            }
            else
            {
                nextAttack = BossAttackType.Spiral;
            }
        }

        if (healthPercentage <= 65f && healthPercentage > 35f)
        {
            if (lastAttack == BossAttackType.Steam)
            {
                nextAttack = Random.Range(0, 2) == 0 ? BossAttackType.Slam : BossAttackType.Spiral;
            }
            else if (IsPlayerInSpecificRing(out ringIndex) && ringIndex == 0)
            {
                nextAttack = BossAttackType.Steam;
            }
            else
            {
                nextAttack = Random.Range(0, 2) == 0 ? BossAttackType.Slam : BossAttackType.Spiral;
            }
        }

        if (healthPercentage <= 35f)
        {
            if (lastAttack == BossAttackType.Steam)
            {
                int attackChoice = Random.Range(0, 2);
                if (attackChoice == 0)
                {
                    nextAttack = BossAttackType.Slam;
                }
                else
                {
                    nextAttack = BossAttackType.Spiral;
                }

            }
            else if (IsPlayerInSpecificRing(out ringIndex) && ringIndex == 0)
            {
                nextAttack = BossAttackType.Steam;
            }
            else
            {
                int attackChoice = Random.Range(0, 2);
                if (attackChoice == 0)
                {
                    nextAttack = BossAttackType.Slam;
                }
                else
                {
                    nextAttack = BossAttackType.Spiral;
                }
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
            case BossAttackType.Spiral:
                PerformSpiralAttack();
                break;
        }

        lastAttack = attackType;
        nextAttack = BossAttackType.None;

    }

    public void PerformSteamAttack()
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
            slamAttack.TriggerAttack(playerControl.currentTileIndex);
        }
    }


    void PerformSpiralAttack()
    {
        if (spiralAttack != null)
        {
            // Debug.Log("Performing Spiral Attack");
            spiralAttack.TriggerAttack();
        }
    }

    private IEnumerator TriggerHazardAttack()
    {
        while (!isSleeping && bossHealth.currHealth / bossHealth.maxHealth <= 0.35f)
        {
            if (hazardAttack != null)
            {
                hazardAttack.TriggerAttack();
            }
            yield return new WaitForSeconds(10f);
        }
    }


    public void SetCooldown(float time)
    {
        coolDownTime = time;
    }
}