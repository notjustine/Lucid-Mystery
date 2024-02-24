using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private PlayerControl playerControl;
    private GameObject pauseMenu;
    private GameObject optionsMenu;
    private bool isOptions = false;
    
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
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
        Time.timeScale = 0;
        AudioManager.instance.PauseAllEvents();
    }

    public void HidePauseMenu()
    {
        optionsMenu.SetActive(false);
        isOptions = false;
        playerControl.SwitchPlayerMap("Player");
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        AudioManager.instance.ResumeAllEvents();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowOptions()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
    
    public void goBack()
    {
        if (isOptions)
        {
            isOptions = false;
            optionsMenu.SetActive(false);
            pauseMenu.SetActive(true);
        }
        else
        {
            HidePauseMenu();
        }
    }

    void Start()
    {
        playerControl = FindObjectOfType<PlayerControl>();
        pauseMenu = gameObject.transform.GetChild(0).gameObject;
        optionsMenu = gameObject.transform.GetChild(1).gameObject;
    }
}