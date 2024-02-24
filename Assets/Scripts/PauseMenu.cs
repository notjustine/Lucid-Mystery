using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private PlayerControl playerControl;
    [SerializeField] private GameObject obj;
    public void onPause(InputAction.CallbackContext context)
    {
        if (context.phase.Equals(InputActionPhase.Started))
            ShowPauseMenu();
    }

    public void onResume(InputAction.CallbackContext context)
    {
        if (context.phase.Equals(InputActionPhase.Started))
            HidePauseMenu();
    }
    public void ShowPauseMenu()
    {
        playerControl.SwitchPlayerMap("UI");
        obj.SetActive(true);
        Time.timeScale = 0;
        AudioManager.instance.PauseAllEvents();
    }

    public void HidePauseMenu()
    {
        playerControl.SwitchPlayerMap("Player");
        obj.SetActive(false);
        Time.timeScale = 1;
        AudioManager.instance.ResumeAllEvents();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowOptions()
    {
        Debug.Log("Show Options");
    }

    void Start()
    {
        playerControl = FindObjectOfType<PlayerControl>();
    }
}