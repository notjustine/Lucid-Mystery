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

    private const float SkipWaitBuffer = 3f;
    private DeathMenu deathMenu;
    private FadingScreen fade;
    private AsyncOperation a;
    private GameObject skipPanel;
    private StudioEventEmitter cutsceneMusic;
    // Start is called before the first frame update

    void Awake()
    {
        videoPlayer = videoPlayer.GetComponent<VideoPlayer>();
        cutsceneMusic = GetComponent<StudioEventEmitter>();
        videoPlayer.Prepare();
    }
    void Start()
    {
        videoPlayer.loopPointReached += EndReached;
        fade = FindObjectOfType<FadingScreen>();
        FadingScreenManager.Instance.UpdateFadeRef(fade);
        if (isIntro)
        {
            videoPlayer.started += source => cutsceneMusic.Play(); 
            a = SceneManager.LoadSceneAsync("Tutorial");
            StartCoroutine(StartVideo());
            a.allowSceneActivation = false;
            skipPanel = GameObject.Find("Skip");
            skipPanel.SetActive(false);
            
        }
        else
        {
            deathMenu = FindObjectOfType<DeathMenu>(true);
        }

        StartCoroutine(BufferTime());
    }
    
    IEnumerator StartVideo()
    {
        yield return new WaitUntil(() => videoPlayer.isPrepared);
        yield return StartCoroutine(fade.FadeFromBlack(1f));
    }

    void Update()
    {
        if (!skipPanel.activeSelf)
            return;
        if (Gamepad.current != null && Gamepad.current.buttonNorth.wasPressedThisFrame)
            EndReached(videoPlayer);
        if (Input.GetKeyDown(KeyCode.Space))
            EndReached(videoPlayer);
    }

    public void Play(GameObject skip)
    {
        skipPanel = skip;
        videoPlayer.Play();
        cutsceneMusic.Play();
    }

    IEnumerator BufferTime()
    {
        yield return new WaitForSeconds(SkipWaitBuffer);
        skipPanel.SetActive(true);
    }

    private void EndReached(VideoPlayer vp)
    {
        if (cutsceneMusic)
            cutsceneMusic.Stop();
        FadingScreenManager.Instance.CutSceneTransitionToScene(1.5f, isIntro, a, deathMenu, vp);
    }
    
}
