using UnityEngine;
using System;
using System.Runtime.InteropServices;
using FMOD;
using FMOD.Studio;
using FMODUnity;

public class MusicEventHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private EventReference backgroundTrack;
    private EventInstance eventInstance;
    private EVENT_CALLBACK beatCallback;

    private PlayerControl player;

    public static bool beatCheck { get; private set; } = false;
    void Start()
    {
        player = FindObjectOfType<PlayerControl>();
        backgroundTrack = SoundRef.Instance.backgroundTrack;
        eventInstance = AudioManager.instance.CreateEventInstance(backgroundTrack);
        
        // ** This is how to convert the data to pass to callback 
        // GCHandle handle1 = GCHandle.Alloc(this);
        // eventInstance.setUserData((IntPtr) handle1);
        eventInstance.setCallback(OnBeatReached,  EVENT_CALLBACK_TYPE.TIMELINE_BEAT);
        eventInstance.start();        
    }
    
    [AOT.MonoPInvokeCallback(typeof(EVENT_CALLBACK))]
    private static RESULT OnBeatReached(EVENT_CALLBACK_TYPE type, IntPtr instance, IntPtr parameterPtr)
    {
        // ** This is reference code in case we need to pass data to the callback
        // EventInstance callBackInstance = new EventInstance(instance);
        // MusicEventHandler musicEventHandler;
        // RESULT result = callBackInstance.getUserData(out IntPtr musicEventHandlerPtr);
        // if (result != RESULT.OK)
        //     Debug.LogError("Timeline Callback error: " + result);
        // GCHandle test = (GCHandle)musicEventHandlerPtr;
        // MusicEventHandler musicEventHandler = test.Target as MusicEventHandler;

        
        var parameter = (TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(TIMELINE_BEAT_PROPERTIES));
        var beat = parameter.beat;
        switch (beat)
        {
            case 1:
                beatCheck = true;
                break;
            case 3:
                beatCheck = true;
                break;

            default:
                beatCheck = false;
                break;
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
            InputIndicator.Instance.color = Color.green;
        }
        else
        {
            InputIndicator.Instance.color = Color.red;
            player.inputted = false;
        }
    }
    



}