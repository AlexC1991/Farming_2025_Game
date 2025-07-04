Shader "Custom/EfficientGrass"
{
    Properties
    {
        _MainTex ("Grass Texture", 2D) = "white" {}
        _Color ("Grass Color", Color) = (0.3, 0.8, 0.2, 1)
        _TipColor ("Tip Color", Color) = (0.8, 1, 0.3, 1)
        _WindSpeed ("Wind Speed", Range(0, 10)) = 1
        _WindStrength ("Wind Strength", Range(0, 1)) = 0.3
        _SwayAmount ("Sway Amount", Range(0, 1)) = 0.5
        _BendAmount ("Bend Amount", Range(0, 1)) = 0.3
        _GrassHeight ("Grass Height", Range(0.1, 2)) = 1
        _GrassWidth ("Grass Width", Range(0.01, 0.2)) = 0.05
        _CullDistance ("Cull Distance", Range(10, 200)) = 100
        _FadeDistance ("Fade Distance", Range(5, 50)) = 20
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" "Queue"="Geometry" }
        LOD 200
        Cull Off
        
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #pragma target 3.0
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float fadeAmount : TEXCOORD2;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };
            
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            
            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _Color;
                float4 _TipColor;
                float _WindSpeed;
                float _WindStrength;
                float _SwayAmount;
                float _BendAmount;
                float _GrassHeight;
                float _GrassWidth;
                float _CullDistance;
                float _FadeDistance;
            CBUFFER_END
            
            // Simple noise function for wind variation
            float noise(float2 pos)
            {
                return frac(sin(dot(pos, float2(12.9898, 78.233))) * 43758.5453);
            }
            
            // Wind calculation
            float2 windOffset(float3 worldPos, float height)
            {
                float time = _Time.y * _WindSpeed;
                float2 wind = float2(
                    sin(time + worldPos.x * 0.1) * cos(time * 0.7 + worldPos.z * 0.1),
                    cos(time * 0.8 + worldPos.x * 0.1) * sin(time * 0.6 + worldPos.z * 0.1)
                );
                
                // Add some noise for variation
                float2 noiseOffset = float2(
                    noise(worldPos.xz + time * 0.1),
                    noise(worldPos.xz + time * 0.1 + 10.0)
                ) * 2.0 - 1.0;
                
                wind += noiseOffset * 0.3;
                
                // Apply wind strength and height-based bending
                return wind * _WindStrength * height * height * _SwayAmount;
            }
            
            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                
                // Get world position
                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
                
                // Calculate distance-based culling and fading
                float distToCamera = distance(positionWS, GetCameraPositionWS());
                
                // Cull distant grass
                if (distToCamera > _CullDistance)
                {
                    output.positionCS = float4(0, 0, 0, 0);
                    return output;
                }
                
                // Calculate fade amount for smooth LOD transition
                float fadeStart = _CullDistance - _FadeDistance;
                output.fadeAmount = 1.0 - saturate((distToCamera - fadeStart) / _FadeDistance);
                
                // Scale grass based on UV.y (0 = bottom, 1 = top)
                float heightFactor = input.uv.y;
                
                // Apply wind displacement to top vertices
                float2 windDisp = windOffset(positionWS, heightFactor);
                
                // Apply wind to vertex position
                input.positionOS.xz += windDisp * heightFactor;
                
                // Apply bending effect
                float bend = _BendAmount * heightFactor * heightFactor;
                input.positionOS.y += bend * sin(windDisp.x + windDisp.y);
                
                // Scale the grass blade
                input.positionOS.xz *= _GrassWidth;
                input.positionOS.y *= _GrassHeight;
                
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                output.positionWS = positionWS;
                
                return output;
            }
            
            half4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                
                // Sample texture
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                
                // Lerp between base color and tip color based on UV.y
                half4 grassColor = lerp(_Color, _TipColor, input.uv.y);
                col *= grassColor;
                
                // Apply fade for distance-based LOD
                col.a *= input.fadeAmount;
                
                // Simple ambient lighting
                half3 ambient = SampleSH(half3(0, 1, 0));
                col.rgb *= ambient;
                
                // Alpha test for grass blade shape
                clip(col.a - 0.1);
                
                return col;
            }
            ENDHLSL
        }
        
        // Shadow pass for proper shadow casting
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }
            
            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull Off
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #pragma target 3.0
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            
            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float _GrassHeight;
                float _GrassWidth;
            CBUFFER_END
            
            float3 _LightDirection;
            
            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                
                // Apply same transformations as main pass
                input.positionOS.xz *= _GrassWidth;
                input.positionOS.y *= _GrassHeight;
                
                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(input.normalOS);
                
                output.positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, _LightDirection));
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                
                return output;
            }
            
            half4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                clip(col.a - 0.1);
                return 0;
            }
            ENDHLSL
        }
    }
    
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}