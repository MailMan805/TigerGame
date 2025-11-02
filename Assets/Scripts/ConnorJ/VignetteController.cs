using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class VignetteController : MonoBehaviour
{
    public static VignetteController instance;

    public UnityEvent FlashVignette;

    [Header("Intensity")]
    [Range(0.0f, 1.0f)][Tooltip("What the intensity of the vignette is during normal gameplay")][SerializeField] float normalIntensity = 0.0f;
    [Range(0.0f, 1.0f)][SerializeField] float maxIntensity = 0.5f;

    [Header("Time")]
    [SerializeField] float vignetteTransitionSpeed = 0.2f;
    [SerializeField] float flashEffectDuration = 2f;
    [SerializeField] float vignetteDeathSpeed = 0.4f;

    Volume volume;
    Vignette vignette;

    float previousIntensity;
    float newIntensity;

    float time = 0;

    bool isDead = false;

    const float INTERNAL_MAX_VALUE = 1.0f;
    const float MAX_TIME = 1.0f;

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

        FlashVignette.AddListener(CommitFlash);
        GameManager.instance?.OnDeath.AddListener(DeathVignette);
    }

    void SetNewIntensity(float intensity)
    {
        time = 0f;

        //intensity can only be between the intensity during normal gameplay and 1.0f (the max intensity can be)

        intensity = Mathf.Clamp(intensity, normalIntensity, INTERNAL_MAX_VALUE);

        previousIntensity = vignette.intensity.value;
        newIntensity = intensity;
        
    }

    void SetNewSmoothness(float smoothness)
    {
        vignette.smoothness.value = smoothness;
    }

    void CommitFlash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashEffect());
    }

    void DeathVignette()
    {
        StopAllCoroutines();
        isDead = true;
        SetNewIntensity(INTERNAL_MAX_VALUE);
        SetNewSmoothness(INTERNAL_MAX_VALUE);

        StartCoroutine(VignetteTransition());
    }

    IEnumerator FlashEffect()
    {
        SetNewIntensity(maxIntensity);

        yield return VignetteTransition();
        
        yield return new WaitForSeconds(flashEffectDuration);

        SetNewIntensity(normalIntensity);

        yield return VignetteTransition();
    }

    void IncrementTime()
    {
        time += isDead? vignetteDeathSpeed * Time.deltaTime : vignetteTransitionSpeed * Time.deltaTime;

        if (time > 1f)
        {
            time = 1f;
        }
    }

    IEnumerator VignetteTransition()
    {
        while (time < MAX_TIME)
        {
            vignette.intensity.value = Mathf.LerpUnclamped(previousIntensity, newIntensity, time);
            IncrementTime();

            yield return null;
        }

        vignette.intensity.value = newIntensity;
        yield return null;
    }
}
