Shader "Custom/AlwaysOnTopWithTexture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}  // Lisätään tekstuurin ominaisuus
        _Color ("Color", Color) = (1,1,1,1)    // Väri-ominaisuus
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
                float2 uv : TEXCOORD0;   // UV-koordinaatit tekstuuria varten
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;  // Tekstuurin muuttuja
            float4 _Color;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;  // Siirretään UV-koordinaatit fragment shaderille
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                // Haetaan tekstuuri ja kerrotaan se värillä
                half4 texColor = tex2D(_MainTex, i.uv);
                return texColor * _Color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
