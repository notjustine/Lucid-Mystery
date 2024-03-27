using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject creditsPanel;
    private DifficultyManager difficulty;
    private FadingScreen fadingScreen;

    public void StartGame()
    {
        AudioManager.instance.PlayOneShot(SoundRef.Instance.menuSelect, new Vector3());
        mainMenuPanel.SetActive(false);
        difficulty.gameObject.SetActive(true);
    }

    public void ShowOptions()
    {
        AudioManager.instance.PlayOneShot(SoundRef.Instance.menuSelect, new Vector3());
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void ShowCredits()
    {
        AudioManager.instance.PlayOneShot(SoundRef.Instance.menuSelect, new Vector3());
        mainMenuPanel.SetActive(false);
        creditsPanel.gameObject.SetActive(true);
    }

    public void GoBack()
    {
        AudioManager.instance.PlayOneShot(SoundRef.Instance.menuSelect, new Vector3());
        if (optionsPanel.activeSelf)
        {
            optionsPanel.SetActive(false);
        }
        else
        {
            creditsPanel.SetActive(false);
        }
        mainMenuPanel.SetActive(true);

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void Update()
    {
        if (difficulty.gameObject.activeSelf && DifficultyManager.Instance.hasChanged)
        {
            fadingScreen.gameObject.SetActive(true);
            FadingScreenManager.Instance.TransitionToScene(sceneName, 1f);
            DifficultyManager.Instance.hasChanged = false;
        }
    }


    private void Start()
    {
        if (PlayerPrefs.HasKey("fps"))
        {
            Application.targetFrameRate = PlayerPrefs.GetInt("fps");
        }
        else
        {
            Application.targetFrameRate = 60;
        }
        difficulty = FindObjectOfType<DifficultyManager>(true);
        fadingScreen = FindObjectOfType<FadingScreen>(true);
        Time.timeScale = 1;
    }
}
