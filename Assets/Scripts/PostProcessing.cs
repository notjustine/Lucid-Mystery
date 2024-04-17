using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessing : MonoBehaviour
{
    public Volume volume;
    private Vignette vignette;
    public PlayerStatus playerStatus;
    public Color color = new Color(0.00f, 0.00f, 0.00f);
    public Vector2 center = new Vector2(0.50f, 0.50f);
    public float lowHealthIntensity = 1f; // Intensity value when health is low (5% or lower)
    public float transitionSpeed = 1.0f;
    public float lerpThreshold = 0.4f; // Health percentage at which intensity interpolation starts

    void Start()
    {
        volume.profile.TryGet(out vignette);
        UpdateVignetteParameters();
    }

    void Update()
    {
        if (vignette == null || playerStatus == null)
            return;

        // Calculate health percentage
        float healthPercentage = (float)playerStatus.currHealth / playerStatus.maxHealth;

        // Calculate intensity based on health percentage and threshold
        float targetIntensity = Mathf.Lerp(0.3f, 0.0f, Mathf.Clamp01((healthPercentage - lerpThreshold) / lerpThreshold));

        // Smoothly transition intensity
        vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, targetIntensity, transitionSpeed * Time.deltaTime);

        // Update other vignette parameters
        UpdateVignetteParameters();
    }

    void UpdateVignetteParameters()
    {
        vignette.color.value = color;
        vignette.center.value = center;
        vignette.smoothness.value = 1.0f;
        vignette.rounded.value = false;
    }
}
