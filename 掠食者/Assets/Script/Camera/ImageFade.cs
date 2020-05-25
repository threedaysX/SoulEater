using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class ImageFade : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float fadeInTime;
    [SerializeField] private float fadeOutTime;

    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.CompareTag("Player"))
        {
            StartCoroutine(Transition(fadeInTime, fadeOutTime));
        }
    }

    private IEnumerator Transition(float fadeInDuration, float fadeOutDuration)
    {
        StartCoroutine(FadeIn(fadeInDuration));
        StartCoroutine(FadeOut(fadeOutDuration));
        yield break;
    }


    private IEnumerator FadeIn(float duration)
    {
        image.gameObject.SetActive(true);
        image.color = Color.black;

        float rate = 1f / duration;
        float progress = 0f;

        while (progress < 1f)
        {
            image.color = Color.Lerp(Color.clear, Color.black, progress);
            progress += rate * Time.deltaTime;
            yield return null;
        }

        image.color = Color.clear;
        image.gameObject.SetActive(false);
    }

    private IEnumerator FadeOut(float duration)
    {
        image.gameObject.SetActive(true);
        image.color = Color.black;

        float rate = 1f / duration;
        float progress = 0f;

        while(progress < 1f)
        {
            image.color = Color.Lerp(Color.black, Color.clear, progress);
            progress += rate * Time.deltaTime;
            yield return null;
        }

        image.color = Color.clear;
        image.gameObject.SetActive(false);
    }
}
