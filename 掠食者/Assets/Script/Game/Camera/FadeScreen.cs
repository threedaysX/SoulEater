using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : Singleton<FadeScreen>
{
    public Image transition;

    public IEnumerator Fade(float fadeInDuration, float fadeOutDuration)
    {
        StartCoroutine(FadeIn(fadeInDuration));
        StartCoroutine(FadeOut(fadeOutDuration));
        yield break;
    }

    private IEnumerator FadeIn(float duration)
    {
        transition.gameObject.SetActive(true);
        transition.color = Color.black;

        float rate = 1f / duration;
        float progress = 0f;

        while (progress < 1f)
        {
            transition.color = Color.Lerp(Color.clear, Color.black, progress);
            progress += rate * Time.deltaTime;
            yield return null;
        }

        transition.color = Color.clear;
        transition.gameObject.SetActive(false);
    }

    private IEnumerator FadeOut(float duration)
    {
        transition.gameObject.SetActive(true);
        transition.color = Color.black;

        float rate = 1f / duration;
        float progress = 0f;

        while(progress < 1f)
        {
            transition.color = Color.Lerp(Color.black, Color.clear, progress);
            progress += rate * Time.deltaTime;
            yield return null;
        }

        transition.color = Color.clear;
        transition.gameObject.SetActive(false);
    }
}
