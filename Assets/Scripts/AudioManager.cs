using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
   public static AudioManager instance { get; private set; }
   private List<EventInstance> eventInstances = new List<EventInstance>();
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
   
   public EventInstance CreateEventInstance(EventReference eventReference)
   {
       EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference.Path);
       eventInstances.Add(eventInstance);
       return eventInstance;
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
