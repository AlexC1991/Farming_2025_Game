Shader "Custom/SpintiresTerrainDeformation"
{
    Properties
    {
        _MainTex ("Mud Texture", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _Smoothness ("Smoothness", Range(0, 1)) = 0.2
        _Metallic ("Metallic", Range(0, 1)) = 0.0
        _Alpha ("Alpha", Range(0, 1)) = 0.8
    }
    
    SubShader
    {
        Tags { "RenderType"="Transparent" "RenderPipeline"="UniversalRenderPipeline" "Queue"="Transparent" }
        LOD 200
        
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }
            
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float2 uv : TEXCOORD0;
                float4 color : COLOR; // Vertex colors for deformation
            };
            
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float3 tangentWS : TEXCOORD2;
                float3 bitangentWS : TEXCOORD3;
                float2 uv : TEXCOORD4;
                float4 shadowCoord : TEXCOORD5;
                float4 color : COLOR;
            };
            
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            
            TEXTURE2D(_NormalMap);
            SAMPLER(sampler_NormalMap);
            
            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float _Smoothness;
                float _Metallic;
                float _Alpha;
            CBUFFER_END
            
            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;
                
                // Transform positions
                output.positionWS = TransformObjectToWorld(input.positionOS.xyz);
                output.positionCS = TransformWorldToHClip(output.positionWS);
                
                // Transform normals and tangents
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.tangentWS = TransformObjectToWorldDir(input.tangentOS.xyz);
                output.bitangentWS = cross(output.normalWS, output.tangentWS) * input.tangentOS.w;
                
                // Pass through UVs and vertex colors
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                output.color = input.color;
                
                // Shadow coordinates
                output.shadowCoord = TransformWorldToShadowCoord(output.positionWS);
                
                return output;
            }
            
            half4 frag(Varyings input) : SV_Target
            {
                // Sample textures
                half4 mudTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                
                // Simple normal map sampling
                half4 normalSample = SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, input.uv);
                half3 normalMap = half3(normalSample.r * 2 - 1, normalSample.g * 2 - 1, 1);
                
                // Combine texture with vertex colors
                half4 albedo = mudTex * input.color;
                
                // Calculate world normal from normal map
                float3x3 tangentToWorld = float3x3(input.tangentWS, input.bitangentWS, input.normalWS);
                float3 worldNormal = normalize(mul(normalMap, tangentToWorld));
                
                // Lighting setup
                Light mainLight = GetMainLight(input.shadowCoord);
                float3 lightDir = normalize(mainLight.direction);
                float3 viewDir = normalize(GetCameraPositionWS() - input.positionWS);
                
                // Basic Lambert diffuse
                float NdotL = saturate(dot(worldNormal, lightDir));
                float3 diffuse = albedo.rgb * mainLight.color * NdotL;
                
                // Simple specular
                float3 halfVector = normalize(lightDir + viewDir);
                float NdotH = saturate(dot(worldNormal, halfVector));
                float specularPower = lerp(1, 256, _Smoothness);
                float3 specular = pow(NdotH, specularPower) * _Metallic * mainLight.color;
                
                // Ambient lighting
                float3 ambient = SampleSH(worldNormal) * albedo.rgb * 0.3;
                
                // Combine lighting
                float shadow = mainLight.shadowAttenuation;
                float3 finalColor = ambient + (diffuse + specular) * shadow;
                
                // Use vertex color alpha for transparency (more deformation = more visible)
                float alpha = input.color.a * _Alpha;
                
                return half4(finalColor, alpha);
            }
            ENDHLSL
        }
    }
    
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}