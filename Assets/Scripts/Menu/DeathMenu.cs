using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class DeathMenu : MonoBehaviour
{

    [SerializeField] private string scene = "PatentEnvironment";
    [SerializeField] private Image header;
    [SerializeField] private VideoPlayer video;
    [SerializeField] private Sprite[] bossWin;
    [SerializeField] private Sprite[] playerWin;
    [SerializeField] private Image restartButton;
    [SerializeField] private Image background;
    private GameObject bossHUD;
    private GameObject endingCanvas;

    private static bool bossDied = false;
    void Start()
    {
        bossHUD = GameObject.Find("Canvas");
        bossHUD.SetActive(false);
        if (bossDied)
        {
            ShowEndingCutScene();
            gameObject.SetActive(false);
            header.sprite = playerWin[0];
            restartButton.sprite = playerWin[1];
        }
        else
        {
            header.sprite = bossWin[0];
            restartButton.sprite = bossWin[1];
            background.sprite = bossWin[2];
        }
        
    }

    void Update()
    {
        if (Keyboard.current[Key.Space].wasPressedThisFrame
           )
        {
            EndReached(video);
        }
    }


    void ShowEndingCutScene()
    {
        // endingCanvas.SetActive(false);
        video.loopPointReached += EndReached;
        video.Play();
    }
    
    void EndReached(VideoPlayer vp)
    {
        vp.Stop();
        gameObject.SetActive(true);
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
        if (!PlayerPrefs.HasKey("bossPhase"))
        {
            Debug.Log("BossPhase not found: This should never happen");
            return;
        }
        
        if (bossDied)
        {
            PlayerPrefs.SetInt("bossPhase", 0);
        }
        
        StartCoroutine(AsyncMusicLoad.LoadGameAsync(scene));
    }
    
    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    
    public void QuitGame()
    {
        PauseMenu.QuitGame();
    }
}
