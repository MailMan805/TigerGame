using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class VignetteController : MonoBehaviour
{
    public static VignetteController instance;

    [Header("Intensity")]
    [Range(0.0f, 1.0f)][Tooltip("What the intensity of the vignette is during normal gameplay")] public float normalIntensity = 0.0f;
    [Range(0.0f, 1.0f)] public float maxIntensity = 0.5f;

    [Header("Time")]
    public float vignetteTransitionSpeed = 0.2f;
    public float flashEffectDuration = 2f;

    Volume volume;
    Vignette vignette;

    float previousIntensity;
    float newIntensity;

    float time = 0;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        volume = GetComponent<Volume>();

        if (volume.profile.TryGet<Vignette>(out vignette))
        {
            vignette.intensity.value = normalIntensity;
        }
    }

    private void Update()
    {
        if (volume.profile.TryGet<Vignette>(out vignette))
        {
            vignette.intensity.value = Mathf.LerpUnclamped(previousIntensity, newIntensity, time);
        }

        time += vignetteTransitionSpeed * Time.deltaTime;

        if (time > 1f)
        {
            time = 1f;
        }
    }

    void SetNewIntensity(float intensity)
    {
        time = 0f;

        previousIntensity = vignette.intensity.value;
        newIntensity = intensity;

        if (intensity < normalIntensity) { intensity = normalIntensity; }
        if (intensity > maxIntensity) { intensity = maxIntensity; }
    }

    public IEnumerator FlashEffect()
    {
        SetNewIntensity(maxIntensity);
        yield return new WaitForSeconds(flashEffectDuration);
        SetNewIntensity(normalIntensity);
    }
}
