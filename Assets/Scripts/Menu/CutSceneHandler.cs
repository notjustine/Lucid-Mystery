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
    private FadingScreen fade;
    private AsyncOperation a;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = videoPlayer.GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += EndReached;
        if (isIntro)
        {
            a = SceneManager.LoadSceneAsync("Tutorial");
            fade = FindObjectOfType<FadingScreen>();
            StartCoroutine(StartVideo());
            a.allowSceneActivation = false;
            
        }
        else
        {
            deathMenu = FindObjectOfType<DeathMenu>(true);
        }
        
    }
    
    IEnumerator StartVideo()
    {
        yield return StartCoroutine(fade.FadeFromBlack(1f));
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
        StartCoroutine(TransitionToScene(vp));
    }
    
    IEnumerator TransitionToScene(VideoPlayer vp)
    {
        yield return StartCoroutine(fade.FadeToBlack("", 1f));
        if (isIntro)
        {
            // StartCoroutine(fade.FadeToBlack("s",1f));
            a.allowSceneActivation = true;
        }
        else
        {
            deathMenu.gameObject.SetActive(true);
            vp.Stop();
        }
    }
    
}
