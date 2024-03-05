using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private PlayerControl playerControl;
    private GameObject pauseMenu;
    private GameObject optionsMenu;
    public GameObject warningText;
    private bool isOptions = false;
    
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase.Equals(InputActionPhase.Started))
            ShowPauseMenu();
    }

    public void OnResume(InputAction.CallbackContext context)
    {
        if (context.phase.Equals(InputActionPhase.Started))
            HidePauseMenu();
    }
    public void ShowPauseMenu()
    {
        warningText.SetActive(false);
        playerControl.SwitchPlayerMap("UI");
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
        Time.timeScale = 0;
        AudioManager.instance.PauseAllEvents();
    }

    public void HidePauseMenu()
    {
        warningText.SetActive(true);
        optionsMenu.SetActive(false);
        isOptions = false;
        playerControl.SwitchPlayerMap("Player");
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        AudioManager.instance.ResumeAllEvents();
    }

    public static void QuitGame()
    {
        if (PlayerPrefs.HasKey("bossPhase")) 
            PlayerPrefs.DeleteKey("bossPhase");
        Application.Quit();
    }

    public void ShowOptions()
    {
        pauseMenu.SetActive(false);
        isOptions = true;
        optionsMenu.SetActive(true);
    }
    
    public void GoBack()
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