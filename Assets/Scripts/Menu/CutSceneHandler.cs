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
        videoPlayer = videoPlayer.GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += EndReached;
        if (isIntro)
        {
            a = SceneManager.LoadSceneAsync("Tutorial");
            a.allowSceneActivation = false;
        }
        else
        {
            deathMenu = FindObjectOfType<DeathMenu>(true);
        }
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            EndReached(videoPlayer);
    }

    public void Play()
    {
        videoPlayer.Play();
    }

    private void EndReached(VideoPlayer vp)
    {
        if (isIntro)
        {
            a.allowSceneActivation = true;
        }
        else
        {
            deathMenu.gameObject.SetActive(true);
            vp.Stop();
        }
    }
    
}
