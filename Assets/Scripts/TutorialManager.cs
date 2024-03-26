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
    int initPosRing;
    int initPosTile;
    int moveCount;

    private bool playerHasAttacked = false;

    void Start()
    {
        // Initialize tutorial
        skip.enabled = true;
        directions.enabled = true;
        onBeat.enabled = false;
        hit.enabled = false;
        consecutive.enabled = false;
        attack.enabled = false;
        sniper.enabled = false;
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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame))
        {
            Invoke("delayEnd", 0.3f);
        }
        switch (currentState)
        {
            case TutorialState.Start:
                directions.enabled = true;
                // leave to the events
                break;
            case TutorialState.OnBeat:
                directions.enabled = false;
                onBeat.enabled = true;

                if (playerControl.currentRingIndex != initPosRing || playerControl.currentTileIndex != initPosTile)
                {
                    initPosRing = playerControl.currentRingIndex;
                    initPosTile = playerControl.currentTileIndex;
                    moveCount += 1;
                }
                if (moveCount >= 3)
                {
                    onBeat.enabled = false;
                    currentState = TutorialState.Strengthen;
                }
                break;
            case TutorialState.Strengthen:
                warningManager.enabled = true;
                sniper.enabled = true;
                consecutive.enabled = true;
                comboImage.GetComponent<CanvasRenderer>().SetAlpha(100f);
                if (playerAtk.getCombo() == 5)
                {
                    consecutive.enabled = false;
                    currentState = TutorialState.ApproachMachine;
                }
                break;
            case TutorialState.ApproachMachine:
                StartCoroutine(HandleApporachMachine());
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
                Invoke("delayEnd", 0.3f);
                break;
        }
    }

    void delayEnd()
    {
        SceneManager.LoadScene("ZyngaMain");
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
        // Wait until the player reaches the specified ring index
        yield return new WaitUntil(() => playerControl.currentRingIndex == 0);

        hit.enabled = false;
        warningManager.ToggleWarning(GetWarningTiles(), false, WarningManager.WarningType.STEAM);
        currentState = TutorialState.Attack;
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
