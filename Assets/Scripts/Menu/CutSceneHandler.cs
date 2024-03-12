using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CutSceneHandler : MonoBehaviour
{

    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private bool isIntro = true;
    private DeathMenu deathMenu;
    private AsyncOperation a;
    // Start is called before the first frame update
    void Start()
    {

        if (isIntro)
        {
            a = SceneManager.LoadSceneAsync("PatentEnvironment");
            a.allowSceneActivation = false;
        }
        else
        {
            deathMenu = FindObjectOfType<DeathMenu>(true);
        }
        
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && isIntro)
        {
            a.allowSceneActivation = true;
        } else if (Input.GetKeyDown(KeyCode.Space) && !isIntro)
        {
            videoPlayer.Stop();
            deathMenu.gameObject.SetActive(true);
        }
    }

    public void Play()
    {
        videoPlayer.Play();
    }
    
}
