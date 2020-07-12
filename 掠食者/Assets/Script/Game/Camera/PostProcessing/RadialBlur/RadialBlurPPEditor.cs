using UnityEngine.Rendering.PostProcessing;
using UnityEditor.Rendering.PostProcessing;

[PostProcessEditor(typeof(RadialBlurPP))]
public class RadialBlurPPEditor : PostProcessEffectEditor<RadialBlurPP>
{
    SerializedParameterOverride m_Center;
    SerializedParameterOverride m_Dist;
    SerializedParameterOverride m_Strength;

    public override void OnEnable()
    {
        base.OnEnable();

        m_Center = FindParameterOverride(x => x.m_Center);
        m_Dist = FindParameterOverride(x => x.m_Dist);
        m_Strength = FindParameterOverride(x => x.m_Strength);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PropertyField(m_Center);
        PropertyField(m_Strength);
        PropertyField(m_Dist);
    }
}
