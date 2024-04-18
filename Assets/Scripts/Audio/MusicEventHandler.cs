using UnityEngine;
using System;
using System.Runtime.InteropServices;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class MusicEventHandler : MonoBehaviour
{
    
    private PlayerControl player;

    public static bool beatCheck { get; set; } = false;
    
    private static double beatInterval = 0f; // This is the time between each beat;

    private static int markerTime;
    
    int lastBeat = 0;
    
    private const float inputDelay = 175f;
    private const float startDelay = 0f;

    private PLAYBACK_STATE musicPlayState;

    [StructLayout(LayoutKind.Sequential)]
    public class TimelineInfo
    {
        public int currentBeat = 0;
        public int beatPosition = 0;
        public float currentTempo = 0;
        public float lastTempo = 0;
        public int currentPosition = 0;
        public float time = 0;
    }

    public TimelineInfo timelineInfo = null;

    private GCHandle timelineHandle;

    private EVENT_CALLBACK beatCallback;
    private EventDescription descriptionCallback;

    private EventInstance eventInstance;
    private bool justChanged = true;

    private void Start()
    {
        player = FindObjectOfType<PlayerControl>();
        EventDescription des;
        eventInstance.getDescription(out des);
        des.loadSampleData();
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Tutorial"))
            eventInstance = AudioManager.instance.CreateEventInstance(SoundRef.Instance.tutorialTrack);
        else
            eventInstance = AudioManager.instance.CreateEventInstance(SoundRef.Instance.backgroundTrack);
        StartMusic();
    }

    private void AssignMusicCallbacks()
    {
        timelineInfo = new TimelineInfo();
        beatCallback = BeatEventCallback;

        timelineHandle = GCHandle.Alloc(timelineInfo, GCHandleType.Pinned);
        eventInstance.setUserData(GCHandle.ToIntPtr(timelineHandle));
        eventInstance.setCallback(beatCallback, EVENT_CALLBACK_TYPE.TIMELINE_BEAT);
    }

    private void StartMusic()
    {
        eventInstance.start();
        AssignMusicCallbacks();
        switch (DifficultyManager.phase)
        {
            case 0:
                SetMainMusicPhaseParameter(0);
                break;
            case 1:
                SetMainMusicPhaseParameter(1);
                break;
            case 2:
                SetMainMusicPhaseParameter(2);
                break;
            default:
                SetMainMusicPhaseParameter(0);
                break;
        }
    }
    

    private void Update()
    {
        
        eventInstance.getPlaybackState(out musicPlayState);

        if (musicPlayState != PLAYBACK_STATE.PLAYING)
            return;

        eventInstance.getTimelinePosition(out timelineInfo.currentPosition);
        
        CheckTempoMarkers();

        if (beatInterval == 0f)
            return;
        
        if (timelineInfo.currentBeat == 1 | timelineInfo.currentBeat == 3)
        {  
            if (timelineInfo.beatPosition + inputDelay <= timelineInfo.currentPosition)
            {
                beatCheck = false;
                
                if (!justChanged)
                {
                    justChanged = true;
                    player.inputted = false;
                }
                
            }
            else
            {
                beatCheck = true;
                if (justChanged)
                {
                    justChanged = false;
                    player.inputted = false;
                }
            }

        } else if (timelineInfo.currentBeat == 2 | timelineInfo.currentBeat == 4)
        {
            if (timelineInfo.beatPosition + inputDelay + startDelay <= timelineInfo.currentPosition)
            {
                beatCheck = true;
                if (justChanged)
                {
                    justChanged = false;
                    player.inputted = false;
                }
            }
            else
            {
                beatCheck = false;
                if (!justChanged)
                {
                    justChanged = true;
                    player.inputted = false;
                }
            }
            
        }

    }

    private void FixedUpdate()
    {
        Update();
    }

    public void SetMainMusicPhaseParameter(int phase)
    {
        eventInstance.setParameterByName("current_phase", phase);
    }

    
    private void CheckTempoMarkers()
    {
        if (timelineInfo.currentTempo != timelineInfo.lastTempo)
            SetTrackTempo();
    }

    private void SetTrackTempo()
    {
        eventInstance.getTimelinePosition(out int currentTimelinePos);
        timelineInfo.lastTempo = timelineInfo.currentTempo;
        beatInterval = 60f / timelineInfo.currentTempo;
    }

    [AOT.MonoPInvokeCallback(typeof(EVENT_CALLBACK))]
    static RESULT BeatEventCallback(EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
    {
        EventInstance instance = new EventInstance(instancePtr);

        // Retrieve the user data
        RESULT result = instance.getUserData(out IntPtr timelineInfoPtr);
        if (result != RESULT.OK)
        {
            Debug.LogError("Timeline Callback error: " + result);
        }
        else if (timelineInfoPtr != IntPtr.Zero)
        {
            // Get the object to store beat and marker details
            GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr);
            TimelineInfo timelineInfo = (TimelineInfo)timelineHandle.Target;

            // There's more info about the callback in the "parameter" variable.
            var parameter = (TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(TIMELINE_BEAT_PROPERTIES));
            timelineInfo.currentBeat = parameter.beat;
            timelineInfo.beatPosition = parameter.position;
            timelineInfo.currentTempo = parameter.tempo;
            
            if (InputIndicator.Instance && (parameter.beat == 1 | parameter.beat == 3))
            {
                var timeSinceBeat = (timelineInfo.currentPosition - parameter.position) / 1000;
                float temp = 15 + (float)(timeSinceBeat * beatInterval);
                temp %= 1.0f;
                InputIndicator.Instance.PlayAnimation("New Animation", -1, temp);
            }   
        }
        return RESULT.OK;
    }
}