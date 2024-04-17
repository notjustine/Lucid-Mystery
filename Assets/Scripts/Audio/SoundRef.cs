using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;


public class SoundRef : MonoBehaviour
{
    public static SoundRef Instance;
    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference attackSound { get; private set; }
    [field: SerializeField] public EventReference attackSwing { get; private set; }
    [field: SerializeField] public EventReference movementSound { get; private set; }
    [field: SerializeField] public EventReference dmgTaken { get; private set; }
    [field: SerializeField] public EventReference heal { get; private set; }
    
    [field: Header("Boss SFX")]
    [field: SerializeField] public EventReference missBeatSniperShot { get; private set; }
    [field: SerializeField] public EventReference steamAttack { get; private set; }
    [field: SerializeField] public EventReference turrentShot { get; private set; }
    
    [field: Header("Menu SFX")]
    [field: SerializeField] public EventReference menuSelect { get; private set; }
    [field: SerializeField] public EventReference menuMove { get; private set; }
    
    [field: Header("Background Music")]
    [field: SerializeField] public EventReference backgroundTrack { get; private set; }
    [field: SerializeField] public EventReference tutorialTrack { get; private set; }

    
    
    [field: Header("Ambient Sounds")]
    [field: SerializeField] public EventReference canShakes { get; private set; }
    [field: SerializeField] public EventReference gears { get; private set; }
    [field: SerializeField] public EventReference wind { get; private set; }
    [field: SerializeField] public EventReference pipeWater { get; private set; }
    [field: SerializeField] public EventReference randomClinks { get; private set; }
    [field: SerializeField] public EventReference scratch { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one Sound Reference");
        }

        Instance = this;
    }
}