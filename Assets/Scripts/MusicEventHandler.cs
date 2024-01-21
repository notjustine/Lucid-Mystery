using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class MusicEventHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public EventReference fmodEvent;

    private EventInstance eventInstance;
    
    private bool clicked = false;
    // private AudioManager audioManager;
    void Start()
    {
        // audioManager = AudioManager.instance;
        eventInstance = RuntimeManager.CreateInstance(fmodEvent);
        // eventInstance = audioManager.CreateEventInstance(fmodEvent);
        eventInstance.setCallback(OnMarkerReached,  FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT);
        eventInstance.start();        
    }

    // private RESULT OnMarkerReached(EVENT_CALLBACK_TYPE type, IntPtr _event, IntPtr parameters)
    // {
    //     throw new NotImplementedException();
    // }

    // static FMOD.RESULT OnMarkerReached(FMOD.Studio.EVENT_CALLBACK_TYPE type, FMOD.Studio.EventInstance instance, IntPtr parameterPtr)
    private FMOD.RESULT OnMarkerReached(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instance, IntPtr parameterPtr)
    {
        EventInstance callBackInstance = new EventInstance(instance);
        
        var parameter = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
        float beat = parameter.beat;
        // callBackInstance.getParameterByName("beat", out beat);
        Debug.Log("Beat " + beat);
        if (beat % 3 == 0)
        {
            CheckInputBasedOnMarker();
        }
        return FMOD.RESULT.OK;
    }
    

    
    private void CheckInputBasedOnMarker()
    {
        if (clicked)
        {
            Debug.Log("Clicked During Beat");
        }
    }

    private void OnDestroy()
    {
        eventInstance.release();
    }


    // Update is called once per frame
    void Update()
    {
        clicked = Input.GetButton("Fire1");
    }
    
}
