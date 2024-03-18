using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class FadingScreenManager : MonoBehaviour
{
    
    public static FadingScreenManager Instance { get; private set; }
    private FadingScreen fade;
    void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one Sound Reference");
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
            deathMenu.gameObject.SetActive(true);
            vp.Stop();
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
        a.allowSceneActivation = true;
    }
    
    IEnumerator ToDeathScene(float speed)
    {
        yield return StartCoroutine(fade.FadeToBlack( speed));
        AudioManager.instance.PauseAllEvents();
        FindObjectOfType<PlayerControl>().gameObject.SetActive(false);
        SceneManager.LoadScene("EndMenu", LoadSceneMode.Additive);
    }
}
