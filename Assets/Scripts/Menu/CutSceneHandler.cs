using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;
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
        if (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame)
            EndReached(videoPlayer);
        if (Input.GetKeyDown(KeyCode.Space))
            EndReached(videoPlayer);
    }

    public void Play()
    {
        videoPlayer.Play();
        // GetComponent<AudioSource>().Play();
        GetComponent<StudioEventEmitter>().Play();
    }   

    private void EndReached(VideoPlayer vp)
    {
        FadingScreenManager.Instance.CutSceneTransitionToScene(1f, isIntro, a, deathMenu, vp);
    }
    
}
