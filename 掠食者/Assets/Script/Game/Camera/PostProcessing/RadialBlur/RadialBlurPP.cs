using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[System.Serializable]
[PostProcess(typeof(RadialBlurPPRenderer), PostProcessEvent.BeforeStack, "Custom/RadialBlur")]
public class RadialBlurPP : PostProcessEffectSettings
{
    public Vector2Parameter m_Center = new Vector2Parameter { value = new Vector2(0.5f, 0.5f) };

    [Range(0.0f, 2.0f), Tooltip("Sample Dist")]
    public FloatParameter m_Dist = new FloatParameter { value = 1.0f };

    [Range(0.0f, 10.0f), Tooltip("Sample Strength")]
    public FloatParameter m_Strength = new FloatParameter { value = 2.0f };
}

public sealed class RadialBlurPPRenderer : PostProcessEffectRenderer<RadialBlurPP>
{
    Shader _shader = Shader.Find("ImageEffects/RadialBlur");

    public override void Render(PostProcessRenderContext context)
    {
        if (_shader == null)
            return;

        var _sheet = context.propertySheets.Get(_shader);
        _sheet.properties.SetVector("_Center", new Vector4(settings.m_Center.value.x, settings.m_Center.value.y, 0.0f, 0.0f));
        _sheet.properties.SetFloat("_SampleDist", settings.m_Dist);
        _sheet.properties.SetFloat("_SampleStrength", settings.m_Strength);
        context.command.BlitFullscreenTriangle(context.source, context.destination, _sheet, 0);
    }
}