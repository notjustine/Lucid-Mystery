using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static AudioManager instance { get; private set; }
    [field: SerializeField] private List<EventInstance> eventInstances;
    private MusicEventHandler musicEventHandler;
    private AmbienceHandler ambienceHandler;
    private Music music;

    private Bus masterBus;
    private Bus musicBus;
    private Bus sfxBus;
    private Bus ambienceBus;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one Audio Manager");
        }

        instance = this;
        eventInstances = new List<EventInstance>();
        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
        ambienceBus = RuntimeManager.GetBus("bus:/Ambience");

        AdjustVolume(PlayerPrefs.GetFloat(VolumeSlider.VolumeType.MASTER.ToString(), 1),
            VolumeSlider.VolumeType.MASTER);
        AdjustVolume(PlayerPrefs.GetFloat(VolumeSlider.VolumeType.MUSIC.ToString(), 1), VolumeSlider.VolumeType.MUSIC);
        AdjustVolume(PlayerPrefs.GetFloat(VolumeSlider.VolumeType.SFX.ToString(), 1), VolumeSlider.VolumeType.SFX);
        AdjustVolume(PlayerPrefs.GetFloat(VolumeSlider.VolumeType.SFX.ToString(), 1), VolumeSlider.VolumeType.AMBIENCE);
        
        musicEventHandler = FindObjectOfType<MusicEventHandler>();
        ambienceHandler = FindObjectOfType<AmbienceHandler>();
        music = FindObjectOfType<Music>();
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public void PlayOneShotAttached(EventReference sound, GameObject gameObject)
    {
        RuntimeManager.PlayOneShotAttached(sound, gameObject);
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public void PauseAllEvents()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.setPaused(true);
        }
        ambienceHandler.PauseAmbience();
    }

    public void ResumeAllEvents()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.setPaused(false);
        }
        ambienceHandler.UnPauseAmbience();
    }
    

    private void OnDestroy()
    {
        // Audio Cleanup
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }

    public void AdjustVolume(float volume, VolumeSlider.VolumeType bus)
    {
        if (bus.Equals(VolumeSlider.VolumeType.MASTER))
        {
            masterBus.setVolume(volume);
        }
        else if (bus.Equals(VolumeSlider.VolumeType.MUSIC))
        {
            musicBus.setVolume(volume);
        }
        else if (bus.Equals(VolumeSlider.VolumeType.SFX))
        {
            sfxBus.setVolume(volume);
        }
        else if (bus.Equals(VolumeSlider.VolumeType.AMBIENCE))
        {
            ambienceBus.setVolume(volume);
        }
        else
        {
            Debug.Log("Bus not found");
        }
    }

    public void TriggerPhaseOneMusic()
    {
       // musicEventHandler.SetMainMusicPhaseParameter(1);
       music.SetParamter(1);
    }
    public void TriggerPhaseTwoMusic()
    {
        // musicEventHandler.SetMainMusicPhaseParameter(2);
        music.SetParamter(2);
    }
}