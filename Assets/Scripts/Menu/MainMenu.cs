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


    public void StartGame()
    {
        AudioManager.instance.PlayOneShot(SoundRef.Instance.menuSelect, new Vector3());
        StartCoroutine(AsyncMusicLoad.LoadGameAsync(sceneName));
        // SceneManager.LoadScene(sceneName);
    }

    public void ShowOptions()
    {
        AudioManager.instance.PlayOneShot(SoundRef.Instance.menuSelect, new Vector3());
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
    
    public void GoBack()
    {
        AudioManager.instance.PlayOneShot(SoundRef.Instance.menuSelect, new Vector3());
        optionsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
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
        PlayerPrefs.SetInt("bossPhase", 0);
    }
}
