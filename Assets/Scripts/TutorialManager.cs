using UnityEngine;
using System;
using TMPro;
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
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] private GameObject centralMachine;
    [SerializeField] private GameObject Phase1HP;
    [SerializeField] private GameObject Phase2HP;
    [SerializeField] private PlayerControl playerControl;
    [SerializeField] private ArenaInitializer arenaInitializer;
    private bool playerHasAttacked = false;

    void Start()
    {
        // Initialize tutorial
        tutorialText.text = "Use directional buttons on the beat to move.";
        centralMachine.SetActive(false);
        Phase1HP.SetActive(false);
        Phase2HP.SetActive(false);
        playerControl.OnAttackEvent += CheckAndSetPlayerAttack;
    }

    void Update()
    {
        switch (currentState)
        {
            case TutorialState.Start:
                if (playerControl.currentRingIndex != arenaInitializer.tilePositions.Count - 1 || playerControl.currentTileIndex != 1)
                {
                    tutorialText.text = "Move up close to the machine to land a hit.";
                    currentState = TutorialState.Move;
                }
                break;
            case TutorialState.Move:
                if (playerControl.currentRingIndex == 0)
                {
                    tutorialText.text = "Press △ ○ X □ to attack the boss.";
                    currentState = TutorialState.ApproachMachine;
                }
                break;
            case TutorialState.ApproachMachine:
                if (playerHasAttacked && playerControl.currentRingIndex == 0)
                {
                    StartCoroutine(ShowAttackMessage());
                } else if (playerControl.currentRingIndex != 0)
                {
                    currentState = TutorialState.Move;
                    playerHasAttacked = false;
                }
                break;
            case TutorialState.Attack:
                SceneManager.LoadScene("PatentEnvironment");
                break;
        }
    }

    System.Collections.IEnumerator ShowAttackMessage()
    {
        tutorialText.text = "Great! ";
        yield return new WaitForSeconds(2f);
        tutorialText.text = "You're ready to face the boss. ";
        yield return new WaitForSeconds(2f);
        tutorialText.text = "";
        playerHasAttacked = false;
        currentState = TutorialState.Attack;
    }

    void CheckAndSetPlayerAttack()
    {
        if (currentState == TutorialState.ApproachMachine && playerControl.currentRingIndex == 0)
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