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

    private const float SkipWaitBuffer = 2f;
    private float time = 0f;
    private DeathMenu deathMenu;
    private FadingScreen fade;
    private AsyncOperation a;

    private StudioEventEmitter cutsceneMusic;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = videoPlayer.GetComponent<VideoPlayer>();
        videoPlayer.Prepare();
        videoPlayer.loopPointReached += EndReached;
        cutsceneMusic = GetComponent<StudioEventEmitter>();
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
        yield return new WaitUntil(() => videoPlayer.isPrepared);
        yield return StartCoroutine(fade.FadeFromBlack(1f));
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time < SkipWaitBuffer)
        {
            
            return;
        }
        
        if (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame)
            EndReached(videoPlayer);
        if (Input.GetKeyDown(KeyCode.Space))
            EndReached(videoPlayer);
    }

    public void Play()
    {
        videoPlayer.Play();
        cutsceneMusic.Play();
    }   

    private void EndReached(VideoPlayer vp)
    {
        if (cutsceneMusic)
            cutsceneMusic.Stop();
        FadingScreenManager.Instance.CutSceneTransitionToScene(1f, isIntro, a, deathMenu, vp);
    }
    
}
