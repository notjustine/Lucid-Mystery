using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class DeathMenu : MonoBehaviour
{

    [SerializeField] private string scene = "ZyngaMain";
    [SerializeField] private Image header;
    [SerializeField] private Sprite[] bossWin;
    [SerializeField] private Sprite[] playerWin;
    [SerializeField] private Image restartButton;
    [SerializeField] private Image background;
    private CutSceneHandler cutSceneHandler;
    private GameObject bossHUD;
    private Transform endCamera;
    private Transform mainCamera;
    private StudioEventEmitter deathMusic;

    private static bool bossDied = false;
    void Start()
    {
        Time.timeScale = 0;
        AudioManager.instance.PauseAllEvents();
        cutSceneHandler = FindObjectOfType<CutSceneHandler>();
        bossHUD = GameObject.Find("Canvas");
        mainCamera = Camera.main.transform;
        endCamera = GameObject.Find("Camera").transform;
        endCamera.position = mainCamera.position;
        bossHUD.SetActive(false);
        if (bossDied)
        {
            cutSceneHandler.Play();
            gameObject.SetActive(false);
            header.sprite = playerWin[0];
            restartButton.sprite = playerWin[1];
        }
        else
        {
            header.sprite = bossWin[0];
            restartButton.sprite = bossWin[1];
            background.sprite = bossWin[2];
            deathMusic = GetComponent<StudioEventEmitter>();
            deathMusic.Play();
        }

    }
    public static void BossLoss()
    {
        bossDied = true;

    }

    public static void PlayerLoss()
    {
        bossDied = false;
    }
    public void TryAgain()
    {
        if (bossDied)
        {
            DifficultyManager.phase = 0;
        }

        SceneManager.LoadScene(scene);
    }


    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        PauseMenu.QuitGame();
    }

    private void OnDestroy()
    {
        Time.timeScale = 1;
        if (deathMusic)
            deathMusic.Stop();
        DifficultyManager.phase = 0;
    }
}
