using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SkipConfirm : MonoBehaviour
{
    private PlayerControl playerControl;
    [SerializeField] private GameObject confirmationPanel;
    private FadingScreen fade;

    void Start()
    {
        playerControl = FindObjectOfType<PlayerControl>();
        confirmationPanel.SetActive(false);
        fade = FindObjectOfType<FadingScreen>();
    }

    public void OnSkip(InputAction.CallbackContext context)
    {
        Debug.Log("pressed space");
        if (context.phase.Equals(InputActionPhase.Started))
        {
            fade.gameObject.SetActive(false);
            confirmationPanel.SetActive(true);
            Time.timeScale = 0;
            playerControl.SwitchPlayerMap("UI");
        }
    }

    public void HideConfirmation()
    {
        confirmationPanel.SetActive(false);
        Time.timeScale = 1;
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
        FadingScreenManager.Instance.TransitionToScene("PatentEnvironment", 0.5f);

    }


}
