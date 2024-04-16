using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject creditsPanel;
    private DifficultyManager difficulty;
    private FadingScreen fadingScreen;
    private AsyncOperation sceneLoading;
    private EventSystem eventSystem;
    private GameObject lastSelected;
    private GameObject backButton;

    public void StartGame()
    {
        backButton.SetActive(true);
        lastSelected = eventSystem.currentSelectedGameObject;
        AudioManager.instance.PlayOneShot(SoundRef.Instance.menuSelect, new Vector3());
        mainMenuPanel.SetActive(false);
        difficulty.gameObject.SetActive(true);
        eventSystem.SetSelectedGameObject(GameObject.Find("Easy"));
    }

    public void ShowOptions()
    {
        backButton.SetActive(true);
        lastSelected = eventSystem.currentSelectedGameObject;
        AudioManager.instance.PlayOneShot(SoundRef.Instance.menuSelect, new Vector3());
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
        eventSystem.SetSelectedGameObject(GameObject.Find("Slider"));
    }

    public void ShowCredits()
    {
        backButton.SetActive(true);
        lastSelected = eventSystem.currentSelectedGameObject;
        AudioManager.instance.PlayOneShot(SoundRef.Instance.menuSelect, new Vector3());
        mainMenuPanel.SetActive(false);
        creditsPanel.gameObject.SetActive(true);
        eventSystem.SetSelectedGameObject(backButton);
    } 

    public void GoBack()
    {
        if (mainMenuPanel.activeSelf) 
            return;
        
        AudioManager.instance.PlayOneShot(SoundRef.Instance.menuSelect, new Vector3());
        
        if (optionsPanel.activeSelf)
        {
            optionsPanel.SetActive(false);
        }
        else if (difficulty.gameObject.activeSelf)
        {
            difficulty.gameObject.SetActive(false);
        }
        else
        {
            creditsPanel.SetActive(false);
        }
        mainMenuPanel.SetActive(true);
        backButton.SetActive(false);
        eventSystem.SetSelectedGameObject(lastSelected);
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
            FadingScreenManager.Instance.AsyncTransitionToScene(1f, sceneLoading);
            DifficultyManager.Instance.hasChanged = false;
        }
    }


    private void Start()
    {
        Time.timeScale = 1;
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
        eventSystem = GameObject.Find("Canvas").GetComponent<EventSystem>();
        sceneLoading = SceneManager.LoadSceneAsync(sceneName);
        sceneLoading.allowSceneActivation = false;
        eventSystem.SetSelectedGameObject(GameObject.Find("Continue Button"));
        backButton = GameObject.Find("Back Button");
        backButton.SetActive(false);
    }
}
