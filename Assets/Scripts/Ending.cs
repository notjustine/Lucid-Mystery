using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ending : MonoBehaviour
{
    
    [SerializeField] private string scene = "AlphaClone";
    [SerializeField] private TextMeshProUGUI TitleText;
    [SerializeField] private TextMeshProUGUI TryAgainText;

    private static bool bossDied = false;
    void Start()
    {
        if (bossDied)
        {
            TitleText.text = "You Win!";
            TryAgainText.text = "Play Again?";
        }
        else
        {
            TitleText.text = "You Died";
            TryAgainText.text = "Try Again?";
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
