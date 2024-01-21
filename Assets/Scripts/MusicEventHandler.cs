using UnityEngine;
using System;
using System.Runtime.InteropServices;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Debug = UnityEngine.Debug;

public class MusicEventHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public EventReference fmodEvent;
    
    private EventInstance eventInstance;
    private EVENT_CALLBACK beatCallback;
    
    // private AudioManager audioManager;
    void Start()
    {
        eventInstance = RuntimeManager.CreateInstance(fmodEvent);
        // eventInstance = audioManager.CreateEventInstance(fmodEvent);
        eventInstance.setCallback(OnMarkerReached,  EVENT_CALLBACK_TYPE.TIMELINE_BEAT | EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
        eventInstance.start();        
    }
    
    [AOT.MonoPInvokeCallback(typeof(EVENT_CALLBACK))]
    private static RESULT OnMarkerReached(EVENT_CALLBACK_TYPE type, IntPtr instance, IntPtr parameterPtr)
    {
        EventInstance callBackInstance = new EventInstance(instance);
        RESULT result = callBackInstance.getUserData(out IntPtr timelineInfoPtr);
        if (result != RESULT.OK)
            Debug.LogError("Timeline Callback error: " + result);

        
        var parameter = (TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(TIMELINE_BEAT_PROPERTIES));
        var beat = parameter.beat;
        Debug.Log("Beat " + beat);
        if (beat % 3 == 0)
        {
            Debug.Log("INPUT CHECK");
            CheckInputBasedOnMarker();
        }
        
        return RESULT.OK;
    }
    
    
    // This works but I think there is a better way to do this
    private static void CheckInputBasedOnMarker()
    {
        bool test = Input.GetButton("Fire1");
        Debug.Log(test);
    }
    private void OnDestroy()
    {
        eventInstance.release();
    }
    
    
    
    // Update is called once per frame
    void Update()
    {
        // clicked = Input.GetButton("Fire1");
        // if (clicked)
        //     Debug.Log(clicked);
    }
    



}