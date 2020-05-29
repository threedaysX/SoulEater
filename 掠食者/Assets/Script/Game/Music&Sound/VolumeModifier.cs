using UnityEngine;
using UnityEngine.UI;

public class VolumeModifier : MonoBehaviour
{
    [Header("主音量")]
    public AudioSource masterSoundAudio;
    public Slider masterSoundSlider;
    private float masterOriginVolume;

    [Header("音樂音量")]
    public AudioSource musicSoundAudio;
    public Slider musicSoundSlider;
    private float musicOriginVolume;

    [Header("音效音量")]
    public AudioSource effectSoundAudio;
    public Slider effectSoundSlider;
    private float effectSoundOriginVolume;

    private void Awake()
    {
        ResetSliderVolume();
    }

    private void Start()
    {
        musicOriginVolume = GetSliderSoundVolume(musicSoundSlider);
        effectSoundOriginVolume = GetSliderSoundVolume(effectSoundSlider);
    }

    #region Slider
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
        masterOriginVolume = GetSliderSoundVolume(slider);
        masterSoundAudio.volume = masterOriginVolume;
        AdjustFinalBackgroundSoundVolume();
        AdjustFinalEffectSoundVolume();
        SetSoundDataToMemory(SoundStringData.masterSoundSlider, masterOriginVolume);
        SetSoundDataToMemory(SoundStringData.masterSound, musicSoundAudio.volume);
    }

    public void SetBackgroundVolumeBySlider(Slider slider)
    {
        musicOriginVolume = GetSliderSoundVolume(slider);
        AdjustFinalBackgroundSoundVolume();
    }

    public void SetEffectVolumeBySlider(Slider slider)
    {
        effectSoundOriginVolume = GetSliderSoundVolume(slider);
        AdjustFinalEffectSoundVolume();
    }
    #endregion

    public void AdjustFinalEffectSoundVolume()
    {
        if (effectSoundOriginVolume == 0)
            return;
        effectSoundAudio.volume = effectSoundOriginVolume * masterOriginVolume;
        SetSoundDataToMemory(SoundStringData.effectSoundSlider, effectSoundOriginVolume);
        SetSoundDataToMemory(SoundStringData.effectSound, musicSoundAudio.volume);
    }

    public void AdjustFinalBackgroundSoundVolume()
    {
        if (effectSoundOriginVolume == 0)
            return;
        musicSoundAudio.volume = musicOriginVolume * masterOriginVolume;
        SetSoundDataToMemory(SoundStringData.musicSoundSlider, musicOriginVolume);
        SetSoundDataToMemory(SoundStringData.musicSound, musicSoundAudio.volume);
    }

    private void SetSoundDataToMemory(string key, float soundVolume)
    {
        PlayerPrefs.SetFloat(key, soundVolume);
    }

    private void ResetSliderVolume()
    {
        if (PlayerPrefs.HasKey(SoundStringData.masterSoundSlider))
        {
            masterOriginVolume = PlayerPrefs.GetFloat(SoundStringData.masterSoundSlider);
            masterSoundSlider.value = masterOriginVolume;
        }
        if (PlayerPrefs.HasKey(SoundStringData.musicSoundSlider))
        {
            musicOriginVolume = PlayerPrefs.GetFloat(SoundStringData.musicSoundSlider);
            musicSoundSlider.value = musicOriginVolume;
        }
        if (PlayerPrefs.HasKey(SoundStringData.effectSoundSlider))
        {
            effectSoundOriginVolume = PlayerPrefs.GetFloat(SoundStringData.effectSoundSlider);
            effectSoundSlider.value = effectSoundOriginVolume;
        }
    }
}