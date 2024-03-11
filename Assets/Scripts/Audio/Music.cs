using UnityEngine;
using System;
using System.Runtime.InteropServices;
using FMODUnity;
public class Music : MonoBehaviour
{
    private FMOD.ChannelGroup masterChannelGroup;
    
    [Header("OPTIONS:")]
    public float upBeatDivisor = 2f; // This value changes the offset of the up beats. Changing this value will "swing" the up beats.

    private int masterSampleRate;
    private double currentSamples = 0;
    private double currentTime = 0f;

    private ulong dspClock;
    private ulong parentDSP;

    private static double beatInterval = 0f; // This is the time between each beat;
    private static double lastBeatInterval = 0f; // This is the previous time between each beat. It's what the "beatInterval" was before a tempo change.

    private static bool justHitBeat = false;

    private double tempoTrackDSPStartTime;

    // private static string markerString = "";
    private static bool justHitMarker = false;
    private static int markerTime;

    // public delegate void BeatEventDelegate();
    // public static event BeatEventDelegate fixedBeatUpdate; // Subscribe any function you wan't to happen on the down beat to this event! DON'T FORGET TO UNSUBSCRIBE BEFORE DESTROYING YOU GAMEOBJECTS!

    private double lastFixedBeatTime = -2;
    private double lastFixedBeatDSPTime = -2;

    // public static event BeatEventDelegate upBeatUpdate; // Subscribe any function you wan't to happen on the up beat to this event.

    private double lastUpBeatTime = -2;
    private double lastUpBeatDSPTime = -2;

    private bool hasDoneEnemyBeat = false;

    //
    // public delegate void TempoUpdateDelegate(float beatInterval);
    // public static event TempoUpdateDelegate tempoChanged;
    //
    // public delegate void MarkerListenerDelegate();
    // public static event MarkerListenerDelegate markerUpdated;

    public static Music instance;
    private float input_delay = 175f;

    [SerializeField]
    private EventReference music;

    private FMOD.Studio.PLAYBACK_STATE musicPlayState;
    private FMOD.Studio.PLAYBACK_STATE lastMusicPlayState;

    [StructLayout(LayoutKind.Sequential)]
    public class TimelineInfo
    {
        public int currentBeat = 0;
        public int currentBar = 0;
        public int beatPosition = 0;
        public float currentTempo = 0;
        public float lastTempo = 0;
        public int currentPosition = 0;
        public double songLength = 0;
        public FMOD.StringWrapper lastMarker = new FMOD.StringWrapper();
    }

    public TimelineInfo timelineInfo = null;

    private GCHandle timelineHandle;

    private FMOD.Studio.EVENT_CALLBACK beatCallback;
    private FMOD.Studio.EventDescription descriptionCallback;

    public FMOD.Studio.EventInstance musicPlayEvent;

    private void Start()
    {
        instance = this;
        FMOD.Studio.EventDescription des;
        
        // FMOD.Studio.System sys = RuntimeManager.StudioSystem;
        // FMOD.Studio.ADVANCEDSETTINGS advancedsettings;
        // sys.getAdvancedSettings(out advancedsettings);
        // advancedsettings.studioupdateperiod = 10;
        // sys.setAdvancedSettings(advancedsettings);
        
        musicPlayEvent.getDescription(out des);
        des.loadSampleData();
        musicPlayEvent = AudioManager.instance.CreateEventInstance(SoundRef.Instance.backgroundTrack);
        StartMusic();
    }

    private void AssignMusicCallbacks()
    {
        timelineInfo = new TimelineInfo();
        beatCallback = new FMOD.Studio.EVENT_CALLBACK(BeatEventCallback);

        timelineHandle = GCHandle.Alloc(timelineInfo, GCHandleType.Pinned);
        musicPlayEvent.setUserData(GCHandle.ToIntPtr(timelineHandle));
        musicPlayEvent.setCallback(beatCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER);

        musicPlayEvent.getDescription(out descriptionCallback);
        descriptionCallback.getLength(out int length);

        timelineInfo.songLength = length;

        FMODUnity.RuntimeManager.CoreSystem.getMasterChannelGroup(out masterChannelGroup);

        FMODUnity.RuntimeManager.CoreSystem.getSoftwareFormat(out masterSampleRate, out FMOD.SPEAKERMODE speakerMode, out int numRawSpeakers);
    }

    private void StartMusic()
    {
        musicPlayEvent.start();
        AssignMusicCallbacks();
    }

    private void SetTrackStartInfo()
    {
        UpdateDSPClock();

        tempoTrackDSPStartTime = currentTime;
        lastFixedBeatTime = 0f;
        lastFixedBeatDSPTime = currentTime;
    }

    private void UpdateDSPClock()
    {
        masterChannelGroup.getDSPClock(out dspClock, out parentDSP);

        currentSamples = dspClock;
        currentTime = currentSamples / masterSampleRate;
    }

    private void Update()
    {

        musicPlayEvent.getPlaybackState(out musicPlayState);

        if (lastMusicPlayState != FMOD.Studio.PLAYBACK_STATE.PLAYING && musicPlayState == FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            SetTrackStartInfo();
        }

        lastMusicPlayState = musicPlayState;

        if (musicPlayState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            return;
        }

        musicPlayEvent.getTimelinePosition(out timelineInfo.currentPosition);

        UpdateDSPClock();

        CheckTempoMarkers();

        if (beatInterval == 0f)
        {
            return;
        }
        // Debug.LogWarning(currentTime);
        // Debug.Log(timelineInfo.beatPosition);
        if (timelineInfo.currentBeat == 1 | timelineInfo.currentBeat == 3)
        {  
            // Debug.Log("HELLO WORLD");
            if (timelineInfo.beatPosition + input_delay >= currentTime * 1000)
            {
                
                InputIndicator.Instance.active = true;
                MusicEventHandler.beatCheck = true;
                return;

            }

            MusicEventHandler.beatCheck = false;
            InputIndicator.Instance.active = false;
        } else if (timelineInfo.currentBeat == 2 | timelineInfo.currentBeat == 4)
        {
            if (timelineInfo.beatPosition + input_delay <= currentTime * 1000)
            {
                InputIndicator.Instance.active = false;
                MusicEventHandler.beatCheck = false;
                return;
            }
            
            InputIndicator.Instance.active = true;
            MusicEventHandler.beatCheck = true;
        }
    
        CheckNextBeat();
    }

    private float UpBeatPosition()
    {
        return ((float)beatInterval / upBeatDivisor);
    }

    private void CheckNextBeat()
    {
        float fixedSongPosition = (float)(currentTime - tempoTrackDSPStartTime);
        float upBeatSongPosition = fixedSongPosition + UpBeatPosition();

        // FIXED BEAT (down beat)
        if (fixedSongPosition >= lastFixedBeatTime + beatInterval)
        {

            float correctionAmount = Mathf.Repeat(fixedSongPosition, (float)beatInterval); // This is the amount of time that we're off from the beat...

            lastFixedBeatTime = (fixedSongPosition - correctionAmount); // ... we subtract that time from the current time to correct the timing off the next beat.
            lastFixedBeatDSPTime = (currentTime - correctionAmount); // So if this beat is late by 0.1 seconds, the next beat will happen 0.1 seconds sooner.

        }

        // UP BEAT
        if (upBeatSongPosition >= lastUpBeatTime + beatInterval)
        {
            float correctionAmount = Mathf.Repeat(upBeatSongPosition, (float)beatInterval);
            lastUpBeatTime = (upBeatSongPosition - correctionAmount);
            lastUpBeatDSPTime = ((currentTime + UpBeatPosition()) - correctionAmount);
        }
    }
    

    private void CheckTempoMarkers()
    {
        if (timelineInfo.currentTempo != timelineInfo.lastTempo)
            SetTrackTempo();
    }

    private void SetTrackTempo()
    {
        musicPlayEvent.getTimelinePosition(out int currentTimelinePos);

        float offset = (currentTimelinePos - timelineInfo.beatPosition) / 1000f;


        tempoTrackDSPStartTime = currentTime - offset;

        lastFixedBeatTime = 0f;
        lastFixedBeatDSPTime = tempoTrackDSPStartTime;

        lastUpBeatTime = 0f;
        lastUpBeatDSPTime = tempoTrackDSPStartTime;

        lastBeatInterval = beatInterval;

        timelineInfo.lastTempo = timelineInfo.currentTempo;

        beatInterval = 60f / timelineInfo.currentTempo;

        // tempoChanged((float)beatInterval);
    }

    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    static FMOD.RESULT BeatEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
    {
        FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(instancePtr);

        // Retrieve the user data
        FMOD.RESULT result = instance.getUserData(out IntPtr timelineInfoPtr);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogError("Timeline Callback error: " + result);
        }
        else if (timelineInfoPtr != IntPtr.Zero)
        {
            // Get the object to store beat and marker details
            GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr);
            TimelineInfo timelineInfo = (TimelineInfo)timelineHandle.Target;

            switch (type)
            {
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT:
                    {
                        // There's more info about the callback in the "parameter" variable.

                        var parameter = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
                        timelineInfo.currentBar = parameter.bar;
                        timelineInfo.currentBeat = parameter.beat;
                        timelineInfo.beatPosition = parameter.position;
                        timelineInfo.currentTempo = parameter.tempo;

                        justHitBeat = true;
                    }
                    break;
                // case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER:
                //     {
                //         // Same here.
                //
                //         var parameter = (FMOD.Studio.TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
                //
                //         timelineInfo.lastMarker = parameter.name;
                //         markerString = parameter.name;
                //         markerTime = parameter.position;
                //         justHitMarker = true;
                //     }
                //     break;
            }
        }
        return FMOD.RESULT.OK;
    }

    public void setParamter(int phase)
    {
        musicPlayEvent.setParameterByName("current_phase", phase);
        Debug.Log("Phase trigger");
    }

}