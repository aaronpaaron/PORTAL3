Shader "Custom/AlwaysOnTopWithTexture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _PixelScale ("Pixel Scale", Float) = 1.0
        _NoiseAmount ("Noise Amount", Float) = 0.1
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" }
        Pass
        {
            ZWrite Off
            ZTest Always
            ColorMask RGB
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _Color;
            float _PixelScale;
            float _NoiseAmount;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float2 Pixelate(float2 uv)
            {
                // Pikseloidaan UV-koordinaatit
                return floor(uv * _PixelScale) / _PixelScale;
            }

            half4 frag (v2f i) : SV_Target
            {
                // Pikseloidaan UV-koordinaatit
                float2 pixelatedUV = Pixelate(i.uv);

                // Haetaan tekstuuri
                half4 texColor = tex2D(_MainTex, pixelatedUV) * _Color;

                // Lisätään satunnaista värinää
                float noise = (sin(dot(pixelatedUV * 100.0, float2(12.9898, 78.233))) * 43758.5453);
                noise = frac(noise); // Normalisoidaan [0, 1] väliin
                noise = (noise - 0.5) * _NoiseAmount; // Skaalataan

                texColor.rgb += noise; // Lisätään värinää
                return texColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
