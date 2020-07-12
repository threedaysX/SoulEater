using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(PostProcessVolume))]
public class ImageEffectController : Singleton<ImageEffectController>
{
    public PostProcessVolume volume;

    private RadialBlurPP _radialBlur = null;

    private void Start()
    {
        volume = GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out _radialBlur);
    }

    public void StartRadialBlur(Ease ease, params RadialBlurSetting[] settings)
    {
        Sequence effectSequence = DOTween.Sequence();

        foreach (var setting in settings)
        {
            effectSequence
                .Append(DOTween.To(() => _radialBlur.m_Strength.value, x => _radialBlur.m_Strength.value = x, setting.strength, setting.duration).SetEase(ease))
                .Insert(0f, DOTween.To(() => _radialBlur.m_Dist.value, x => _radialBlur.m_Dist.value = x, setting.dist, setting.duration).SetEase(ease));
        }

        effectSequence.Play();
    }
}

public struct RadialBlurSetting
{
    public float strength;
    public float dist;
    public float duration;
}