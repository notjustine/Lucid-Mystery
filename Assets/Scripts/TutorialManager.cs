using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    [SerializeField] private GameObject comboImage;
    [SerializeField] private PlayerControl playerControl;
    [SerializeField] private GameObject Sniper;
    [SerializeField] private ArenaInitializer arenaInitializer;
    [SerializeField] private Image skip;
    [SerializeField] private Image onBeat;
    [SerializeField] private Image directions;
    [SerializeField] private Image hit;
    [SerializeField] private Image attack;
    private Attack playerAtk;

    private bool playerHasAttacked = false;

    void Start()
    {
        // Initialize tutorial
        skip.enabled = true;
        directions.enabled = true;
        onBeat.enabled = false;
        hit.enabled = false;
        attack.enabled = false;
        centralMachine.SetActive(false);
        Phase1HP.SetActive(false);
        Phase2HP.SetActive(false);
        comboImage.SetActive(false);
        Sniper.SetActive(false);
        playerControl.OnAttackEvent += CheckAndSetPlayerAttack;
        playerControl.OnMoveEvent += CheckAndSetPlayerMove;
        playerAtk = GameObject.FindGameObjectWithTag("Weapon").GetComponent<Attack>();
        if (playerAtk)
        {
            Debug.Log("disable weapon attack");
            playerAtk.enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("PatentEnvironment");
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
                Sniper.SetActive(true);
                if (playerControl.currentRingIndex != arenaInitializer.tilePositions.Count - 1 || playerControl.currentTileIndex != 1)
                {
                    onBeat.enabled = false;
                    currentState = TutorialState.Strengthen;
                }
                break;
            case TutorialState.Strengthen:
                Debug.Log("Moving on beat consecutively strengthens you");
                comboImage.SetActive(true);
                playerAtk.enabled = true;
                if (playerAtk.getCombo() == 5)
                {
                    currentState = TutorialState.ApproachMachine;
                }
                break;
            case TutorialState.ApproachMachine:
                hit.enabled = true;
                if (playerControl.currentRingIndex == 0)
                {
                    currentState = TutorialState.Attack;
                }
                
                break;
            case TutorialState.Attack:
                hit.enabled = false;
                attack.enabled = true;
                if (playerHasAttacked && playerControl.currentRingIndex == 0)
                {
                    attack.enabled = false;
                    playerHasAttacked = false;
                    currentState = TutorialState.End;
                }
                else if (playerControl.currentRingIndex != 0)
                {
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
        SceneManager.LoadScene("PatentEnvironment");
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

    void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        if (playerControl != null)
        {
            playerControl.OnAttackEvent -= CheckAndSetPlayerAttack;
            playerControl.OnMoveEvent -= CheckAndSetPlayerMove;
        }
    }
}
