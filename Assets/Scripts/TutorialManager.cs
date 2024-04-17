using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public enum TutorialState
{
    Start,
    OnBeat, // also explain sniper here
    Heal, // also explain different indicators here
    ApproachMachine,
    Attack,
    Strengthen,
    Avoid,
    End
}

public class TutorialManager : MonoBehaviour, IWarningGenerator
{
    private FadingScreen fade;
    public TutorialState currentState = TutorialState.Start;
    private Image comboImage;
    private PlayerControl playerControl;
    private ArenaInitializer arenaInitializer;
    private WarningManager warningManager;
    private TutorialInstruction instructions;
    [SerializeField] private Attack playerAtk;
    [SerializeField] private Image highlightCombo;
    [SerializeField] private Image highlightBeat;
    [SerializeField] private GameObject healthKit;
    private Dictionary<(int, int), string> logicalToPhysicalTileMapping;
    private List<string> movingTiles;

    int initPosRing;
    int initPosTile;
    bool toggled;
    int moveCount;
    bool isBeatCoroutineRunning;
    bool BeatRunning;
    bool approachRunning;
    bool isStrengthenCoroutineRunning;
    bool StrengthenRunning;
    bool healRunning;

    private bool playerHasAttacked = false;
    public static TutorialManager Instance { get; private set; }

    void Start()
    {
        // Initialize tutorial
        instructions = FindObjectOfType<TutorialInstruction>();
        instructions.SetInstructionType(TutorialInstruction.SpriteType.Start);

        fade = FindObjectOfType<FadingScreen>();
        highlightCombo.enabled = false;
        highlightBeat.enabled = false;
        warningManager = WarningManager.Instance;
        arenaInitializer = FindObjectOfType<ArenaInitializer>();
        playerControl = FindObjectOfType<PlayerControl>();
        comboImage = GameObject.FindGameObjectWithTag("ComboMeter").GetComponent<Image>();
        comboImage.GetComponent<CanvasRenderer>().SetAlpha(0f);
        playerControl.OnAttackEvent += CheckAndSetPlayerAttack;
        playerControl.OnMoveEvent += CheckAndSetPlayerMove;
        initPosRing = arenaInitializer.tilePositions.Count - 1;
        initPosTile = 1;
        toggled = false;
        moveCount = 0;
        isBeatCoroutineRunning = false;
        BeatRunning = false;
        approachRunning = false;
        isStrengthenCoroutineRunning = false;
        StrengthenRunning = false;
        healRunning = false;
        InitLogicToPhysMapping();
        
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one Tutorial Manager");
        }
        Instance = this;
    }

    void Update()
    {
        switch (currentState)
        {
            case TutorialState.Start:
                // leave to the events
                if (!toggled)
                {
                    movingTiles = GetMoveTiles();
                    warningManager.ToggleWarning(movingTiles, true, WarningManager.WarningType.INFO);
                    toggled = true;
                }
                break;
            case TutorialState.OnBeat:
                //need a "sniper would fire if any action is not on beat"
                if (!BeatRunning)
                {
                    BeatRunning = true;
                    StartCoroutine(HandleOnBeat());
                }
                break;

            case TutorialState.Heal:
                if (!healRunning)
                {
                    healRunning = true;
                    instructions.SetInstructionType(TutorialInstruction.SpriteType.Heal);
                    SpawnKit("R3_22");
                }
                break;
            case TutorialState.ApproachMachine:
                if (!approachRunning)
                {
                    approachRunning = true;
                    StartCoroutine(HandleApporachMachine());
                }
                break;
            case TutorialState.Attack:
                instructions.SetInstructionType(TutorialInstruction.SpriteType.Attack);
                if (playerHasAttacked && playerControl.currentRingIndex == 0)
                {
                    playerHasAttacked = false;
                    currentState = TutorialState.Strengthen; // hit the boss
                }
                else if (playerControl.currentRingIndex != 0)
                {
                    currentState = TutorialState.ApproachMachine; // went back before hitting boss
                    playerHasAttacked = false;
                }
                break;
            case TutorialState.Strengthen:
                // need a "attacking on beat consecutively boosts your damage"
                if (!StrengthenRunning)
                {
                    StrengthenRunning = true;
                    StartCoroutine(HandleStrengthen());
                }
                break;
            case TutorialState.End:
                instructions.SetDisplayImageAlpha(0f);
                FadingScreenManager.Instance.TransitionToScene("ZyngaMain", 1f);
                break;
        }
    }

    private System.Collections.IEnumerator HandleOnBeat()
    {
        
        if (!isBeatCoroutineRunning)
        {
            instructions.SetInstructionType(TutorialInstruction.SpriteType.OnBeat);
            isBeatCoroutineRunning = true;
            yield return StartCoroutine(HandleOnBeatState());
        }
        if (playerControl.currentRingIndex != initPosRing || playerControl.currentTileIndex != initPosTile)
        {
            initPosRing = playerControl.currentRingIndex;
            initPosTile = playerControl.currentTileIndex;
            moveCount += 1;
            //Debug.Log("adding moveCount");
        }
        if (moveCount >= 3)
        {
            currentState = TutorialState.Heal;
        }
        BeatRunning = false;
    }

    private System.Collections.IEnumerator HandleOnBeatState()
    {
        highlightBeat.enabled = true;

        yield return new WaitForSeconds(1f);
        instructions.SetInstructionType(TutorialInstruction.SpriteType.Sniper);
        yield return new WaitForSeconds(0.5f);
        //yield return new WaitUntil(() => Input.anyKey);
        highlightBeat.enabled = false;
        
    }

    //private System.Collections.IEnumerator HandleHeal()
    //{
        
    //}

    public void SpawnKit(string tilename)
    {
        GameObject tile = GameObject.Find(tilename);
        Transform tileTransform = tile.GetComponent<Transform>();
        Vector3 kitPosition = new Vector3(tileTransform.position.x, 1.25f, tileTransform.position.z);
        GameObject kitInstance = Instantiate(healthKit, kitPosition, Quaternion.identity);
        HealKitTutorialController kitController = kitInstance.GetComponent<HealKitTutorialController>();
        Animator animator = kitController.GetComponentInChildren<Animator>();
        animator.SetBool("isActive", true);
        kitController.OnHealEvent += HandleKitDestroyed;
    }

    private void HandleKitDestroyed()
    {
        HealKitTutorialController[] kits = FindObjectsOfType<HealKitTutorialController>();
        foreach (var kit in kits)
        {
            kit.OnHealEvent -= HandleKitDestroyed;
        }
        currentState = TutorialState.ApproachMachine;
    }

    private System.Collections.IEnumerator HandleStrengthen()
    {
        instructions.SetInstructionType(TutorialInstruction.SpriteType.Strengthen);
        comboImage.GetComponent<CanvasRenderer>().SetAlpha(100f);
        if (!isStrengthenCoroutineRunning)
        {
            isStrengthenCoroutineRunning = true;

            yield return StartCoroutine(HandleStrengthenState());
        }
        if (playerAtk.getCombo() >= 5)
        {
            currentState = TutorialState.End;
        }
        StrengthenRunning = false;
    }

    private System.Collections.IEnumerator HandleStrengthenState()
    {
        highlightCombo.enabled = true;

        yield return new WaitForSeconds(1.5f);
        //yield return new WaitUntil(() => Input.anyKey);
        highlightCombo.enabled = false;
    }

    void CheckAndSetPlayerAttack()
    {
        if (currentState == TutorialState.Attack && playerControl.currentRingIndex == 0)
        {
            playerHasAttacked = true;
        }
    }
    void CheckAndSetPlayerMove()
    {
        warningManager.ToggleWarning(movingTiles, false, WarningManager.WarningType.INFO);
        if (currentState == TutorialState.Start)
        {
            
            currentState = TutorialState.OnBeat;
        }
    }

    private System.Collections.IEnumerator HandleApporachMachine()
    {
        //Debug.Log("In handle approach machine");
        instructions.SetInstructionType(TutorialInstruction.SpriteType.ApproachMachine);
        warningManager.ToggleWarning(GetWarningObjects(), true, WarningManager.WarningType.INFO);
        yield return new WaitUntil(() => playerControl.currentRingIndex == 0);
        warningManager.ToggleWarning(GetWarningObjects(), false, WarningManager.WarningType.INFO);
        currentState = TutorialState.Attack;
        approachRunning = false;
    }

    void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        if (playerControl != null)
        {
            playerControl.OnAttackEvent -= CheckAndSetPlayerAttack;
            playerControl.OnMoveEvent -= CheckAndSetPlayerMove;
        }
    }

    public List<string> GetMoveTiles() // for directions state
    {
        List<string> tiles = new List<string>();
        int currentIndex = playerControl.currentTileIndex;
        int currentRing = playerControl.currentRingIndex;
        int leftIndex = (currentIndex - 1 + 24) % 24;
        int rightIndex = (currentIndex + 1) % 24;
        int backIndex = currentRing + 1;
        int frontIndex = currentRing - 1;
        tiles.Add(logicalToPhysicalTileMapping[(playerControl.currentRingIndex, leftIndex)]);
        tiles.Add(logicalToPhysicalTileMapping[(playerControl.currentRingIndex, rightIndex)]);
        if (playerControl.currentRingIndex > 0)
        {
            tiles.Add(logicalToPhysicalTileMapping[(frontIndex, currentIndex)]);
        }
        if (playerControl.currentRingIndex < 3)
        {
            tiles.Add(logicalToPhysicalTileMapping[(backIndex, currentIndex)]);
        }
        return tiles;
    }
    public List<string> GetWarningObjects()
    {
        return new List<string> {
            "R1_01",
            "R1_02",
            "R1_03",
            "R1_04",
            "R1_05",
            "R1_06",
            "R1_07",
            "R1_08",
            "R1_09",
            "R1_10",
            "R1_11",
            "R1_12",
            "R1_13",
            "R1_14",
            "R1_15",
            "R1_16",
            "R1_17",
            "R1_18",
            "R1_19",
            "R1_20",
            "R1_21",
            "R1_22",
            "R1_23",
            "R1_24",
            };
    }
    private void InitLogicToPhysMapping()
    {
        logicalToPhysicalTileMapping = new Dictionary<(int, int), string>
        {
            {(0, 0), "R1_18"},
            {(0, 1), "R1_17"},
            {(0, 2), "R1_16"},
            {(0, 3), "R1_15"},
            {(0, 4), "R1_14"},
            {(0, 5), "R1_13"},
            {(0, 6), "R1_12"},
            {(0, 7), "R1_11"},
            {(0, 8), "R1_10"},
            {(0, 9), "R1_09"},
            {(0, 10), "R1_08"},
            {(0, 11), "R1_07"},
            {(0, 12), "R1_06"},
            {(0, 13), "R1_05"},
            {(0, 14), "R1_04"},
            {(0, 15), "R1_03"},
            {(0, 16), "R1_02"},
            {(0, 17), "R1_01"},
            {(0, 18), "R1_24"},
            {(0, 19), "R1_23"},
            {(0, 20), "R1_22"},
            {(0, 21), "R1_21"},
            {(0, 22), "R1_20"},
            {(0, 23), "R1_19"},

            {(1, 0), "R2_18"},
            {(1, 1), "R2_17"},
            {(1, 2), "R2_16"},
            {(1, 3), "R2_15"},
            {(1, 4), "R2_14"},
            {(1, 5), "R2_13"},
            {(1, 6), "R2_12"},
            {(1, 7), "R2_11"},
            {(1, 8), "R2_10"},
            {(1, 9), "R2_09"},
            {(1, 10), "R2_08"},
            {(1, 11), "R2_07"},
            {(1, 12), "R2_06"},
            {(1, 13), "R2_05"},
            {(1, 14), "R2_04"},
            {(1, 15), "R2_03"},
            {(1, 16), "R2_02"},
            {(1, 17), "R2_01"},
            {(1, 18), "R2_24"},
            {(1, 19), "R2_23"},
            {(1, 20), "R2_22"},
            {(1, 21), "R2_21"},
            {(1, 22), "R2_20"},
            {(1, 23), "R2_19"},

            {(2, 0), "R3_18"},
            {(2, 1), "R3_17"},
            {(2, 2), "R3_16"},
            {(2, 3), "R3_15"},
            {(2, 4), "R3_14"},
            {(2, 5), "R3_13"},
            {(2, 6), "R3_12"},
            {(2, 7), "R3_11"},
            {(2, 8), "R3_10"},
            {(2, 9), "R3_09"},
            {(2, 10), "R3_08"},
            {(2, 11), "R3_07"},
            {(2, 12), "R3_06"},
            {(2, 13), "R3_05"},
            {(2, 14), "R3_04"},
            {(2, 15), "R3_03"},
            {(2, 16), "R3_02"},
            {(2, 17), "R3_01"},
            {(2, 18), "R3_24"},
            {(2, 19), "R3_23"},
            {(2, 20), "R3_22"},
            {(2, 21), "R3_21"},
            {(2, 22), "R3_20"},
            {(2, 23), "R3_19"},

            {(3, 0), "R4_18"},
            {(3, 1), "R4_17"},
            {(3, 2), "R4_16"},
            {(3, 3), "R4_15"},
            {(3, 4), "R4_14"},
            {(3, 5), "R4_13"},
            {(3, 6), "R4_12"},
            {(3, 7), "R4_11"},
            {(3, 8), "R4_10"},
            {(3, 9), "R4_09"},
            {(3, 10), "R4_08"},
            {(3, 11), "R4_07"},
            {(3, 12), "R4_06"},
            {(3, 13), "R4_05"},
            {(3, 14), "R4_04"},
            {(3, 15), "R4_03"},
            {(3, 16), "R4_02"},
            {(3, 17), "R4_01"},
            {(3, 18), "R4_24"},
            {(3, 19), "R4_23"},
            {(3, 20), "R4_22"},
            {(3, 21), "R4_21"},
            {(3, 22), "R4_20"},
            {(3, 23), "R4_19"},
        };
    }
}
