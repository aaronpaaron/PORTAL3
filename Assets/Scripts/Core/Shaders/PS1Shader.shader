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

        // ForwardBase pass (handles the main directional light)
        Pass
        {
            Name "FORWARD"
            Tags { "LightMode"="ForwardBase" }
//            Tags { "LightMode"="Vertex" }
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers gles xbox360 ps3
            #include "UnityCG.cginc"
            #include "Lighting.cginc" // Include lighting macros

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
                float3 worldPos : TEXCOORD1;
            };

            // Vertex function to pass the data to fragment shader
            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz; // Calculate world position

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

                // Calculate directional light (main light) contributions
                fixed3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                fixed diffuse = max(0, dot(worldNormal, lightDir));

                // Fetch the main directional light color from the lighting system
                fixed3 lightColor = _LightColor0.rgb;

                // Apply light color to diffuse lighting
                diffuse *= lightColor;

                // Ambient lighting to brighten shadows
                fixed ambient = _AmbientIntensity;
                
                // Calculate specular reflection
                fixed3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                fixed3 reflectDir = reflect(-lightDir, worldNormal);
                fixed specular = pow(max(0, dot(viewDir, reflectDir)), 16) * _SpecularIntensity;
                
                // Apply light color to specular reflection
                specular *= lightColor;

                // Combine colors and apply lighting
                fixed4 col = mainCol * (diffuse + ambient) + emissiveCol;
                col += specular * specularCol; // Add specular highlight
                
                return col;
            }
            ENDCG
        }

        // Additive pass for point lights and additional lights
        Pass
        {
            Name "FORWARD_ADD"
            Tags { "LightMode" = "ForwardAdd" }

            Blend One One // Additive blending for multiple lights

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment fragAdd
            #pragma multi_compile_fwdadd
            #include "UnityCG.cginc"
            #include "Lighting.cginc" // Include lighting macros

            // Properties
            sampler2D _MainTex;
            sampler2D _EmissiveTex;
            sampler2D _SpecularTex;
            sampler2D _NormalMap;
            float4 _MainTex_ST;

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
                float3 worldPos : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            // Fragment shader for additional lights (e.g., point lights)
            fixed4 fragAdd(v2f i) : SV_Target
            {
                float2 uvQuantized = floor(i.uv * _PixelationAmount) / _PixelationAmount;
                fixed4 mainCol = tex2D(_MainTex, uvQuantized);
                fixed4 specularCol = tex2D(_SpecularTex, uvQuantized);
                fixed4 normalMap = tex2D(_NormalMap, uvQuantized);
                float3 normal = UnpackNormal(normalMap);
                fixed3 worldNormal = normalize(i.normal);

                // Get light direction and light color from the light system
                fixed3 lightDir = normalize(_WorldSpaceLightPos0.xyz - i.worldPos); // For point lights
                fixed3 lightColor = _LightColor0.rgb;
                
                // Diffuse and specular lighting
                fixed diffuse = max(0, dot(worldNormal, lightDir));
                diffuse *= lightColor;

                fixed3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                fixed3 reflectDir = reflect(-lightDir, worldNormal);
                fixed specular = pow(max(0, dot(viewDir, reflectDir)), 16) * _SpecularIntensity;
                specular *= lightColor;

                // Combine colors
                fixed4 col = mainCol * diffuse + specular * specularCol;
                return col;
            }
            ENDCG
        }

        // Shadow pass (optional)
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