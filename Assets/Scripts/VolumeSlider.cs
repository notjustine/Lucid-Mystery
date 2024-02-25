using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public enum VolumeType
    {
        MASTER,
        MUSIC,
        SFX
    }

    [Header("Type")] 
    [SerializeField] private VolumeType volumeType;

    public void OnSliderValueChanged(float value)
    {
        AudioManager.instance.AdjustVolume(value, volumeType);
    }

}