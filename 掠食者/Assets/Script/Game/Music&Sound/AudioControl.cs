using UnityEngine;

public class AudioControl : Singleton<AudioControl>
{
    public AudioSource masterSoundAudio;
    public AudioSource musicSoundAudio;
    public AudioSource effectSoundAudio;

    private void Awake()
    {
        ResetVolume();
    }

    public void ResetVolume()
    {
        if (PlayerPrefs.HasKey(SoundStringData.masterSound))
        {
            masterSoundAudio.volume = PlayerPrefs.GetFloat(SoundStringData.masterSound);
        }
        if (PlayerPrefs.HasKey(SoundStringData.musicSound))
        {
            musicSoundAudio.volume = PlayerPrefs.GetFloat(SoundStringData.musicSound);
        }
        if (PlayerPrefs.HasKey(SoundStringData.effectSound))
        {
            effectSoundAudio.volume = PlayerPrefs.GetFloat(SoundStringData.effectSound);
        }
    }

    public void PlaySound(AudioClip sound)
    {
        effectSoundAudio.PlayOneShot(sound);
    }

    public void PlayMusic(AudioClip music)
    {
        musicSoundAudio.PlayOneShot(music);
    }
}