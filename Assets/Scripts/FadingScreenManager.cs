using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class FadingScreenManager : MonoBehaviour
{
    
    public static FadingScreenManager Instance { get; private set; }
    private FadingScreen fade;
    private string[] banks = { "Sfx", "Music", "Ambience" };
    void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one Fading Screen Manager Reference");
        }
        Instance = this;
        fade = FindObjectOfType<FadingScreen>(true);
    }
    public void TransitionToScene(string scene, float speed)
    {
        StartCoroutine(ToScene(scene, speed));
    }
    
    public void AsyncTransitionToScene(float speed, AsyncOperation a)
    {
        StartCoroutine(AsyncToScene(speed, a));
    }
    
    public void CutSceneTransitionToScene(float speed, bool isIntro, 
                                            AsyncOperation a, DeathMenu deathMenu, VideoPlayer vp)
    {
        StartCoroutine(CutSceneToScene(speed, isIntro, a, deathMenu, vp));
    }
    
    public void DeathMenuTransitionToScene(float speed = 0.5F)
    {
        var playerInput = FindObjectOfType<PlayerInput>();
        playerInput.currentActionMap = playerInput.actions.FindActionMap("UI");
        StopAllCoroutines();
        StartCoroutine(ToDeathScene(speed));
    }
    
    IEnumerator CutSceneToScene(float speed, bool isIntro, 
                                AsyncOperation a, DeathMenu deathMenu, VideoPlayer vp)
    {
        yield return StartCoroutine(fade.FadeToBlack(speed));
        if (isIntro)
        {
            a.allowSceneActivation = true;
        }
        else
        {
            deathMenu.DisableVideoCamAndSkip();
            vp.Stop();

            deathMenu.gameObject.SetActive(true);
            yield return StartCoroutine(fade.FadeFromBlack(speed));
            fade.gameObject.SetActive(false);
        }
    }

    IEnumerator ToScene(string scene, float speed)
    {
        yield return StartCoroutine(fade.FadeToBlack( speed));
        SceneManager.LoadScene(scene);
    }
    
    IEnumerator AsyncToScene(float speed, AsyncOperation a)
    {
        yield return StartCoroutine(fade.FadeToBlack( speed));
        // Iterate all the Studio Banks and start them loading in the background
        // including the audio sample data
        foreach(var bank in banks)
        {
            FMODUnity.RuntimeManager.LoadBank(bank, true);
        }

        // Keep yielding the co-routine until all the bank loading is done
        // (for platforms with asynchronous bank loading)
        while (!FMODUnity.RuntimeManager.HaveAllBanksLoaded)
        {
            yield return null;
        }

        // Keep yielding the co-routine until all the sample data loading is done
        while (FMODUnity.RuntimeManager.AnySampleDataLoading())
        {
            yield return null;
        }

        // Allow the scene to be activated. This means that any OnActivated() or Start()
        // methods will be guaranteed that all FMOD Studio loading will be completed and
        // there will be no delay in starting events
        a.allowSceneActivation = true;

        // Keep yielding the co-routine until scene loading and activation is done.
        while (!a.isDone)
        {
            yield return null;
        }
    }
    
    IEnumerator ToDeathScene(float speed)
    {
        yield return StartCoroutine(fade.FadeToBlack( speed));
        AudioManager.instance.PauseAllEvents();
        Time.timeScale = 0;
        SceneManager.LoadScene("EndMenu", LoadSceneMode.Additive);
    }
    
    public void UpdateFadeRef(FadingScreen newFade)
    {
        fade = newFade;
    }
    
    
}
