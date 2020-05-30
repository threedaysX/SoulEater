using System;
using System.Collections;
using UnityEngine;

public class MusicTransition : Singleton<MusicTransition>
{
    public float duration;

    public void SetDuration(float duration)
    {
        this.duration = duration;
    }

    public void StartFadeOutMusic(AudioSource audio)
    {
        StartCoroutine(FadeOutMusicAndAction(audio, duration));
    }

    public IEnumerator FadeOutMusicAndAction(AudioSource audio, float duration)
    {
        float startVolume = audio.volume;
        float timeleft = duration;
        while (timeleft > 0)
        {
            audio.volume -= startVolume * Time.deltaTime / duration;
            timeleft -= Time.deltaTime;
            yield return null;
        }
    }
}
