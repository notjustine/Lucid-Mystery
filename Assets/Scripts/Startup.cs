using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Startup : MonoBehaviour
{
    // [SerializeField] private bool tutorialActive = true;
    // Start is called before the first frame update
    void Start()
    {
        if (TutorialManager.tutorialActive)
        {
            SceneManager.LoadScene("Tutorial", LoadSceneMode.Additive);
            
        }
        if (PlayerPrefs.HasKey("fps"))
        {
            Application.targetFrameRate = PlayerPrefs.GetInt("fps");
        }
        else
        {
            Application.targetFrameRate = 60;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
