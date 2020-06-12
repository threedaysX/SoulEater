Shader "ImageEffects/RadialBlur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SampleDist ("Dist", Float) = 1.0
        _SampleStrength("Strength", Float) = 2.0
        _Center ("Center", Vector) = (0.5, 0.5, 0.0, 0.0)
    }

    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

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
            }

            sampler2D _MainTex;
            float _SampleDist;
            float _SampleStrength;
            float2 _Center;

            fixed4 frag (v2f i) : SV_Target
            {
                float samples[10] = {-0.08, -0.05, -0.03, -0.02, -0.01, 0.01, 0.02, 0.03, 0.05, 0.08};
                float2 dir = _Center - i.uv;
                float dist = length(dir);
                dir = normalize(dir);

                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 sum = col;
                for (int j = 0; j < 10; ++j)
                {
                    sum += tex2D(_MainTex, i.uv + (dir * samples[j] * _SampleDist));
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