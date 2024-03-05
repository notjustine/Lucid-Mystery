using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public class VolumeType
    {
        public string Value { get; private set; }   
        private VolumeType(string value)
        {
            Value = value;
            
        }
        public static VolumeType MASTER = new VolumeType("Master");
        public static VolumeType MUSIC = new VolumeType("Music");
        public static VolumeType SFX = new VolumeType("SFX");
        public static VolumeType AMBIENCE = new VolumeType("Ambience");
        
        public override string ToString()
        {
            return Value;
        }

        public override bool Equals(object obj)
        {
            try
            {
                return Value.Equals(obj.ToString());
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }
    }

    [Header("Type")] 
    [ SerializeField] private string volumeName;
    private VolumeType volumeType;
    private Slider slider;
    public void OnSliderValueChanged(float value)
    {
        AudioManager.instance.AdjustVolume(value, volumeType);
        PlayerPrefs.SetFloat(volumeType.ToString(), value);
    }

    void Start()
    {
        slider = GetComponent<Slider>();
        switch (volumeName)
        {
            case "Master":
                volumeType = VolumeType.MASTER;
                break;
            case "Music":
                volumeType = VolumeType.MUSIC;
                break;
            case "SFX":
                volumeType = VolumeType.SFX;
                break;
            case "Ambience":
                volumeType = VolumeType.AMBIENCE;
                break;
            default:
                Debug.Log("Volume name not found.");
                break;
        }

        slider.value = PlayerPrefs.GetFloat(volumeType.ToString(), 1);
    }

    // void Update()
    // {
    //     slider.value = PlayerPrefs.GetFloat(volumeType.ToString(), 1);
    // }

}