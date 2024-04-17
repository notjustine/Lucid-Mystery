using System.Collections;
using UnityEngine;

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
    public enum BossAttackType { None, Slam, Steam, Spiral, Hazard }

    BossState currentState = BossState.Idle;
    BossAttackType nextAttack = BossAttackType.None;
    BossAttackType lastAttack = BossAttackType.None;
    private float time = 0f;
    private float coolDownTime = 5f;
    private bool isHazardAttackRunning = false;

    private DifficultyManager difficultyManager;


    void Start()
    {
        arenaInitializer = FindObjectOfType<ArenaInitializer>();
        steamAttack = FindObjectOfType<SteamAttack>();
        slamAttack = FindObjectOfType<SlamAttack>();
        spiralAttack = FindObjectOfType<SpiralAttack>();
        hazardAttack = FindObjectOfType<HazardAttack>();
        bossHealth = FindObjectOfType<BossHealth>();
        difficultyManager = DifficultyManager.Instance;

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
        DifficultyManager.Difficulty difficultyLevel = difficultyManager.CurrDifficulty;
        bossStateMachine();

        if (!isSleeping && !isHazardAttackRunning && bossHealth.currHealth / bossHealth.maxHealth <= GetHazardTriggerThreshold() && difficultyLevel != DifficultyManager.Difficulty.EASY)
        {
            StartCoroutine(TriggerHazardAttack());
            isHazardAttackRunning = true;
        }
    }

    float GetHazardTriggerThreshold()
    {
        DifficultyManager.Difficulty difficultyLevel = difficultyManager.CurrDifficulty;
        switch (difficultyLevel)
        {
            case DifficultyManager.Difficulty.EASY:
                return float.MaxValue; // Never triggers in easy mode
            case DifficultyManager.Difficulty.MEDIUM:
                return 0.25f;
            case DifficultyManager.Difficulty.HARD:
                return 0.50f;
            case DifficultyManager.Difficulty.INSANE:
                return 0.75f;
            default:
                return 1.0f;
        }
    }

    private IEnumerator TriggerHazardAttack()
    {
        while (bossHealth.currHealth / bossHealth.maxHealth <= GetHazardTriggerThreshold())
        {
            hazardAttack.TriggerAttack();
            yield return new WaitForSeconds(difficultyManager.GetValue(DifficultyManager.StatName.HAZARD_TIMING)+0.5f);
        }
        isHazardAttackRunning = false;
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

    void DecideNextAttack()
    {
        int ringIndex;
        float healthPercentage = (bossHealth.currHealth / bossHealth.maxHealth) * 100f;
        DifficultyManager.Difficulty difficultyLevel = difficultyManager.CurrDifficulty;

        nextAttack = BossAttackType.None;
        switch (difficultyLevel)
        {
            case DifficultyManager.Difficulty.EASY:
                if (healthPercentage < 100f && healthPercentage > 50f) {
                    nextAttack = Random.Range(0, 2) == 0 ? BossAttackType.Slam : BossAttackType.Spiral;
                }
                if (healthPercentage <= 50f)
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
                break;

            case DifficultyManager.Difficulty.MEDIUM:
                if (healthPercentage < 100f && healthPercentage > 50f)
                {
                    nextAttack = Random.Range(0, 2) == 0 ? BossAttackType.Slam : BossAttackType.Spiral;
                }
                if (healthPercentage <= 75f)
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
                break;

            case DifficultyManager.Difficulty.HARD:
                if (healthPercentage < 100f && healthPercentage > 50f)
                {
                    nextAttack = Random.Range(0, 2) == 0 ? BossAttackType.Slam : BossAttackType.Spiral;
                }
                if (healthPercentage <= 75f)
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
                break;

            case DifficultyManager.Difficulty.INSANE:
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
                break;
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
            spiralAttack.TriggerAttack();
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

    public void SetCooldown(float time)
    {
        coolDownTime = time;
    }
}