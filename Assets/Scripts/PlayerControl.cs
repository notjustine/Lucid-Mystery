using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private BeatCheckController beatChecker;
    [SerializeField] private ArenaInitializer arenaInitializer;
    private int currentRingIndex = 0;
    private int currentTileIndex = 0;
    private Animator animator;
    private Transform cameraTransform;
    private PlayerInput input;

    private static readonly int Attack1 = Animator.StringToHash("Attack");

    // Movement Updates
    public bool inputted { get; set; }

    void Start()
    {
        animator = GameObject.FindGameObjectWithTag("Weapon").GetComponent<Animator>();
        beatChecker = FindObjectOfType<BeatCheckController>();
        arenaInitializer = FindObjectOfType<ArenaInitializer>();
        cameraTransform = Camera.main.transform;

        currentRingIndex = arenaInitializer.tilePositions.Count - 1; // Start at the outermost ring
        currentTileIndex = 1; // Start at the first tile of that ring
        MoveToCurrentTile();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!MusicEventHandler.beatCheck || inputted || context.phase != InputActionPhase.Started)
            return;

        AudioManager.instance.PlayOneShotAttached(SoundRef.Instance.movementSound, gameObject);

        Vector2 move = context.ReadValue<Vector2>();
        bool xDominantAxis = Mathf.Abs(move.x) > Mathf.Abs(move.y);

        if (xDominantAxis)
        {
            if (move.x > 0)
                MoveToAdjacentTile(1); // Right
            else if (move.x < 0)
                MoveToAdjacentTile(-1); // Left
        }
        else
        {
            if (move.y > 0 && currentRingIndex > 0)
                MoveToAdjacentRing(-1); // In (towards the center)
            else if (move.y < 0 && currentRingIndex < arenaInitializer.tilePositions.Count - 1)
                MoveToAdjacentRing(1); // Out (away from the center)
        }

        inputted = true;
        beatChecker.SetVulnerable(true);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!MusicEventHandler.beatCheck || inputted || context.phase != InputActionPhase.Started)
            return;

        AudioManager.instance.PlayOneShotAttached(SoundRef.Instance.attackSwing, gameObject);
        animator.SetTrigger(Attack1);
        beatChecker.SetVulnerable(true);
        inputted = true;
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);
    }

    void MoveToAdjacentTile(int direction)
    {
        var currentRing = arenaInitializer.tilePositions[currentRingIndex];
        currentTileIndex = (currentTileIndex + direction + currentRing.Count) % currentRing.Count;
        Debug.Log($"Moving to adjacent tile, direction: {direction}");
        MoveToCurrentTile();
    }

    void MoveToAdjacentRing(int direction)
    {
        currentRingIndex += direction;
        // Ensure the tile index is valid in the new ring
        currentTileIndex = Mathf.Clamp(currentTileIndex, 0, arenaInitializer.tilePositions[currentRingIndex].Count - 1);
        MoveToCurrentTile();
    }

    void MoveToCurrentTile()
    {
        Debug.Log("TileIndex is : " + currentTileIndex);
        Debug.Log("RingIndex is : " + currentRingIndex);

        Vector3 newPosition = arenaInitializer.tilePositions[currentRingIndex][currentTileIndex];
        newPosition.y = 1.6f;
        transform.position = newPosition;
        inputted = true;

    }

    public void SwitchPlayerMap(string map)
    {
        input.SwitchCurrentActionMap(map);
    }
}