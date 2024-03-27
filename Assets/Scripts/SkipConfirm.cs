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
        if (fade == null)
        {
            Debug.Log("can't find fade");
        }
    }

    public void OnSkip(InputAction.CallbackContext context)
    {
        Debug.Log("pressed space");
        if (context.phase.Equals(InputActionPhase.Started))
        {
            ShowConfirmation();
        }
    }

    public void ShowConfirmation()
    {
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
        FadingScreenManager.Instance.TransitionToScene("ZyngaMain", 2f);
    }


}
