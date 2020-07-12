public class UIImageControll : Singleton<UIImageControll>
{
    public float SetImageFillAmount(UnityEngine.UI.Image image, float max, float current)
    {
        return (image.fillAmount = current / max);
    }
}
