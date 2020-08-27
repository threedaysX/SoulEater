Shader "ImageEffects/Flash"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FlashTex ("Flash Texture", 2D) = "while" {}
        _LineColor ("Line Color", Color) = (1,1,1,1)
        _Factor ("Factor", float) = 0.0
        _Strength ("Strength", float) = 0.0
        _Center ("Center", Vector) = (0.5, 0.5, 0.0, 0.0)
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
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
            sampler2D _FlashTex;
            float _Factor;
            float _Strength;
            float4 _Center;
            fixed4 _LineColor;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // float4 center = float4(0.5, 0.5, 0, 0);

                float2 dir = _Center - i.uv;

                fixed4 col2 = tex2D(_FlashTex, i.uv + dir * _Factor);

                if(col2.a > 0.9) {
                    return col;
                }

                fixed4 finalCol = lerp(col, _LineColor, _Strength);

                return finalCol;
            }
            ENDCG
        }
    }
}
