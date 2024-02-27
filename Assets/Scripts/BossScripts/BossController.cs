using UnityEngine;

public class BossController : MonoBehaviour
{
    private BossState currentState;
    public Transform playerTransform; // Assign this in the Inspector or find it dynamically

    void Start()
    {
        SetState(new SlamAttackState(this)); // Start with slam attack as default
    }

    void Update()
    {
        currentState?.Tick();
        FacePlayer();
        CheckTransitions();
    }

    void CheckTransitions()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer < 10f) // Assuming 10 is the threshold for "close"
        {
            if (!(currentState is SteamAttackState))
                SetState(new SteamAttackState(this));
        }
        else
        {
            if (!(currentState is SlamAttackState))
                SetState(new SlamAttackState(this));
        }
    }

    public void PerformSlamAttack()
    {
        // Implementation of the Slam Attack
        Debug.Log("Performing Slam Attack");
    }

    public void PerformSteamAttack()
    {
        // Implementation of the Steam Attack
        Debug.Log("Performing Steam Attack");
    }

    public void SetState(BossState state)
    {
        currentState?.OnStateExit();
        currentState = state;
        currentState.OnStateEnter();
    }

    void FacePlayer()
    {
        Vector3 directionToPlayer = playerTransform.position - transform.position;
        directionToPlayer.y = 0; // Keep the boss upright
        transform.rotation = Quaternion.LookRotation(directionToPlayer);
    }
}
