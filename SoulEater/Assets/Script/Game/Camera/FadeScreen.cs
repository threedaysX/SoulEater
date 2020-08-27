using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : Singleton<FadeScreen>
{
    public Image transition;
    public GameObject fadeImage;
    public List<SpriteSortInfo> originObjSortInfos;

    private void Start()
    {
        originObjSortInfos = new List<SpriteSortInfo>();
    }

    public IEnumerator Fade(float fadeInDuration, float fadeOutDuration)
    {
        StartCoroutine(FadeIn(fadeInDuration));
        StartCoroutine(FadeOut(fadeOutDuration));
        yield break;
    }

    public IEnumerator FadeIn(float duration)
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
    }

    public IEnumerator FadeOut(float duration)
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
    }

    public void HighlightObjects(float waitSeconds, params GameObject[] highlighObjs)
    {
        StartCoroutine(HighlightObjectsCoroutine(waitSeconds, highlighObjs));
    }

    private IEnumerator HighlightObjectsCoroutine(float waitSeconds, params GameObject[] highlighObjs)
    {
        yield return new WaitForSeconds(waitSeconds);

        foreach (var obj in highlighObjs)
        {
            var sr = obj.GetComponent<SpriteRenderer>();
            originObjSortInfos.Add(new SpriteSortInfo { gameObject = obj, layerName = sr.sortingLayerName, sortOrder = sr.sortingOrder });
            sr.sortingLayerName = "UI";
            sr.sortingOrder = 10;
        }
    }

    public void ResetGameObjectHighlight()
    {
        foreach (var origin in originObjSortInfos)
        {
            var sr = origin.gameObject.GetComponent<SpriteRenderer>();
            sr.sortingLayerName = origin.layerName;
            sr.sortingOrder = origin.sortOrder;
        }
    }
}

public struct SpriteSortInfo
{
    public GameObject gameObject;
    public string layerName;
    public int sortOrder;
}
