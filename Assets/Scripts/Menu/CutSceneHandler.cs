using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CutSceneHandler : MonoBehaviour
{

    private VideoPlayer videoPlayer;

    private AsyncOperation a;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        // videoPlayer.loopPointReached += EndReached;
        a = SceneManager.LoadSceneAsync("PatentEnvironment");
        a.allowSceneActivation = false;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            a.allowSceneActivation = true;
            // EndReached(videoPlayer);
        }
    }
    
    // I think we don't need this anymore
    void EndReached(VideoPlayer vp)
    {
        vp.Stop();
        // a.allowSceneActivation = true;
        SceneManager.LoadScene("PatentEnvironment");
    }
}
