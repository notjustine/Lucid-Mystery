using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class AmbienceHandler : MonoBehaviour
{
    StudioEventEmitter[] ambienceEmitters;
    // Start is called before the first frame update
    void Start()
    {
        ambienceEmitters = GetComponentsInChildren<StudioEventEmitter>();
        PlayAmbience();
    }
    
    public void PlayAmbience()
    {
        foreach (var emitter in ambienceEmitters)
        {
            emitter.Play();
        }
    }
    
    public void PauseAmbience()
    {
        foreach (var emitter in ambienceEmitters)
        {
            emitter.EventInstance.setPaused(true);
        }
    }
    
    public void UnPauseAmbience()
    {
        foreach (var emitter in ambienceEmitters)
        {
            emitter.EventInstance.setPaused(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
