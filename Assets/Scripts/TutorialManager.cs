using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum TutorialState
{
    Start,
    Move,
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
    [SerializeField] private PlayerControl playerControl;
    [SerializeField] private ArenaInitializer arenaInitializer;
    [SerializeField] private Image skip;
    [SerializeField] private Image onBeat;
    [SerializeField] private Image directions;
    [SerializeField] private Image hit;
    [SerializeField] private Image attack;
    private AsyncOperation asyncSceneLoad;

    private bool playerHasAttacked = false;
    private FadingScreen fade;

    void Start()
    {
        fade = FindObjectOfType<FadingScreen>();
        StartCoroutine(fade.FadeFromBlack(1f));
        asyncSceneLoad = SceneManager.LoadSceneAsync("PatentEnvironment");
        asyncSceneLoad.allowSceneActivation = false;
        // Initialize tutorial
        skip.enabled = true;
        onBeat.enabled = true;
        directions.enabled = false;
        hit.enabled = false;
        attack.enabled = false;
        centralMachine.SetActive(false);
        Phase1HP.SetActive(false);
        Phase2HP.SetActive(false);
        playerControl.OnAttackEvent += CheckAndSetPlayerAttack;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FadingScreenManager.Instance.AsyncTransitionToScene(1f, asyncSceneLoad);
        }
        
        switch (currentState)
        {
            case TutorialState.Start:
                if (Input.anyKey)
                {
                    currentState = TutorialState.Move;
                }
                
                break;
            case TutorialState.Move:
                onBeat.enabled = false;
                directions.enabled = true;
                if (playerControl.currentRingIndex != arenaInitializer.tilePositions.Count - 1 || playerControl.currentTileIndex != 1)
                {
                    directions.enabled = false;
                    hit.enabled = true;
                    currentState = TutorialState.ApproachMachine;
                }
                
                break;
            case TutorialState.ApproachMachine:
                if (playerControl.currentRingIndex == 0)
                {
                    hit.enabled = false;
                    attack.enabled = true;
                    currentState = TutorialState.Attack;
                }
                
                break;
            case TutorialState.Attack:
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
                FadingScreenManager.Instance.AsyncTransitionToScene(1f, asyncSceneLoad);
                break;
        }
    }
    
    void CheckAndSetPlayerAttack()
    {
        if (currentState == TutorialState.Attack && playerControl.currentRingIndex == 0)
        {
            playerHasAttacked = true;
        }
    }

    void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        if (playerControl != null)
        {
            playerControl.OnAttackEvent -= CheckAndSetPlayerAttack;
        }
    }
}
