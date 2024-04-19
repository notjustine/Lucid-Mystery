using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SkipConfirm : MonoBehaviour
{
    private PlayerControl playerControl;
    [SerializeField] private GameObject confirmationPanel;
    private FadingScreen fade;
    private EventSystem eventSystem;

    void Awake()
    {
        eventSystem = GetComponent<EventSystem>();
        playerControl = FindObjectOfType<PlayerControl>();
        confirmationPanel.SetActive(false);
        fade = FindObjectOfType<FadingScreen>();
        if (fade == null)
        {
            Debug.Log("can't find fade");
        }
    }

    public void OnSkip(InputAction.CallbackContext context)
    {
        Debug.Log("on skip");
        if (context.phase.Equals(InputActionPhase.Started))
        {
            ShowConfirmation();
        }
    }

    public void ShowConfirmation()
    {
        eventSystem.SetSelectedGameObject(GameObject.Find("No"));
        fade.gameObject.SetActive(false);
        confirmationPanel.SetActive(true);
        Time.timeScale = 0f;
        playerControl.SwitchPlayerMap("UI");
    }


    public void HideConfirmation()
    {
        Time.timeScale = 1f;
        confirmationPanel.SetActive(false);
        playerControl.SwitchPlayerMap("Player");
        fade.gameObject.SetActive(true);
    }

    public void CancleSkip()
    {
        HideConfirmation();
    }

    public void ConfirmSkip()
    {
        HideConfirmation();
        // FadingScreenManager.Instance.TransitionToScene("ZyngaMain", 2f);
        FadingScreenManager.Instance.AsyncTransitionToScene( 1f, TutorialManager.Instance.asyncLoad);
    }


}
