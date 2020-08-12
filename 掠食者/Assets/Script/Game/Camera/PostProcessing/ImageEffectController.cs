using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.PostProcessing;
using System;

[RequireComponent(typeof(PostProcessVolume))]
public class ImageEffectController : Singleton<ImageEffectController>
{
    public PostProcessVolume volume;

    private RadialBlurPP _radialBlur = null;
    private Vignette _vignette = null;
    private Sequence bleedSequence;

    private void Start()
    {
        _radialBlur = volume.sharedProfile.GetSetting<RadialBlurPP>();
        _vignette = volume.sharedProfile.GetSetting<Vignette>();
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

    public void BleedVignette(bool start, params VignetteSetting[] settings)
    {
        bleedSequence.Kill();
        bleedSequence = DOTween.Sequence();
        if (!start)
        {
            _vignette.active = false;
            return;
        }
        _vignette.active = true;

        foreach (var setting in settings)
        {
            bleedSequence
                .Append(DOTween.To(() => _vignette.color.value, x => _vignette.color.value = x, setting.color, setting.duration))
                .Insert(0f, DOTween.To(() => _vignette.intensity.value, x => _vignette.intensity.value = x, setting.intensity, setting.duration))
                .Insert(0f, DOTween.To(() => _vignette.smoothness.value, x => _vignette.smoothness.value = x, setting.smoothness, setting.duration))
                .Insert(0f, DOTween.To(() => _vignette.roundness.value, x => _vignette.roundness.value = x, setting.roundness, setting.duration));
        }

        bleedSequence.Play();
    }
}

[Serializable]
public struct RadialBlurSetting
{
    public float strength;
    public float dist;
    public float duration;
}

[Serializable]
public struct VignetteSetting
{
    public Color color;
    public float intensity;
    public float smoothness;
    public float roundness;
    public float duration;
}
