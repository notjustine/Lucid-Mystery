using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private PlayerControl playerControl;
    private GameObject pauseMenu;
    private GameObject optionsMenu;
    public GameObject warningText;
    private bool isOptions = false;
    private bool warningTextActive = false;
    private FadingScreen fade;


    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase.Equals(InputActionPhase.Started))
        {
            AudioManager.instance.PlayOneShot(SoundRef.Instance.menuSelect, new Vector3());
            ShowPauseMenu();
        }
    }

    public void OnResume(InputAction.CallbackContext context)
    {
        if (context.phase.Equals(InputActionPhase.Started))
        {
            AudioManager.instance.PlayOneShot(SoundRef.Instance.menuSelect, new Vector3());
            HidePauseMenu();
        }
    }
    public void ShowPauseMenu()
    {
        fade.gameObject.SetActive(false);
        warningTextActive = warningText.activeSelf;
        warningText.SetActive(false);
        playerControl.SwitchPlayerMap("UI");
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
        Time.timeScale = 0;
        AudioManager.instance.PauseAllEvents();
    }

    public void RestartPhase()
    {
        SceneManager.LoadScene("ZyngaMain");
        HidePauseMenu();
    }

    public void HidePauseMenu()
    {
        if (warningTextActive)
            warningText.SetActive(true);
        optionsMenu.SetActive(false);
        isOptions = false;
        playerControl.SwitchPlayerMap("Player");
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        AudioManager.instance.ResumeAllEvents();
        fade.gameObject.SetActive(true);
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
            AudioManager.instance.PlayOneShotAttached(SoundRef.Instance.menuSelect, Camera.main.gameObject);
            isOptions = false;
            optionsMenu.SetActive(false);
            pauseMenu.SetActive(true);
        }
        else
        {
            HidePauseMenu();
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    void Start()
    {
        playerControl = FindObjectOfType<PlayerControl>();
        pauseMenu = gameObject.transform.GetChild(0).gameObject;
        optionsMenu = gameObject.transform.GetChild(1).gameObject;
        fade = FindObjectOfType<FadingScreen>();
    }
}