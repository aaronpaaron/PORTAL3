Shader "Custom/PS1Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _EmissiveTex ("Emissive Texture", 2D) = "black" {}
        _SpecularTex ("Specular Texture", 2D) = "black" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _PixelationAmount ("Pixelation Amount", Range(1, 256)) = 100
        _JitterAmount ("Jitter Amount", Range(0, 0.1)) = 0.01
        _SpecularIntensity ("Specular Intensity", Range(0, 1)) = 0.5
        _AmbientIntensity ("Ambient Intensity", Range(0, 1)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Name "FORWARD"
            Tags { "LightMode"="ForwardBase" }
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers gles xbox360 ps3
            #include "UnityCG.cginc"

            // Properties
            sampler2D _MainTex;
            sampler2D _EmissiveTex;
            sampler2D _SpecularTex;
            sampler2D _NormalMap;
            float4 _MainTex_ST; // Tiling and Offset are packed into this

            float _PixelationAmount;
            float _JitterAmount;
            float _SpecularIntensity;
            float _AmbientIntensity;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            // Vertex function to pass the data to fragment shader
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = UnityObjectToWorldNormal(v.normal);

                // Calculate jitter based on time and position
                float jitter = _JitterAmount * sin(_Time.y * 10.0 + v.vertex.x * 0.5 + v.vertex.z * 0.5);
                o.pos.xy += jitter;

                return o;
            }

            // Fragment shader for PS1 effect
            fixed4 frag (v2f i) : SV_Target
            {
                // Quantize the UV coordinates for texture distortion
                float2 uvQuantized = floor(i.uv * _PixelationAmount) / _PixelationAmount;

                // Sample the textures
                fixed4 mainCol = tex2D(_MainTex, uvQuantized);
                fixed4 emissiveCol = tex2D(_EmissiveTex, uvQuantized);
                fixed4 specularCol = tex2D(_SpecularTex, uvQuantized);
                fixed4 normalMap = tex2D(_NormalMap, uvQuantized);

                // Decode normal from normal map
                float3 normal = UnpackNormal(normalMap);

                // Lighting calculations
                fixed3 worldNormal = normalize(i.normal);
                fixed3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                fixed diffuse = max(0, dot(worldNormal, lightDir));

                // Ambient lighting to brighten shadows
                fixed ambient = _AmbientIntensity;
                
                // Calculate specular reflection
                fixed3 viewDir = normalize(_WorldSpaceCameraPos - i.pos.xyz);
                fixed3 reflectDir = reflect(-lightDir, worldNormal);
                fixed specular = pow(max(0, dot(viewDir, reflectDir)), 16) * _SpecularIntensity;
                
                // Combine colors and apply lighting
                fixed4 col = mainCol * (diffuse + ambient) + emissiveCol;
                col += specular * specularCol; // Add specular highlight
                
                return col;
            }
            ENDCG
        }
        
        // Add Shadow Passes
        Pass
        {
            Name "SHADOWCASTER"
            Tags { "LightMode"="ShadowCaster" }
            
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
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return fixed4(1, 1, 1, 1);
            }
            ENDCG
        }
    }
    
    FallBack "Diffuse"
}