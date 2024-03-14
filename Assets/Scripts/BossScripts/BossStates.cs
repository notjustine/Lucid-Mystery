using UnityEngine;

public class BossStates : MonoBehaviour
{
    [SerializeField] private ArenaInitializer arenaInitializer;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private SteamAttack steamAttack;
    [SerializeField] private SlamAttack slamAttack;
    [SerializeField] private SprayAttackController sprayAttack;
    [SerializeField] private PlayerControl playerControl;
    [SerializeField] private BossHealth bossHealth;
    public bool isSleeping;
    public enum BossState { Idle, PreparingAttack, Attacking, Cooldown }
    public enum BossAttackType { None, Slam, Steam, Spray }

    BossState currentState = BossState.Idle;
    BossAttackType nextAttack = BossAttackType.None;
    BossAttackType lastAttack = BossAttackType.None;
    private float time = 0f;
    private float coolDownTime = 5f;

    void Start()
    {
        arenaInitializer = FindObjectOfType<ArenaInitializer>();
        steamAttack = FindObjectOfType<SteamAttack>();
        slamAttack = FindObjectOfType<SlamAttack>();
        sprayAttack = FindObjectOfType<SprayAttackController>();
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
        Debug.Log(isSleeping);
        Debug.Log(DifficultyManager.phase);
    }
    void Update()
    {
        bossStateMachine();
    }

    void bossStateMachine()
    {
        if (isSleeping)
            return;

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
        float healthPercentage = (bossHealth.currHealth / bossHealth.maxHealth) * 100f;
        
        if (healthPercentage < 100f && healthPercentage > 80)
        {
            nextAttack = BossAttackType.Spray;
        }

        if (healthPercentage <= 80f && healthPercentage > 65)
        {
            if (IsPlayerInSpecificRing(out ringIndex) && lastAttack != BossAttackType.Steam)
            {
                nextAttack = BossAttackType.Steam;
            }
            else
            {
                nextAttack = BossAttackType.Spray;
            }
        }

        if (healthPercentage <= 65f)
        {
            if (lastAttack == BossAttackType.Steam)
            {
                nextAttack = Random.Range(0, 2) == 0 ? BossAttackType.Slam : BossAttackType.Spray;
            }
            else if (IsPlayerInSpecificRing(out ringIndex) && ringIndex == 0)
            {
                nextAttack = BossAttackType.Steam;
            }
            else
            {
                nextAttack = Random.Range(0, 2) == 0 ? BossAttackType.Slam : BossAttackType.Spray;
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

    public void SetCooldown(float time)
    {
        coolDownTime = time;
    }
}