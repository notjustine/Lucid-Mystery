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

    int initPosRing;
    int initPosTile;
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
        warningManager.enabled = false;
        arenaInitializer = FindObjectOfType<ArenaInitializer>();
        playerControl = FindObjectOfType<PlayerControl>();
        comboImage = GameObject.FindGameObjectWithTag("ComboMeter").GetComponent<Image>();
        comboImage.GetComponent<CanvasRenderer>().SetAlpha(0f);
        playerControl.OnAttackEvent += CheckAndSetPlayerAttack;
        playerControl.OnMoveEvent += CheckAndSetPlayerMove;
        initPosRing = arenaInitializer.tilePositions.Count - 1;
        initPosTile = 1;
        moveCount = 0;
        isBeatCoroutineRunning = false;
        BeatRunning = false;
        approachRunning = false;
        isStrengthenCoroutineRunning = false;
        StrengthenRunning = false;
        healRunning = false;
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
                    StartCoroutine(HandleHeal());
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
                // need a "now, you are ready to face the boss"
                FadingScreenManager.Instance.TransitionToScene("ZyngaMain", 1f);
                break;
        }
    }

    private System.Collections.IEnumerator HandleOnBeat()
    {
        instructions.SetInstructionType(TutorialInstruction.SpriteType.OnBeat);
        warningManager.enabled = true;
        if (!isBeatCoroutineRunning)
        {
            isBeatCoroutineRunning = true;
            yield return StartCoroutine(HandleOnBeatState());
        }
        instructions.SetInstructionType(TutorialInstruction.SpriteType.Sniper);
        if (playerControl.currentRingIndex != initPosRing || playerControl.currentTileIndex != initPosTile)
        {
            initPosRing = playerControl.currentRingIndex;
            initPosTile = playerControl.currentTileIndex;
            moveCount += 1;
            Debug.Log("adding moveCount");
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

        yield return new WaitForSeconds(1.5f);
        //yield return new WaitUntil(() => Input.anyKey);
        highlightBeat.enabled = false;
    }

    private System.Collections.IEnumerator HandleHeal()
    {
        instructions.SetInstructionType(TutorialInstruction.SpriteType.Heal);
        yield return new WaitForSeconds(2f);
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
        if (playerAtk.getCombo() == 5)
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
        if (currentState == TutorialState.Start)
        {
            currentState = TutorialState.OnBeat;
        }
    }

    private System.Collections.IEnumerator HandleApporachMachine()
    {
        Debug.Log("In handle approach machine");
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
}
