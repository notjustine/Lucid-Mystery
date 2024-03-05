using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Ending : MonoBehaviour
{
    
    [SerializeField] private string scene = "AlphaClone";
    [SerializeField] private Image header;
    [SerializeField] private VideoPlayer video;
    [SerializeField] private Sprite[] bossWin;
    [SerializeField] private Sprite[] playerWin;
    [SerializeField] private Image restartButton;
    [SerializeField] private Image background;
    

    private static bool bossDied = false;
    void Start()
    {
        video = Camera.main.GetComponent<VideoPlayer>();
        
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            video.Stop();
            gameObject.SetActive(true);
        }
    }

    void ShowEndingCutScene()
    {
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
    
    public void QuitGame()
    {
        PauseMenu.QuitGame();
    }
}
