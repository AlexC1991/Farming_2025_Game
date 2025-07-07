Shader "FogShader/Volume_Fog_Shader"
{
    Properties
    {
        _Color("Fog Color", Color) = (1, 1, 1, 0.1)
        _Density("Density", Range(0, 1)) = 0.1
        _NoiseScale("Noise Scale", Range(0.1, 10)) = 1
        _WindSpeed("Wind Speed", Range(0, 5)) = 1
        _DepthFade("Depth Fade", Range(0, 10)) = 1
        _FresnelPower("Fresnel Power", Range(0, 5)) = 2
        _NightEdgeSoftening("Night Edge Softening", Range(1, 3)) = 1.5
    }

    SubShader
    {
        Tags { 
            "RenderType" = "Transparent" 
            "RenderPipeline" = "UniversalPipeline" 
            "Queue" = "Transparent" 
        }
        
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            Name "SimpleFogPass"
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float4 screenPos : TEXCOORD2;
                float3 viewDir : TEXCOORD3;
                float2 uv : TEXCOORD4;
            };

            float4 _Color;
            float _Density;
            float _NoiseScale;
            float _WindSpeed;
            float _DepthFade;
            float _FresnelPower;
            float _NightEdgeSoftening;

            // Simple 3D noise function
            float noise3D(float3 pos)
            {
                return frac(sin(dot(pos, float3(12.9898, 78.233, 45.164))) * 43758.5453);
            }

            // Improved noise with multiple octaves
            float fbm(float3 pos)
            {
                float value = 0.0;
                float amplitude = 0.5;
                float frequency = 1.0;
                
                for(int i = 0; i < 4; i++)
                {
                    value += amplitude * (noise3D(pos * frequency) * 2.0 - 1.0);
                    amplitude *= 0.5;
                    frequency *= 2.0;
                }
                return value * 0.5 + 0.5;
            }

            Varyings vert(Attributes input)
            {
                Varyings output;
                
                VertexPositionInputs positionInputs = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInputs = GetVertexNormalInputs(input.normalOS);
                
                output.positionCS = positionInputs.positionCS;
                output.positionWS = positionInputs.positionWS;
                output.normalWS = normalInputs.normalWS;
                output.screenPos = ComputeScreenPos(positionInputs.positionCS);
                output.viewDir = GetWorldSpaceViewDir(positionInputs.positionWS);
                output.uv = input.uv;
                
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                // Get screen space UV for depth sampling
                float2 screenUV = input.screenPos.xy / input.screenPos.w;
                
                // Sample scene depth for soft particles
                float sceneDepth = SampleSceneDepth(screenUV);
                float3 worldPosFromDepth = ComputeWorldSpacePosition(screenUV, sceneDepth, UNITY_MATRIX_I_VP);
                float depthDistance = length(worldPosFromDepth - _WorldSpaceCameraPos);
                float objectDepth = length(input.positionWS - _WorldSpaceCameraPos);
                
                // Soft particle factor
                float depthFade = saturate((depthDistance - objectDepth) / _DepthFade);
                
                // Get light intensity for night detection
                Light mainLight = GetMainLight();
                float lightIntensity = dot(mainLight.color, float3(0.299, 0.587, 0.114));
                
                // Slightly softer fresnel at night - SUBTLE change only
                float fresnelPower = lerp(_FresnelPower * _NightEdgeSoftening, _FresnelPower, lightIntensity);
                
                // Fresnel effect for edge fading
                float3 viewDir = normalize(input.viewDir);
                float3 normal = normalize(input.normalWS);
                float fresnel = 1.0 - saturate(dot(viewDir, normal));
                fresnel = pow(fresnel, fresnelPower);
                
                // Animated noise
                float3 noisePos = input.positionWS * _NoiseScale + float3(_Time.y * _WindSpeed, 0, _Time.y * _WindSpeed * 0.5);
                float noise = fbm(noisePos);
                
                // Distance-based density falloff
                float distance = length(input.positionWS - _WorldSpaceCameraPos);
                float distanceFade = 1.0 / (1.0 + distance * 0.01);
                
                // Combine all factors - EXACTLY like the original
                float alpha = _Color.a * _Density * noise * fresnel * depthFade * distanceFade;
                alpha = saturate(alpha);
                
                return float4(_Color.rgb, alpha);
            }
            ENDHLSL
        }
    }
    
    Fallback "Hidden/Universal Render Pipeline/FallbackError"
}