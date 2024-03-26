using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public enum TutorialState
{
    Start,
    OnBeat,
    Strengthen,
    ApproachMachine,
    Attack,
    End
}

public class TutorialManager : MonoBehaviour
{
    private FadingScreen fade;
    public TutorialState currentState = TutorialState.Start;
    [SerializeField] private GameObject centralMachine;
    [SerializeField] private GameObject Phase1HP;
    [SerializeField] private GameObject Phase2HP;
    private Image comboImage;
    private PlayerControl playerControl;
    private ArenaInitializer arenaInitializer;
    private WarningManager warningManager;
    [SerializeField] private Image skip;
    [SerializeField] private Image onBeat;
    [SerializeField] private Image directions;
    [SerializeField] private Image hit;
    [SerializeField] private Image attack;
    [SerializeField] private Image consecutive;
    [SerializeField] private Attack playerAtk;
    [SerializeField] private SniperAttack sniper;
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

    private bool playerHasAttacked = false;

    void Start()
    {
        // Initialize tutorial
        fade = FindObjectOfType<FadingScreen>();
        skip.enabled = true;
        directions.enabled = true;
        onBeat.enabled = false;
        hit.enabled = false;
        consecutive.enabled = false;
        attack.enabled = false;
        sniper.enabled = false;
        highlightCombo.enabled = false;
        highlightBeat.enabled = false;
        warningManager = WarningManager.Instance;
        warningManager.enabled = false;
        centralMachine.SetActive(false);
        Phase1HP.SetActive(false);
        Phase2HP.SetActive(false);
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
    }

    void Update()
    {
        switch (currentState)
        {
            case TutorialState.Start:
                directions.enabled = true;
                // leave to the events
                break;
            case TutorialState.OnBeat:
                if (!BeatRunning)
                {
                    BeatRunning = true;
                    StartCoroutine(HandleOnBeat());
                }
                break;
            case TutorialState.Strengthen:
                if (!StrengthenRunning)
                {
                    StrengthenRunning = true;
                    StartCoroutine(HandleStrengthen());
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
                attack.enabled = true;
                if (playerHasAttacked && playerControl.currentRingIndex == 0)
                {
                    attack.enabled = false;
                    playerHasAttacked = false;
                    currentState = TutorialState.End;
                }
                else if (playerControl.currentRingIndex != 0)
                {
                    attack.enabled = false;
                    currentState = TutorialState.ApproachMachine;
                    playerHasAttacked = false;
                }
                break;
            case TutorialState.End:
                FadingScreenManager.Instance.TransitionToScene("PatentEnvironment", 1f);
                break;
        }
    }

    private System.Collections.IEnumerator HandleOnBeat()
    {
        directions.enabled = false;
        onBeat.enabled = true;
        if (!isBeatCoroutineRunning)
        {
            isBeatCoroutineRunning = true;
            yield return StartCoroutine(HandleOnBeatState());
        }
        if (playerControl.currentRingIndex != initPosRing || playerControl.currentTileIndex != initPosTile)
        {
            initPosRing = playerControl.currentRingIndex;
            initPosTile = playerControl.currentTileIndex;
            Debug.Log("adding movecount");
            moveCount += 1;
        }
        if (moveCount >= 3)
        {
            onBeat.enabled = false;
            currentState = TutorialState.Strengthen;
        }
        BeatRunning = false;
    }

    private System.Collections.IEnumerator HandleOnBeatState()
    {
        directions.enabled = false;
        onBeat.enabled = true;
        highlightBeat.enabled = true;

        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => Input.anyKey);
        highlightBeat.enabled = false;
    }


    private System.Collections.IEnumerator HandleStrengthen()
    {
        warningManager.enabled = true;
        sniper.enabled = true;
        consecutive.enabled = true;
        if (!isStrengthenCoroutineRunning)
        {
            isStrengthenCoroutineRunning = true;
            yield return StartCoroutine(HandleStrengthenState());
        }
        comboImage.GetComponent<CanvasRenderer>().SetAlpha(100f);
        if (playerAtk.getCombo() == 5)
        {
            consecutive.enabled = false;
            currentState = TutorialState.ApproachMachine;
        }
        StrengthenRunning = false;
    }

    private System.Collections.IEnumerator HandleStrengthenState()
    {
        highlightCombo.enabled = true;

        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => Input.anyKey);
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
        warningManager.ToggleWarning(GetWarningTiles(), true, WarningManager.WarningType.STEAM);
        hit.enabled = true;
        yield return new WaitUntil(() => playerControl.currentRingIndex == 0);
        hit.enabled = false;
        warningManager.ToggleWarning(GetWarningTiles(), false, WarningManager.WarningType.STEAM);
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
    public List<string> GetWarningTiles()
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
