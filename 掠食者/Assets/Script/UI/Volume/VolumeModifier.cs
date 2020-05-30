using UnityEngine;
using UnityEngine.UI;

public class VolumeModifier : MonoBehaviour
{
    public AudioSource masterSoundAudio;
    public AudioSource backgroundSoundAudio;
    public Slider backgroundSoundSlider;
    private float backgroundOriginVolume;
    public AudioSource effectSoundAudio;
    public Slider effectSoundSlider;
    private float effectOriginVolume;

    private void Start()
    {
        backgroundOriginVolume = GetSliderSoundVolume(backgroundSoundSlider);
        effectOriginVolume = GetSliderSoundVolume(effectSoundSlider);
    }

    public float GetSliderSoundVolume(Slider slider)
    {
        return slider.value;
    }

    public void SetVolume(AudioSource audio, float volume)
    {
        audio.volume = volume;
    }

    public void SetVolumeBySlider(AudioSource audio, Slider slider)
    {
        audio.volume = slider.value;
    }

    public void SetMasterVolumeBySlider(Slider slider)
    {
        masterSoundAudio.volume = GetSliderSoundVolume(slider);
        AdjustFinalBackgroundSoundVolume();
        AdjustFinalEffectSoundVolume();
    }

    public void SetBackgroundVolumeBySlider(Slider slider)
    {
        backgroundOriginVolume = GetSliderSoundVolume(slider);
        AdjustFinalBackgroundSoundVolume();
    }

    public void SetEffectVolumeBySlider(Slider slider)
    {
        effectOriginVolume = GetSliderSoundVolume(slider);
        AdjustFinalEffectSoundVolume();
    }

    public void AdjustFinalEffectSoundVolume()
    {
        effectSoundAudio.volume = effectOriginVolume * GetMasterVolume();
    }

    public void AdjustFinalBackgroundSoundVolume()
    {
        backgroundSoundAudio.volume = backgroundOriginVolume * GetMasterVolume();
    }

    public float GetMasterVolume()
    {
        return masterSoundAudio.volume;
    }
}