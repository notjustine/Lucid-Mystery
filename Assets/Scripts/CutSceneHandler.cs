using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CutSceneHandler : MonoBehaviour
{

    private VideoPlayer videoPlayer;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += EndReached;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndReached(videoPlayer);
        }
    }

    void EndReached(VideoPlayer vp)
    {
        vp.Stop();
        SceneManager.LoadScene("AlphaClone");
    }
}
