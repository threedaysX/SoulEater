Shader "ImageEffects/RadialBlur"
{
    HLSLINCLUDE

        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

    ENDHLSL

    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment frag

            /*#include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }*/
            
            TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
            float _SampleDist;
            float _SampleStrength;
            float2 _Center;

            float4 frag(VaryingsDefault i) : SV_Target
            {
                float samples[10] = {-0.08, -0.05, -0.03, -0.02, -0.01, 0.01, 0.02, 0.03, 0.05, 0.08};
                float2 dir = _Center - i.texcoord;
                float dist = length(dir);
                dir = normalize(dir);

                float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);

                float4 sum = col;
                for (int j = 0; j < 10; ++j)
                {
                    sum += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + (dir * samples[j] * _SampleDist));
                }
                sum /= 11.0f;
                float t = clamp(dist * _SampleStrength, 0.0f, 1.0f);
                col = lerp(col, sum, t);

                return col;
            }
            
            ENDHLSL
        }
    }
}