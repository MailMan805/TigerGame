using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private Slider slider;
    public bool isMusicSlider;
    public string sliderName;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        if(isMusicSlider)
        {
            slider.value = AudioManager.Instance.musicSource.volume;
        }
        else
        {
            slider.value = AudioManager.Instance.soundEffectsSource.volume;
        }
    }

    public void AdjustVolume()
    {
        if(isMusicSlider)
        {
            AudioManager.Instance.musicSource.volume = slider.value;
        }
        else
        {
            AudioManager.Instance.soundEffectsSource.volume = slider.value;
        }
    }
}
