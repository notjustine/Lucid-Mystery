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
    private EventReference sleepingTrack;
    private EventReference backgroundTrack;
    private EventInstance eventInstance;
    private EVENT_CALLBACK beatCallback;

    private PlayerControl player;

    public static bool beatCheck { get; set; } = false;

    void Start()
    {
        player = FindObjectOfType<PlayerControl>();
        // backgroundTrack = SoundRef.Instance.backgroundTrack;
        // eventInstance = AudioManager.instance.CreateEventInstance(backgroundTrack);
        // ** This is how to convert the data to pass to callback 
        // GCHandle handle1 = GCHandle.Alloc(this);
        // eventInstance.setUserData((IntPtr) handle1);
        // eventInstance.setCallback(OnMarkerReached, EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
        // eventInstance.start();
        SetMainMusicPhaseParameter(PlayerPrefs.GetInt("bossPhase", 0));
    }

    [AOT.MonoPInvokeCallback(typeof(EVENT_CALLBACK))]
    private static RESULT OnMarkerReached(EVENT_CALLBACK_TYPE type, IntPtr instance, IntPtr parameterPtr)
    {
        // ** This is reference code in case we need to pass data to the callback
        // EventInstance callBackInstance = new EventInstance(instance);
        // MusicEventHandler musicEventHandler;
        // RESULT result = callBackInstance.getUserData(out IntPtr musicEventHandlerPtr);
        // if (result != RESULT.OK)
        //     Debug.LogError("Timeline Callback error: " + result);
        // GCHandle test = (GCHandle)musicEventHandlerPtr;
        // MusicEventHandler musicEventHandler = test.Target as MusicEventHandler;
        var parameter =
            (TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(TIMELINE_MARKER_PROPERTIES));
        if (parameter.name == "allowinput")
        {
            beatCheck = true;
            InputIndicator.Instance.active = true;
        }
        else if (parameter.name == "stopinput")
        {
            beatCheck = false;
            InputIndicator.Instance.active =false;
        }

        return RESULT.OK;
    }
    
    public void SetMainMusicPhaseParameter(int phase)
    {
        eventInstance.setParameterByName("current_phase", phase);
    }

    private void OnDestroy()
    {
        eventInstance.release();
    }

    // Update is called once per frame
    void Update()
    {
        if (!beatCheck) 
            player.inputted = false;
    }
}