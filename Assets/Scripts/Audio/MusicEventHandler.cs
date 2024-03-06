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
    private EventReference backgroundTrack2;
    private EventInstance eventInstance;
    private EVENT_CALLBACK beatCallback;

    private PlayerControl player;

    public static bool beatCheck { get; private set; } = false;

    void Start()
    {
        player = FindObjectOfType<PlayerControl>();
        backgroundTrack = SoundRef.Instance.backgroundTrack;
        backgroundTrack2 = SoundRef.Instance.backgroundTrack2;
        sleepingTrack = SoundRef.Instance.sleepingTrack;
        StartPhaseOneMusic();
        // switch (PlayerPrefs.GetInt("bossPhase", -1))
        // {
        //     case 0:
        //         StartSleepingMusic();
        //         break;
        //     case 1:
        //         StartPhaseOneMusic();
        //         break;
        //     case 2:
        //         StartPhaseTwoMusic();
        //         break;
        //     default:
        //         break;
        // }

    }
    
    void StartSleepingMusic()
    {
        eventInstance = AudioManager.instance.CreateEventInstance(sleepingTrack);
        eventInstance.setCallback(OnBeatReached, EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
        eventInstance.start();
    }

    public void StartPhaseOneMusic()
    {
        if (eventInstance.isValid())
        {
            AudioManager.instance.StopEvent(eventInstance.GetHashCode());
        }
        eventInstance = AudioManager.instance.CreateEventInstance(backgroundTrack);
        // ** This is how to convert the data to pass to callback 
        // GCHandle handle1 = GCHandle.Alloc(this);
        // eventInstance.setUserData((IntPtr) handle1);
        eventInstance.setCallback(OnBeatReached, EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
        eventInstance.start();
    }
    
    public void StartPhaseTwoMusic()
    {
        // if (eventInstance.isValid())
        // {
        //     AudioManager.instance.StopEvent(eventInstance.GetHashCode());
        // }
        // eventInstance = AudioManager.instance.CreateEventInstance(backgroundTrack2);
        // eventInstance.setCallback(OnBeatReached, EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
        // eventInstance.start();
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