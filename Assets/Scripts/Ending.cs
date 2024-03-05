using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class Ending : MonoBehaviour
{
    
    [SerializeField] private string scene = "AlphaClone";
    [SerializeField] private TextMeshProUGUI TitleText;
    [SerializeField] private TextMeshProUGUI TryAgainText;
    [SerializeField] private VideoPlayer video;

    private static bool bossDied = false;
    void Start()
    {
        video = Camera.main.GetComponent<VideoPlayer>();
        
        if (bossDied)
        {
            // TitleText.text = "You Win!";
            // TryAgainText.text = "Play Again?";
            ShowEndingCutScene();
        }
        else
        {
            TitleText.text = "You Died";
            TryAgainText.text = "Try Again?";
        }
    }

    void ShowEndingCutScene()
    {
        
        TitleText.gameObject.SetActive(false);
        TryAgainText.gameObject.SetActive(false);
        
        ;
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
