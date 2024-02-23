using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
   public static AudioManager instance { get; private set; }
   private List<EventInstance> eventInstances;
   private void Awake()
   {
       if (instance != null)
       {
           Debug.Log("Found more than one Audio Manager");
       }

       instance = this;
       eventInstances = new List<EventInstance>();
   }

   public void PlayOneShot(EventReference sound, Vector3 worldPos)
   {
       RuntimeManager.PlayOneShot(sound , worldPos);
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
   }

   public void ResumeAllEvents()
   {
       foreach (EventInstance eventInstance in eventInstances)
       {
           eventInstance.setPaused(false);
       }
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
}
