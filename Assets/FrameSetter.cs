using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameSetter : MonoBehaviour
{
    [SerializeField] private int[] frameRate = {30, 60, 120, 144, -1};
    [SerializeField] private TMPro.TMP_Dropdown dropdown;    
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("fps"))
        {
            Application.targetFrameRate = PlayerPrefs.GetInt("fps");
            dropdown.value = Array.IndexOf(frameRate, PlayerPrefs.GetInt("fps"));
        }
        else
        {
            Application.targetFrameRate = 60;
        }
    }
    
    public void SetFrameRate(Int32 index)
    {
        Debug.Log(index);
        Application.targetFrameRate = frameRate[index];
        PlayerPrefs.SetInt("fps", frameRate[index]);
    }
}