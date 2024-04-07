using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadHPUI : MonoBehaviour
{

    void Awake()
    {
        SceneManager.LoadScene("HP HUD", LoadSceneMode.Additive);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
