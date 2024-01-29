using UnityEngine;
using System;
using System.Runtime.InteropServices;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class MusicEventHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public EventReference fmodEvent;
    private EventInstance eventInstance;
    private EVENT_CALLBACK beatCallback;

    private PlayerControl player;

    private static bool beatCheck = false;
    void Start()
    {
        player = FindObjectOfType<PlayerControl>();
        eventInstance = AudioManager.instance.CreateEventInstance(fmodEvent);
        // GCHandle handle1 = GCHandle.Alloc(this);
        // eventInstance.setUserData((IntPtr) handle1);
        eventInstance.setCallback(OnBeatReached,  EVENT_CALLBACK_TYPE.TIMELINE_BEAT);
        eventInstance.start();        
    }
    
    [AOT.MonoPInvokeCallback(typeof(EVENT_CALLBACK))]
    private static RESULT OnBeatReached(EVENT_CALLBACK_TYPE type, IntPtr instance, IntPtr parameterPtr)
    {
        EventInstance callBackInstance = new EventInstance(instance);
        // MusicEventHandler musicEventHandler;
        // RESULT result = callBackInstance.getUserData(out IntPtr musicEventHandlerPtr);
        // if (result != RESULT.OK)
        //     Debug.LogError("Timeline Callback error: " + result);
        // GCHandle test = (GCHandle)musicEventHandlerPtr;
        // MusicEventHandler musicEventHandler = test.Target as MusicEventHandler;

        
        var parameter = (TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(TIMELINE_BEAT_PROPERTIES));
        var beat = parameter.beat;
        if (beat % 3 == 0)
        {
            beatCheck = true;
            // Debug.Log("INPUT CHECK");
        }
        else
        {
            beatCheck = false;
        }
        return RESULT.OK;
    }
    
    private void OnDestroy()
    {
        eventInstance.release();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (beatCheck)
        {
            // PlayerMovement.Instance.color = Color.green;
            // PlayerMovement.Instance.UpdateInputHelper();
            // player;
            InputIndicator.Instance.color = Color.green;
            player.AllowMove();
        }
        else
        {
            InputIndicator.Instance.color = Color.red;
            // PlayerMovement.Instance.color = Color.red;
            
        }
    }
    



}