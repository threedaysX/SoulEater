using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIImageControll : Singleton<UIImageControll>
{
    public float SetImageFillAmount(Image image, float max, float current)
    {
        return (image.fillAmount = current / max);
    }

    public float SetImageFillAmount(Image image, Image fadeImage, float max, float current)
    {
        float resultPercentage = (image.fillAmount = current / max);
        StartCoroutine(ImageFadeInTimes(fadeImage, 0.15f, resultPercentage));
        return resultPercentage;
    }

    private IEnumerator ImageFadeInTimes(Image fadeImage, float duration, float targetFillAmount)
    {
        float timeleft = duration;
        float originFillAmount = fadeImage.fillAmount;
        float step = originFillAmount - targetFillAmount;
        while (timeleft > 0)
        {
            if (timeleft > Time.deltaTime)
                fadeImage.fillAmount -= (step * Time.deltaTime / duration);
            else
                fadeImage.fillAmount -= (step * timeleft / duration);

            timeleft -= Time.deltaTime;
            yield return null;
        }
    }
}
