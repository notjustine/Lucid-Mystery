using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SoundRef : MonoBehaviour
{
    public static SoundRef Instance;
    public EventReference attackSound;
    public EventReference backgroundTrack;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one Sound Reference");
        }

        Instance = this;
    }
}


