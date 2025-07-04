// Made with Amplify Shader Editor v1.9.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Skybox/SkyHDR"
{
	Properties
	{
		[HDR][Header(Sky Color Settings)]_DayColorTop("Day Color Top", Color) = (0.3455882,0.8104462,1,0)
		[HDR]_DayColorBottom("Day Color Bottom", Color) = (0.1544118,0.5451319,1,0)
		[HDR]_NightColorTop("Night Color Top", Color) = (0,0.1359026,0.2941176,0)
		[HDR]_NightColorBottom("Night Color Bottom", Color) = (0.0537526,0,0.1470588,0)
		[HDR]_HorizonColor("Horizon Color", Color) = (1,0.2705882,0.1686275,0)
		_SkyHorizonGradient("Sky Horizon Gradient", Range( 0.01 , 2)) = 1
		_HorizonHeight("Horizon Height", Range( 0 , 1)) = 2.04
		_HorizonContrast("Horizon Contrast", Range( 0 , 1)) = 2.04
		[HDR]_HorizontLineColor("Horizont Line Color", Color) = (0.9622642,0.9622642,0.9622642,0)
		_HorizontLinePower("Horizont Line Power", Range( -5 , 200)) = 124.398
		_HorizontLineOpacity("Horizont Line Opacity", Range( 0 , 1)) = 1
		[Header(Clouds Settings)]_CloudCoverage1("Cloud Coverage", Range( 0 , 1)) = 0
		_CloudBrightness("Cloud Brightness", Range( 0 , 0.999999)) = 0.4486982
		_CloudContrast("Cloud Contrast", Range( 0 , 100)) = 27.57542
		_CloudMaskingSunMoon("Cloud Masking Sun/Moon", Range( 0 , 5)) = 0.85
		[NoScaleOffset][Header(Clouds Visual Settings)]_CloudMain("Cloud Main", 2D) = "white" {}
		_CloudSize("Cloud Size", Range( 0 , 1)) = 1
		_CloudSpeed("Cloud Speed", Float) = 0.2
		[NoScaleOffset]_CloudEdgeMask("Cloud Edge Mask", 2D) = "white" {}
		_CloudEdgeOpacity("Cloud Edge Opacity", Range( 0 , 1)) = 0
		_CloudEdgeSize("Cloud Edge Size", Range( 0 , 4)) = 0
		_CloudEdgeSpeed("Cloud Edge Speed", Float) = 0.2
		_CloudsHorizonGradient("Clouds Horizon Gradient", Range( 0 , 25)) = 1
		_CloudsHorizonHeight("Clouds Horizon Height", Range( 0 , 3)) = 0
		[HDR][Header(Clouds Color Settings)]_CloudDayColor("Cloud Day Color", Color) = (1,1,1,0)
		[HDR]_CloudDayColorBottom("Cloud Day Color Bottom", Color) = (0,0,0,0)
		[HDR]_CloudNightColor("Cloud Night Color", Color) = (1,1,1,0)
		[HDR]_CloudNightColorBottom("Cloud Night Color Bottom", Color) = (0,0,0,0)
		_CloudBottomContrastDay1("Cloud Bottom Contrast Day", Range( 0 , 10)) = 0
		_CloudBottomContrastNight1("Cloud Bottom Contrast Night", Range( 0 , 10)) = 0
		_CloudBottomContrastSunset1("Cloud Bottom Contrast Sunset", Range( 0 , 10)) = 0
		[NoScaleOffset]_CloudBottomMask("Cloud Bottom Mask", 2D) = "white" {}
		_CloudBottomScale("Cloud Bottom Scale", Range( -2 , 2)) = 0
		_CloudBottomOpacity("Cloud Bottom Opacity", Range( 0 , 1)) = 0
		_CloudBottomBias("Cloud Bottom Bias", Range( -5 , 5)) = 0
		_CloudBottomSize("Cloud Bottom Size", Range( 0 , 4)) = 0
		[Header(Clouds Background)]_BackgroundPower("Background Power", Range( 0.1 , 1)) = 0
		_BackgroundSize("Background Size", Range( 0 , 4)) = 1
		_BackgroundSpeed("Background Speed", Float) = 0.2
		_Background("Background ", 2D) = "white" {}
		[HDR]_DayColorBackground("Day Color Background", Color) = (0,0.1359026,0.2941176,0)
		[HDR]_NightColorBackground("Night Color Background", Color) = (0.0537526,0,0.1470588,0)
		_BackgroundOpacityNight("Background Opacity Night", Range( 0 , 2)) = 0
		_BackgroundOpacityDay("Background Opacity Day", Range( 0 , 2)) = 0
		[HDR]_SunsetColorBackground("Sunset Color Background ", Color) = (1,0.2705882,0.1686275,0)
		[HDR][Header(Sun Settings)]_SunDayColor("Sun Day Color", Color) = (1,1,1,0)
		[HDR]_SunSunsetColor("Sun Sunset Color", Color) = (1,0.8823529,0.8823529,0)
		[HDR]_CloudSunsetColor("Cloud Sunset Color", Color) = (1,0.8382353,0.8382353,0)
		[HDR]_CloudSunsetColorBottom("Cloud Sunset Color Bottom", Color) = (0,0,0,0)
		_SunHaloRadius("Sun Halo Radius", Range( 0 , 1)) = 0.11
		[HDR]_SunsetHorizonColor("Sunset Horizon Color", Color) = (1,0.2705882,0.1686275,0)
		_SunHaloSize("Sun Halo Size", Range( 0 , 1)) = 0
		_SunRadius("Sun Radius", Range( 0 , 1)) = 0.11
		_SunScatteringRadius("Sun Scattering Radius", Range( 0 , 1)) = 0
		_Vector5("Vector 5", Vector) = (0.2,0.2,0.2,0)
		_SunScattering("Sun Scattering", Range( 0 , 4)) = 0
		_SunsetHorizonHeight("Sunset Horizon Height", Range( 1 , 10)) = 2.04
		_SunsetSize1("Sunset Size", Range( 0 , 5)) = 3.14
		[Header(Moon Settings)]_MoonDayColor("Moon Day Color", Color) = (0,0,0,0)
		[HDR]_MoonNightColor("Moon Night Color", Color) = (0,0,0,0)
		_MoonRotationAxis2("Moon Rotation Axis", Vector) = (0.85,1,0,0)
		[NoScaleOffset]_Moon("Moon", 2D) = "black" {}
		_MoonScattering("Moon Scattering", Range( 0 , 4)) = 0
		_MoonScatteringRadius("Moon Scattering Radius", Range( 0 , 2)) = 0
		_MoonSize("Moon Size", Range( 0 , 30)) = 1
		[HDR][Header(Star Settings)]_StarsColor("StarsColor", Color) = (1,1,1,0)
		_Star("Star", 2D) = "white" {}
		_NightSkyBrightness("Night Sky Brightness", Range( 0 , 10)) = 0.22
		_StarsHorizonFade("Stars Horizon Fade", Range( 0 , 1)) = 1
		_StarsBlinkBrightness("Stars Blink Brightness", Range( 0 , 10)) = 0
		_StarsBlinkSpeed("Stars Blink Speed", Float) = 0.2
		[NoScaleOffset]_StarsBlinkNoise("Stars Blink Noise", 2D) = "white" {}
		_StarsBlinkNoiseSize("Stars Blink Noise Size", Float) = 1
		_StarSize1("Star Size", Range( 0 , 1)) = 1
		[Header(Other Settings)][Toggle]_AutoLightDirection("Auto Light Direction", Float) = 0
		_ManualLightDirection("Manual Light Direction", Vector) = (0,0,0,0)
		[Enum(UnityEngine.Rendering.CullMode)]_CullMode("Cull Mode", Float) = 2
		[Toggle]_InvertWorld("Invert World", Float) = 0
		[Toggle]_MirrorWorld("Mirror World", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Background+0" "IsEmissive" = "true"  }
		Cull [_CullMode]
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 4.6
		#pragma surface surf Unlit keepalpha noshadow nofog 
		struct Input
		{
			float3 worldPos;
		};

		uniform float _InvertWorld;
		uniform float _MirrorWorld;
		uniform float _AutoLightDirection;
		uniform float3 _ManualLightDirection;
		uniform float _SunHaloRadius;
		uniform float _SunHaloSize;
		uniform float _SunRadius;
		uniform float _CloudContrast;
		uniform sampler2D _CloudMain;
		uniform float _CloudSpeed;
		uniform float _CloudSize;
		uniform sampler2D _CloudEdgeMask;
		uniform float _CloudEdgeSpeed;
		uniform float _CloudEdgeSize;
		uniform float _CloudEdgeOpacity;
		uniform float _CloudCoverage1;
		uniform float _CloudBrightness;
		uniform float _CloudMaskingSunMoon;
		uniform float _CloudsHorizonGradient;
		uniform float _CloudsHorizonHeight;
		uniform float4 _SunDayColor;
		uniform float4 _SunSunsetColor;
		uniform float _SunsetHorizonHeight;
		uniform float _SunsetSize1;
		uniform float4 _NightColorBottom;
		uniform float4 _NightColorTop;
		uniform float _SkyHorizonGradient;
		uniform float4 _DayColorTop;
		uniform float4 _DayColorBottom;
		uniform float _HorizontLinePower;
		uniform float _HorizontLineOpacity;
		uniform float4 _HorizontLineColor;
		uniform float4 _SunsetHorizonColor;
		uniform sampler2D _Star;
		uniform float _StarSize1;
		uniform float4 _StarsColor;
		uniform float _StarsHorizonFade;
		uniform sampler2D _StarsBlinkNoise;
		uniform float _StarsBlinkSpeed;
		uniform float _StarsBlinkNoiseSize;
		uniform float _StarsBlinkBrightness;
		uniform float _NightSkyBrightness;
		uniform float4 _HorizonColor;
		uniform float _HorizonHeight;
		uniform float _HorizonContrast;
		uniform sampler2D _Moon;
		uniform float3 _MoonRotationAxis2;
		uniform float3 _Vector5;
		uniform float _MoonSize;
		uniform float4 _MoonNightColor;
		uniform float4 _MoonDayColor;
		uniform float4 _NightColorBackground;
		uniform float4 _DayColorBackground;
		uniform float4 _SunsetColorBackground;
		uniform sampler2D _Background;
		uniform float _BackgroundSpeed;
		uniform float _BackgroundSize;
		uniform float _BackgroundOpacityNight;
		uniform float _BackgroundOpacityDay;
		uniform float _BackgroundPower;
		uniform float _SunScatteringRadius;
		uniform float _SunScattering;
		uniform float4 _CloudNightColor;
		uniform float4 _CloudDayColor;
		uniform float4 _CloudSunsetColor;
		uniform float4 _CloudNightColorBottom;
		uniform float4 _CloudDayColorBottom;
		uniform float4 _CloudSunsetColorBottom;
		uniform float _CloudBottomContrastNight1;
		uniform float _CloudBottomContrastDay1;
		uniform float _CloudBottomContrastSunset1;
		uniform float _CloudBottomBias;
		uniform float _CloudBottomScale;
		uniform sampler2D _CloudBottomMask;
		uniform float _CloudBottomSize;
		uniform float _CloudBottomOpacity;
		uniform float _MoonScatteringRadius;
		uniform float _MoonScattering;
		uniform float _CullMode;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 appendResult901 = (float3(_WorldSpaceCameraPos.x , 0.0 , _WorldSpaceCameraPos.z));
			float3 break899 = ( ase_worldPos - appendResult901 );
			float2 appendResult581 = (float2(break899.x , break899.z));
			float2 break582 = (( _InvertWorld )?( -appendResult581 ):( appendResult581 ));
			float3 appendResult573 = (float3(break582.x , (( _MirrorWorld )?( abs( break899.y ) ):( break899.y )) , break582.y));
			float3 normalizeResult253 = normalize( appendResult573 );
			float3 WorldPositionNormalised254 = normalizeResult253;
			float3 normalizeResult664 = normalize( _ManualLightDirection );
			float3 ifLocalVar662 = 0;
			if( distance( _ManualLightDirection , float3( 0,0,0 ) ) == 0.0 )
				ifLocalVar662 = float3(0,1,0);
			else
				ifLocalVar662 = normalizeResult664;
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 LightDirection622 = (( _AutoLightDirection )?( ase_worldlightDir ):( ifLocalVar662 ));
			float SunMask262 = distance( WorldPositionNormalised254 , LightDirection622 );
			float lerpResult684 = lerp( _SunHaloSize , 0.0 , 0.0);
			float smoothstepResult681 = smoothstep( 0.0 , 1.0 , saturate( ( saturate( (lerpResult684 + (( SunMask262 / _SunHaloRadius ) - 0.0) * (0.0 - lerpResult684) / (1.0 - 0.0)) ) + saturate( (( _SunRadius * 250.0 ) + (( SunMask262 / _SunRadius ) - 0.0) * (0.0 - ( _SunRadius * 250.0 )) / (1.0 - 0.0)) ) ) ));
			float temp_output_13_0_g1892 = _CloudContrast;
			float2 temp_cast_0 = (_CloudSpeed).xx;
			float3 break99 = WorldPositionNormalised254;
			float2 appendResult102 = (float2(break99.x , break99.z));
			float ifLocalVar101 = 0;
			if( break99.y == 0.0 )
				ifLocalVar101 = 1E-05;
			else
				ifLocalVar101 = break99.y;
			float2 CloudsUV107 = ( appendResult102 / ifLocalVar101 );
			float2 panner2378 = ( 0.02 * _Time.y * temp_cast_0 + CloudsUV107);
			float4 tex2DNode2245 = tex2D( _CloudMain, ( panner2378 * _CloudSize ) );
			float2 temp_cast_1 = (_CloudEdgeSpeed).xx;
			float2 panner2237 = ( 0.02 * _Time.y * temp_cast_1 + CloudsUV107);
			float lerpResult2243 = lerp( tex2DNode2245.r , tex2D( _CloudEdgeMask, ( panner2237 * _CloudEdgeSize ) ).r , _CloudEdgeOpacity);
			float temp_output_5_0_g1892 = ( lerpResult2243 + (-1.5 + (_CloudCoverage1 - 0.0) * (0.5 - -1.5) / (1.0 - 0.0)) );
			float temp_output_45_0_g1892 = ( ( tan( ( ( ( saturate( temp_output_5_0_g1892 ) - 0.5 ) * UNITY_PI ) / ( ( 0.5 * UNITY_PI ) / atan( ( temp_output_13_0_g1892 / 2.0 ) ) ) ) ) / temp_output_13_0_g1892 ) + 0.5 );
			float ifLocalVar46_g1892 = 0;
			if( temp_output_13_0_g1892 == 0.0 )
				ifLocalVar46_g1892 = temp_output_5_0_g1892;
			else
				ifLocalVar46_g1892 = temp_output_45_0_g1892;
			float CloudMask1736 = (0.0 + (ifLocalVar46_g1892 - 0.0) * (1.0 - 0.0) / (( 1.0 - _CloudBrightness ) - 0.0));
			float CloudMasking269 = _CloudMaskingSunMoon;
			float SkyMask291 = saturate( ( ( (WorldPositionNormalised254).y * _CloudsHorizonGradient ) - _CloudsHorizonHeight ) );
			float lerpResult278 = lerp( smoothstepResult681 , 0.0 , saturate( ( CloudMask1736 * CloudMasking269 * SkyMask291 ) ));
			float SunsetMask2106 = ( ( 1.0 - saturate( ( abs( (WorldPositionNormalised254).y ) * _SunsetHorizonHeight ) ) ) * ( 1.0 - saturate( ( ( SunMask262 / ( 1.0 - abs( (LightDirection622).y ) ) ) / _SunsetSize1 ) ) ) * saturate( ( 1.0 - abs( ( ( (LightDirection622).y * 3.0 ) + -0.4 ) ) ) ) );
			float4 lerpResult277 = lerp( _SunDayColor , _SunSunsetColor , SunsetMask2106);
			float4 Sun280 = ( lerpResult278 * lerpResult277 );
			float smoothstepResult1121 = smoothstep( -0.015 , 0.009 , (WorldPositionNormalised254).y);
			float smoothstepResult2067 = smoothstep( -0.15 , _SkyHorizonGradient , (WorldPositionNormalised254).y);
			float4 lerpResult2055 = lerp( _NightColorBottom , _NightColorTop , smoothstepResult2067);
			float temp_output_2059_0 = ( 1.0 - smoothstepResult2067 );
			float4 lerpResult2054 = lerp( _DayColorTop , _DayColorBottom , temp_output_2059_0);
			float SkyGradientMask2060 = temp_output_2059_0;
			float4 temp_cast_2 = (SkyGradientMask2060).xxxx;
			float4 lerpResult2064 = lerp( lerpResult2054 , temp_cast_2 , float4( 0,0,0,0 ));
			float dotResult2480 = dot( WorldPositionNormalised254 , float3( 0,1,0 ) );
			float smoothstepResult2051 = smoothstep( 0.0 , 1.0 , saturate( ( ( (LightDirection622).y + 0.2 ) * 1.2 ) ));
			float DaynightProgress2062 = smoothstepResult2051;
			float4 lerpResult250 = lerp( lerpResult2055 , ( lerpResult2064 + ( saturate( pow( ( 1.0 - abs( dotResult2480 ) ) , _HorizontLinePower ) ) * _HorizontLineOpacity * _HorizontLineColor ) ) , DaynightProgress2062);
			float4 lerpResult227 = lerp( lerpResult250 , _SunsetHorizonColor , SunsetMask2106);
			float4 SkyColor228 = lerpResult227;
			float4 tex2DNode1690 = tex2D( _Star, ( CloudsUV107 * _StarSize1 ) );
			float4 StarsColor514 = tex2DNode1690;
			float2 temp_cast_3 = (_StarsBlinkSpeed).xx;
			float2 panner921 = ( 1.0 * _Time.y * temp_cast_3 + ( (WorldPositionNormalised254).xz / pow( (WorldPositionNormalised254).y , 0.1 ) ));
			float clampResult936 = clamp( ( ( (-1.0 + (tex2D( _StarsBlinkNoise, ( panner921 * _StarsBlinkNoiseSize ) ).b - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) * _StarsBlinkBrightness ) + _NightSkyBrightness ) , 0.0 , 100.0 );
			float smoothstepResult1110 = smoothstep( 0.0 , 0.0 , (WorldPositionNormalised254).y);
			float StarsMask513 = ( ( saturate( ( (LightDirection622).y * -5.0 ) ) * saturate( ( ( tex2DNode1690.r + tex2DNode1690.g + tex2DNode1690.b ) / 3.0 ) ) * ( 1.0 - saturate( ( CloudMask1736 * SkyMask291 * CloudMasking269 ) ) ) * saturate( ( ( 1.0 - _StarsHorizonFade ) * SkyMask291 ) ) * clampResult936 ) * smoothstepResult1110 );
			float4 lerpResult519 = lerp( SkyColor228 , ( ( StarsColor514 * _StarsColor ) + _StarsColor ) , ( _StarsColor.a * StarsMask513 ));
			float smoothstepResult1068 = smoothstep( _HorizonHeight , -1.0 , abs( saturate( (WorldPositionNormalised254).y ) ));
			float4 Horizont1167 = ( _HorizonColor * smoothstepResult1068 * _HorizonContrast );
			float3 normalizeResult3307 = normalize( _MoonRotationAxis2 );
			float3 temp_output_1_0_g1898 = normalizeResult3307;
			float3 temp_output_2_0_g1898 = _Vector5;
			float dotResult3_g1898 = dot( temp_output_1_0_g1898 , temp_output_2_0_g1898 );
			float3 break19_g1898 = cross( temp_output_1_0_g1898 , temp_output_2_0_g1898 );
			float4 appendResult23_g1898 = (float4(break19_g1898.x , break19_g1898.y , break19_g1898.z , ( dotResult3_g1898 + 1.0 )));
			float4 normalizeResult24_g1898 = normalize( appendResult23_g1898 );
			float4 ifLocalVar25_g1898 = 0;
			if( dotResult3_g1898 <= 0.999999 )
				ifLocalVar25_g1898 = normalizeResult24_g1898;
			else
				ifLocalVar25_g1898 = float4(0,0,0,1);
			float temp_output_4_0_g1899 = ( UNITY_PI / 2.0 );
			float3 temp_output_8_0_g1898 = cross( float3(1,0,0) , temp_output_1_0_g1898 );
			float3 ifLocalVar10_g1898 = 0;
			if( length( temp_output_8_0_g1898 ) >= 1E-06 )
				ifLocalVar10_g1898 = temp_output_8_0_g1898;
			else
				ifLocalVar10_g1898 = cross( float3(0,1,0) , temp_output_1_0_g1898 );
			float3 normalizeResult13_g1898 = normalize( ifLocalVar10_g1898 );
			float3 break10_g1899 = ( sin( temp_output_4_0_g1899 ) * normalizeResult13_g1898 );
			float4 appendResult8_g1899 = (float4(break10_g1899.x , break10_g1899.y , break10_g1899.z , cos( temp_output_4_0_g1899 )));
			float4 ifLocalVar4_g1898 = 0;
			if( dotResult3_g1898 >= -0.999999 )
				ifLocalVar4_g1898 = ifLocalVar25_g1898;
			else
				ifLocalVar4_g1898 = appendResult8_g1899;
			float4 temp_output_2_0_g1900 = ifLocalVar4_g1898;
			float4 temp_output_1_0_g1901 = temp_output_2_0_g1900;
			float3 temp_output_7_0_g1901 = (temp_output_1_0_g1901).xyz;
			float3 temp_cast_4 = (90.0).xxx;
			float temp_output_3319_0 = distance( temp_cast_4 , LightDirection622 );
			float temp_output_4_0_g1894 = ( ( radians( temp_output_3319_0 ) + temp_output_3319_0 ) / 2.0 );
			float3 break10_g1894 = ( sin( temp_output_4_0_g1894 ) * normalizeResult3307 );
			float4 appendResult8_g1894 = (float4(break10_g1894.x , break10_g1894.y , break10_g1894.z , cos( temp_output_4_0_g1894 )));
			float4 temp_output_2_0_g1895 = appendResult8_g1894;
			float4 temp_output_1_0_g1896 = temp_output_2_0_g1895;
			float3 temp_output_7_0_g1896 = (temp_output_1_0_g1896).xyz;
			float3 break8_g1895 = WorldPositionNormalised254;
			float4 appendResult9_g1895 = (float4(break8_g1895.x , break8_g1895.y , break8_g1895.z , 0.0));
			float4 temp_output_1_0_g1897 = appendResult9_g1895;
			float3 temp_output_7_0_g1897 = (temp_output_1_0_g1897).xyz;
			float4 temp_output_2_0_g1897 = ( temp_output_2_0_g1895 * float4(-1,-1,-1,1) );
			float temp_output_10_0_g1897 = (temp_output_2_0_g1897).w;
			float3 temp_output_3_0_g1897 = (temp_output_2_0_g1897).xyz;
			float temp_output_11_0_g1897 = (temp_output_1_0_g1897).w;
			float3 break17_g1897 = ( ( temp_output_7_0_g1897 * temp_output_10_0_g1897 ) + cross( temp_output_1_0_g1897.xyz , temp_output_2_0_g1897.xyz ) + ( temp_output_3_0_g1897 * temp_output_11_0_g1897 ) );
			float dotResult16_g1897 = dot( temp_output_7_0_g1897 , temp_output_3_0_g1897 );
			float4 appendResult18_g1897 = (float4(break17_g1897.x , break17_g1897.y , break17_g1897.z , ( ( temp_output_11_0_g1897 * temp_output_10_0_g1897 ) - dotResult16_g1897 )));
			float4 temp_output_2_0_g1896 = appendResult18_g1897;
			float temp_output_10_0_g1896 = (temp_output_2_0_g1896).w;
			float3 temp_output_3_0_g1896 = (temp_output_2_0_g1896).xyz;
			float temp_output_11_0_g1896 = (temp_output_1_0_g1896).w;
			float3 break17_g1896 = ( ( temp_output_7_0_g1896 * temp_output_10_0_g1896 ) + cross( temp_output_1_0_g1896.xyz , temp_output_2_0_g1896.xyz ) + ( temp_output_3_0_g1896 * temp_output_11_0_g1896 ) );
			float dotResult16_g1896 = dot( temp_output_7_0_g1896 , temp_output_3_0_g1896 );
			float4 appendResult18_g1896 = (float4(break17_g1896.x , break17_g1896.y , break17_g1896.z , ( ( temp_output_11_0_g1896 * temp_output_10_0_g1896 ) - dotResult16_g1896 )));
			float3 break8_g1900 = (appendResult18_g1896).xyz;
			float4 appendResult9_g1900 = (float4(break8_g1900.x , break8_g1900.y , break8_g1900.z , 0.0));
			float4 temp_output_1_0_g1902 = appendResult9_g1900;
			float3 temp_output_7_0_g1902 = (temp_output_1_0_g1902).xyz;
			float4 temp_output_2_0_g1902 = ( temp_output_2_0_g1900 * float4(-1,-1,-1,1) );
			float temp_output_10_0_g1902 = (temp_output_2_0_g1902).w;
			float3 temp_output_3_0_g1902 = (temp_output_2_0_g1902).xyz;
			float temp_output_11_0_g1902 = (temp_output_1_0_g1902).w;
			float3 break17_g1902 = ( ( temp_output_7_0_g1902 * temp_output_10_0_g1902 ) + cross( temp_output_1_0_g1902.xyz , temp_output_2_0_g1902.xyz ) + ( temp_output_3_0_g1902 * temp_output_11_0_g1902 ) );
			float dotResult16_g1902 = dot( temp_output_7_0_g1902 , temp_output_3_0_g1902 );
			float4 appendResult18_g1902 = (float4(break17_g1902.x , break17_g1902.y , break17_g1902.z , ( ( temp_output_11_0_g1902 * temp_output_10_0_g1902 ) - dotResult16_g1902 )));
			float4 temp_output_2_0_g1901 = appendResult18_g1902;
			float temp_output_10_0_g1901 = (temp_output_2_0_g1901).w;
			float3 temp_output_3_0_g1901 = (temp_output_2_0_g1901).xyz;
			float temp_output_11_0_g1901 = (temp_output_1_0_g1901).w;
			float3 break17_g1901 = ( ( temp_output_7_0_g1901 * temp_output_10_0_g1901 ) + cross( temp_output_1_0_g1901.xyz , temp_output_2_0_g1901.xyz ) + ( temp_output_3_0_g1901 * temp_output_11_0_g1901 ) );
			float dotResult16_g1901 = dot( temp_output_7_0_g1901 , temp_output_3_0_g1901 );
			float4 appendResult18_g1901 = (float4(break17_g1901.x , break17_g1901.y , break17_g1901.z , ( ( temp_output_11_0_g1901 * temp_output_10_0_g1901 ) - dotResult16_g1901 )));
			float3 temp_output_3302_0 = (appendResult18_g1901).xyz;
			float3 MoonRotation747 = temp_output_3302_0;
			float4 tex2DNode462 = tex2D( _Moon, saturate( ( (( MoonRotation747 * _MoonSize )).xy + 0.5 ) ) );
			float4 lerpResult471 = lerp( _MoonNightColor , _MoonDayColor , DaynightProgress2062);
			float4 MoonLightColor1821 = lerpResult471;
			float4 Moon467 = ( tex2DNode462 * MoonLightColor1821 );
			float MoonSkyMask725 = ( ( ( 1.0 - saturate( (MoonRotation747).z ) ) * tex2DNode462.a ) * (lerpResult471).a );
			float4 lerpResult469 = lerp( ( ( ( Sun280 * smoothstepResult1121 ) + lerpResult519 ) + Horizont1167 ) , Moon467 , MoonSkyMask725);
			float4 lerpResult2440 = lerp( _NightColorBackground , _DayColorBackground , DaynightProgress2062);
			float4 lerpResult2443 = lerp( lerpResult2440 , _SunsetColorBackground , SunsetMask2106);
			float2 temp_cast_13 = (_BackgroundSpeed).xx;
			float2 panner2256 = ( 1.0 * _Time.y * temp_cast_13 + CloudsUV107);
			float lerpResult2470 = lerp( _BackgroundOpacityNight , _BackgroundOpacityDay , DaynightProgress2062);
			float smoothstepResult2123 = smoothstep( 0.0 , 3.0 , (0.0 + (( tex2D( _Background, ( panner2256 * _BackgroundSize ) ).r * lerpResult2470 * SkyMask291 ) - _BackgroundPower) * (1.0 - 0.0) / (1.0 - _BackgroundPower)));
			float BackroundTexture1917 = smoothstepResult2123;
			float4 lerpResult1947 = lerp( lerpResult469 , lerpResult2443 , BackroundTexture1917);
			float smoothstepResult331 = smoothstep( 0.0 , 1.0 , ( ( 1.0 - SunMask262 ) - _SunScatteringRadius ));
			float ScatteringMask362 = ( smoothstepResult331 * _SunScattering );
			float4 lerpResult303 = lerp( _CloudNightColor , _CloudDayColor , DaynightProgress2062);
			float4 lerpResult566 = lerp( lerpResult303 , _CloudSunsetColor , SunsetMask2106);
			float4 lerpResult567 = lerp( _CloudNightColorBottom , _CloudDayColorBottom , DaynightProgress2062);
			float4 lerpResult569 = lerp( lerpResult567 , _CloudSunsetColorBottom , SunsetMask2106);
			float MoonGradient762 = ( 1.0 - (temp_output_3302_0).z );
			float lerpResult3010 = lerp( _CloudBottomContrastNight1 , _CloudBottomContrastDay1 , DaynightProgress2062);
			float lerpResult3011 = lerp( lerpResult3010 , _CloudBottomContrastSunset1 , SunsetMask2106);
			float4 temp_cast_14 = (( ( 1.0 + _CloudBottomBias ) * _CloudBottomScale )).xxxx;
			float Cloud_Speed3209 = _CloudSpeed;
			float2 temp_cast_15 = (Cloud_Speed3209).xx;
			float2 panner3177 = ( 0.02 * _Time.y * temp_cast_15 + CloudsUV107);
			float CloudBottomMask2998 = ( ( ( ( SunMask262 * DaynightProgress2062 ) + ( MoonGradient762 * ( 1.0 - DaynightProgress2062 ) ) ) * ( lerpResult3011 * distance( tex2DNode2245 , temp_cast_14 ) ) ) + ( tex2D( _CloudBottomMask, ( panner3177 * _CloudBottomSize ) ).r * _CloudBottomOpacity ) );
			float4 lerpResult407 = lerp( lerpResult566 , lerpResult569 , CloudBottomMask2998);
			float smoothstepResult492 = smoothstep( 0.0 , 1.0 , ( saturate( ( MoonGradient762 - _MoonScatteringRadius ) ) * _MoonScattering ));
			float MoonScatteringMask486 = smoothstepResult492;
			float4 lerpResult3236 = lerp( ( MoonScatteringMask486 * MoonLightColor1821 ) , float4( 0,0,0,0 ) , DaynightProgress2062);
			float4 CloudsColor405 = ( ( ( ( 1.0 - ( SunsetMask2106 * saturate( SkyMask291 ) ) ) * DaynightProgress2062 ) * ScatteringMask362 ) + lerpResult407 + lerpResult3236 );
			float4 lerpResult309 = lerp( lerpResult1947 , saturate( CloudsColor405 ) , saturate( ( SkyMask291 * CloudMask1736 ) ));
			o.Emission = ( lerpResult309 + ( _CullMode * 0.0 ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
	FallBack "Skybox/Procedural"
    CustomEditor "BioStart.Shaders.SkyBoxShaderHDREditor"
}
/*ASEBEGIN
Version=19200
Node;AmplifyShaderEditor.CommentaryNode;495;-1836.103,5595.678;Inherit;False;2977.248;1160.298;Stars;35;512;510;509;508;609;591;586;505;587;511;605;633;585;604;506;925;632;504;501;503;590;936;1552;1104;1717;1716;1690;514;1109;2506;2507;1718;1105;1110;513;;0.2593305,0.1985294,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;1552;-1840.91,5944.619;Inherit;False;1548.511;335.3179;Comment;10;911;907;913;928;929;921;922;917;920;914;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;577;-3078.087,-162.0927;Inherit;False;2439.877;448.2189;World Position;14;901;254;253;573;578;582;579;574;583;581;899;900;897;252;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;667;-4811.307,6752.066;Inherit;False;1296.304;469.4434;Light Direction;8;618;622;619;662;664;666;663;660;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;106;-2966.608,398.5918;Inherit;False;1469.849;403.1014;Clouds UV;7;107;104;101;102;103;99;576;;0.4779412,1,0.9135903,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;257;-4954.734,8525.09;Inherit;False;3310.459;692.9037;Sun Generation;31;280;279;277;278;275;274;276;273;681;271;272;268;265;690;692;691;270;686;269;706;685;264;684;267;687;266;263;262;261;624;281;;1,0.8482759,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;282;-4911.144,6186.523;Inherit;False;1369.3;515.5005;Skymask;8;291;290;288;285;293;289;287;286;;0.5294118,0.9026368,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;451;-4555.01,7451.297;Inherit;False;2116.419;908.2753;Moon;22;473;472;471;467;463;465;462;461;460;459;457;458;455;456;454;452;453;474;476;725;726;1821;;0.4558824,0.4558824,0.4558824,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;439;-3508.23,6209.401;Inherit;False;1525.489;972.7324;Scattering Masks;16;316;320;322;319;317;488;486;362;327;492;328;326;331;324;325;321;;1,0.9168357,0.6985294,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;438;-1547.249,6906.348;Inherit;False;2734.094;1541.347;Clouds Color;32;565;563;304;564;408;405;414;490;427;487;436;407;491;305;413;435;434;303;298;433;301;299;431;429;430;566;567;568;569;570;3236;3235;;0.75,0.9586207,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;1025;-5046.051,2759.625;Inherit;False;2784.415;623.0728;Horizont color;10;1167;1050;1175;1164;1165;1051;1068;1046;1076;1047;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;2087;-4968.213,5050.404;Inherit;False;2895.711;937.9996;Sunset;30;2119;2118;2117;2113;2112;2111;2110;2109;2108;2106;2105;2104;2103;2102;2101;2100;2099;2098;2097;2096;2095;2094;2093;2092;2091;2090;2089;2088;226;227;;1,0.3931034,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;2219;-3486.804,1807.911;Inherit;False;1548.511;335.3179;Comment;6;2241;2240;2238;2237;2381;2239;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;2218;-3465.282,2232.965;Inherit;False;1548.511;335.3179;Comment;6;2244;2225;2378;2224;2245;3209;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;3175;-727.3914,1830.895;Inherit;False;1548.511;335.3179;Comment;6;3179;3177;3176;3181;3210;3180;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;605;-57.49042,6145.578;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;430;-380.8922,7054.523;Inherit;False;291;SkyMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;609;113.4795,6176.241;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;429;-203.4219,7060.035;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;433;-44.97898,7034.934;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;936;297.5901,6152.877;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;434;93.67651,7053.66;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;487;1.199409,7752.938;Inherit;False;486;MoonScatteringMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;569;-977.6167,8220.226;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;566;-977.6165,7569.43;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;407;-584.3929,7603.069;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;427;398.1272,7325.259;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;490;325.6111,7768.263;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;414;538.5611,7566.888;Inherit;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;920;-1280.91,6056.619;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;917;-1120.91,5992.619;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;922;-1392.91,6152.619;Inherit;False;Property;_StarsBlinkSpeed;Stars Blink Speed;71;0;Create;True;0;0;0;False;0;False;0.2;0.015;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;929;-1040.91,6136.619;Inherit;False;Property;_StarsBlinkNoiseSize;Stars Blink Noise Size;73;0;Create;True;0;0;0;False;0;False;1;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;928;-784.9096,6024.619;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;921;-976.9096,6008.619;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;907;-608.9099,5992.619;Inherit;True;Property;_StarsBlinkNoise;Stars Blink Noise;72;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;cd14613d6b1fe6841890e5c2111473e8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;567;-1119.802,7946.833;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;501;-1007.541,6520.607;Inherit;False;269;CloudMasking;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;586;-771.2153,5828.361;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;633;-789.4318,5692.369;Inherit;False;False;True;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;505;-554.0098,5711.639;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;590;-757.2889,6429.036;Float;False;Property;_StarsHorizonFade;Stars Horizon Fade;69;0;Create;True;0;0;0;False;0;False;1;0.738;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;911;-1472.91,5992.619;Inherit;False;True;False;True;False;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;914;-1488.91,6072.619;Inherit;False;False;True;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;913;-1792.91,6008.619;Inherit;False;254;WorldPositionNormalised;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;510;-394.6403,5795.215;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;508;-402.0685,5701.571;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;925;-275.1361,5954.298;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;509;-440.1161,5865.459;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;491;28.61112,7844.263;Inherit;False;1821;MoonLightColor;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;585;-967.6376,5847.64;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;2047;-4398.512,4702.865;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;2050;-4110.979,4703.187;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;2051;-3829.177,4761.758;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;2052;-4707.256,4663.763;Inherit;False;False;True;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2053;-4254.886,4706.363;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;2054;-3200.653,3785.944;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;2061;-4187.096,4245.36;Float;False;Property;_SkyHorizonGradient;Sky Horizon Gradient;6;0;Create;True;0;0;0;False;0;False;1;0.57;0.01;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;632;-1036.36,5729.792;Inherit;False;622;LightDirection;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;305;-1234.81,7691.628;Inherit;False;2106;SunsetMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceCameraPos;897;-3059.54,73.4781;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;901;-2787.54,99.47806;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldPosInputsNode;252;-2821.476,-80.28104;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.BreakToComponentsNode;582;-1513.154,113.4061;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleSubtractOpNode;900;-2559.54,-47.52188;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;581;-2080.112,115.4087;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ToggleSwitchNode;579;-1738.509,116.0691;Float;False;Property;_InvertWorld;Invert World;78;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NegateNode;583;-1884.156,206.0827;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ToggleSwitchNode;578;-1756.795,-113.5067;Float;True;Property;_MirrorWorld;Mirror World;79;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;570;-1198.517,8330.015;Inherit;False;2106;SunsetMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;301;-1492.642,7490.896;Inherit;False;2062;DaynightProgress;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;303;-1174.183,7291.418;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;2046;-4706.994,4755.321;Float;False;Constant;_VerticalNightTimeShift;Vertical Night Time Shift;16;0;Create;True;0;0;0;False;0;False;0.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;2060;-3094.153,4108.458;Float;False;SkyGradientMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;2057;-4535.327,4193.28;Inherit;True;False;True;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;2485;-4002.629,3783.647;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;2059;-3385.841,4074.066;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;2481;-4701.592,3629.839;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;2055;-3267.45,4464.559;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;2067;-3667.924,4247.393;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;-0.15;False;2;FLOAT;0.02;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;591;-119.0734,6471.261;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;587;-600.1123,5869.972;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;506;-779.2979,6291.276;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;504;-1007.804,6320.954;Inherit;True;291;SkyMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;2505;-476.067,6630.356;Inherit;True;291;SkyMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2506;-321.3284,6513.532;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;2507;-465.0851,6432.278;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;503;-991.9636,6245.591;Inherit;False;1736;CloudMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;408;-1029.782,7766.786;Inherit;False;2998;CloudBottomMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;2066;-4993.07,4199.312;Inherit;False;254;WorldPositionNormalised;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;413;140.9669,7343.926;Inherit;False;362;ScatteringMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;405;847.5042,7662.74;Float;False;CloudsColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;524;-1234.319,4714.844;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;1120;-1257.643,3628.894;Inherit;True;False;True;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;516;-1465.556,4263.141;Inherit;True;514;StarsColor;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;1121;-996.4072,3621.637;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;-0.015;False;2;FLOAT;0.009;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1118;-794.4263,3846.137;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;295;-1435.692,3854.632;Inherit;True;280;Sun;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;1119;-1601.689,3701.281;Inherit;False;254;WorldPositionNormalised;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;297;-558.2927,4070.89;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;470;-266.1344,4455.62;Inherit;True;467;Moon;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;468;-268.4514,4686.47;Inherit;True;725;MoonSkyMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;469;174.6692,4473.493;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;1942;297.9438,4874.319;Inherit;False;1917;BackroundTexture;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;1172;140.0073,4153.728;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;2441;233.9381,3944.813;Inherit;False;2062;DaynightProgress;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;2440;448.2068,3737.019;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;1947;667.6473,4609.338;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;2446;703.0759,4198.178;Inherit;False;2106;SunsetMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;519;-854.3192,4311.844;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;523;-1190.319,4358.844;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;517;-1471.301,4729.532;Inherit;True;513;StarsMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;2492;-1094.837,4479.865;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;1174;-102.912,4282.241;Inherit;False;1167;Horizont;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;309;580.7344,4993.846;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;412;52.90253,5001.437;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;406;-410.8304,4955.46;Inherit;False;405;CloudsColor;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;903;1527.4,4713.238;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2418.802,4643.642;Float;False;True;-1;6;ASEMaterialInspector;0;0;Unlit;Skybox/SkyHDR;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;False;False;False;False;False;Front;0;False;;0;False;;False;0;False;;0;False;;False;0;Custom;0.5;True;False;0;True;Opaque;;Background;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;16.7;10;25;False;0.306;False;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;True;_CullMode;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.LerpOp;2443;1038.167,3966.504;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;514;-1038.432,5617.723;Float;False;StarsColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;296;-518.2499,5317.107;Inherit;False;1736;CloudMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;358;-266.1881,5193.737;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;411;-12.14249,5196.389;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;359;-513.1606,5217.806;Inherit;False;291;SkyMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;2241;-3449.329,1889.432;Inherit;True;107;CloudsUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;2237;-2800,1906;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,1;False;1;FLOAT;0.02;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2381;-2539.245,1903.798;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;431;-377.4652,6948.696;Inherit;False;2106;SunsetMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;436;240,7186;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;435;-32,7202;Inherit;False;2062;DaynightProgress;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;2062;-3477.856,4761.989;Float;False;DaynightProgress;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;3205;1324.161,1969.713;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3206;1128.268,2058.754;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;3010;-164.5094,1215.579;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;10;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;3007;-471.0207,1377.737;Inherit;False;2062;DaynightProgress;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3009;-510.3878,1107.048;Float;False;Property;_CloudBottomContrastDay1;Cloud Bottom Contrast Day;29;0;Create;True;0;0;0;False;0;False;0;1;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;3199;694.39,1177.375;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3195;894.9468,1154.471;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;3201;1121.658,1103.424;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;3011;374.4593,1256.288;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3006;43.09531,1105.301;Float;False;Property;_CloudBottomContrastSunset1;Cloud Bottom Contrast Sunset;31;0;Create;True;0;0;0;False;0;False;0;1.5;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;3194;699.7968,859.451;Inherit;False;262;SunMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;3196;452.6325,1118.248;Inherit;False;2062;DaynightProgress;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;3189;455.9511,1017.98;Inherit;False;762;MoonGradient;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3200;892.5431,1039.303;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3008;-492.2749,1242.77;Float;False;Property;_CloudBottomContrastNight1;Cloud Bottom Contrast Night;30;0;Create;True;0;0;0;False;0;False;0;0.5;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3003;602.5944,1520.847;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;1737;-389.3996,3005.806;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;2248;-659.0381,2656.595;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;1819;-956.3975,2684.434;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1.5;False;4;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;2243;-1468.503,2324.846;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;2997;-641.4851,1524.439;Inherit;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;1738;-391.4026,2817.539;Inherit;False;TgContrast;-1;;1892;b6c9a31911662164faebb1535cfec412;0;2;5;FLOAT;0;False;13;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;1739;-165.35,2837.761;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;576;-2910.668,503.5467;Inherit;True;254;WorldPositionNormalised;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BreakToComponentsNode;99;-2535.982,450.9224;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;103;-2421.818,677.8244;Float;False;Constant;_Float0;Float 0;0;0;Create;True;0;0;0;False;0;False;1E-05;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;101;-2200.396,600.3658;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;102;-2147.49,456.925;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;104;-1950.795,501.353;Inherit;True;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;254;-899.8552,10.46895;Float;True;WorldPositionNormalised;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalizeNode;253;-1139.284,7.44893;Inherit;True;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;573;-1367.71,-10.07426;Inherit;True;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3190;1028.112,1470.29;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;3005;121.4994,1355.177;Inherit;False;2106;SunsetMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;107;-1498.839,545.0825;Float;False;CloudsUV;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;2256;-3642.459,1009.723;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1873;-3403.937,1088.903;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;1936;-3127.84,1237.336;Inherit;False;291;SkyMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1912;-2767.185,1000.024;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;2467;-3124.7,1627.46;Inherit;False;2062;DaynightProgress;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;2470;-2958.417,1445.646;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;2124;-2494.786,1021.462;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;2123;-2192.31,1026.27;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;1917;-1909.412,1104.751;Inherit;False;BackroundTexture;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;1871;-3950.264,847.7845;Inherit;True;107;CloudsUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;1872;-3879.757,1273.749;Inherit;False;Property;_BackgroundSize;Background Size;38;0;Create;True;0;0;0;False;0;False;1;0.324;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1690;-1412.823,5650.987;Inherit;True;Property;_Star;Star;67;0;Create;True;0;0;0;False;0;False;-1;None;6a70ce3f1f8cda54fa03f55dae5e9231;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1716;-1595.847,5669.31;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;1717;-1763.961,5851.589;Inherit;False;Property;_StarSize1;Star Size;74;0;Create;True;0;0;0;False;0;False;1;0.282;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;1718;-1814.64,5648.221;Inherit;True;107;CloudsUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;512;490.9549,5983.136;Inherit;True;5;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;513;934.5889,5995.523;Float;False;StarsMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1109;722.5652,5988.546;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;1104;-80.49944,5693.634;Inherit;False;254;WorldPositionNormalised;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;1105;234.1749,5693.084;Inherit;True;False;True;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;1110;483.2603,5703.733;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;604;-368.4048,6256.409;Float;False;Property;_StarsBlinkBrightness;Stars Blink Brightness;70;0;Create;True;0;0;0;False;0;False;0;10;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;511;-349.823,6336.461;Float;False;Property;_NightSkyBrightness;Night Sky Brightness;68;0;Create;True;0;0;0;False;0;False;0.22;0.38;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;1047;-4602.656,3019.596;Inherit;True;False;True;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;1076;-3106.146,2835.175;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;1046;-3655.538,2811.094;Float;False;Property;_HorizonColor;Horizon Color;5;1;[HDR];Create;True;0;0;0;False;0;False;1,0.2705882,0.1686275,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;1051;-3761.992,3315.555;Float;False;Property;_HorizonHeight;Horizon Height;7;0;Create;True;0;0;0;False;0;False;2.04;0.275;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;1165;-3402.213,2920.437;Float;False;Property;_HorizonContrast;Horizon Contrast;8;0;Create;True;0;0;0;False;0;False;2.04;0.016;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;1175;-4248.217,3065.123;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;1050;-4898.886,3045.65;Inherit;False;254;WorldPositionNormalised;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SmoothstepOpNode;1068;-3440.145,3061.724;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0.17;False;2;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;1164;-3934.912,3056.718;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;1167;-2628.923,2859.308;Float;False;Horizont;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;2068;-4531.897,4883.266;Float;False;Constant;_NightTimeProgressShift;Night Time Progress Shift;17;0;Create;True;0;0;0;False;0;False;1.2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;2063;-4933.559,4667.459;Inherit;False;622;LightDirection;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;2091;-4609.154,5175.551;Inherit;False;254;WorldPositionNormalised;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;2093;-4153.864,5175.393;Inherit;False;False;True;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2096;-4051.989,5280.279;Float;False;Property;_SunsetHorizonHeight;Sunset Horizon Height;57;0;Create;True;0;0;0;False;0;False;2.04;1;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;2097;-3883.581,5174.175;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2098;-4524.711,5621.449;Float;False;Property;_SunsetSize1;Sunset Size;58;0;Create;True;0;0;0;False;0;False;3.14;2.03;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;2099;-3948.181,5536.849;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2100;-3704.365,5207.957;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;2101;-3495.14,5210.697;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;2102;-3817.873,5473.921;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;2103;-3637.205,5464.455;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;2104;-3337.067,5209.248;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2105;-3041.142,5465.255;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;2108;-4260.613,5721.27;Inherit;False;622;LightDirection;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;2109;-4053.594,5803.281;Float;False;Constant;_SunsetHorizonScale1;Sunset Horizon Scale;18;0;Create;True;0;0;0;False;0;False;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;2110;-4032.613,5719.27;Inherit;False;False;True;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2111;-3770.007,5744.605;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2112;-4108.722,5881.482;Float;False;Constant;_SunsetHorizonVerticalOffset1;Sunset Horizon Vertical Offset;18;0;Create;True;0;0;0;False;0;False;-0.4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;2113;-3624.422,5745.582;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;2117;-3501.66,5741.688;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;2118;-3380.689,5747.769;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;2119;-3231.801,5746.158;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;227;-2404.458,5174.906;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;2106;-2790.338,5477.339;Float;False;SunsetMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;2064;-2751.484,3874.343;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;2474;-2557.97,4183.446;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;228;-2079.487,5167.22;Float;True;SkyColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector3Node;660;-4779.825,6856.518;Float;False;Property;_ManualLightDirection;Manual Light Direction;76;0;Create;True;0;0;0;False;0;False;0,0,0;1,20,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DistanceOpNode;663;-4505.556,6814;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;664;-4502.556,6911;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;488;-3393.99,6934.288;Float;False;Property;_MoonScatteringRadius;Moon Scattering Radius;64;0;Create;True;0;0;0;False;0;False;0;1.781;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;317;-3340.327,6696.452;Inherit;True;762;MoonGradient;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;321;-3065.474,6788.769;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;320;-3234.185,6444.823;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;319;-3369.01,6560.347;Float;False;Property;_SunScatteringRadius;Sun Scattering Radius;54;0;Create;True;0;0;0;False;0;False;0;0.289;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;324;-2824.108,6794.764;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;325;-3145.393,7060.784;Float;False;Property;_MoonScattering;Moon Scattering;63;0;Create;True;0;0;0;False;0;False;0;1.26;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;322;-3079.494,6442.981;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;328;-3023.525,6557.185;Float;False;Property;_SunScattering;Sun Scattering;56;0;Create;True;0;0;0;False;0;False;0;0.226;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;326;-2662.737,6798.205;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;492;-2433.408,6800.051;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;331;-2924.646,6429.647;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;290;-3939.871,6316.315;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;289;-4112.989,6314.956;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;288;-4412.063,6536.854;Float;False;Property;_CloudsHorizonHeight;Clouds Horizon Height;24;0;Create;True;0;0;0;False;0;False;0;3;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;291;-3780.257,6309.926;Float;True;SkyMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;327;-2688.066,6428.166;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;316;-3467.426,6353.554;Inherit;True;262;SunMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;454;-4189.904,7679.33;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;461;-3408.73,7528.055;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;471;-3385.533,7918.235;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;726;-2939.674,7646.717;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;465;-2917.68,7776.716;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;460;-3592.329,7533.858;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;455;-4041.578,7623.627;Inherit;False;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;476;-3171.312,8137.154;Inherit;False;False;False;False;True;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;725;-2738.174,7644.119;Float;False;MoonSkyMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;463;-3053.436,7499.148;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;457;-3839.177,7691.526;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;456;-4017.038,7724.025;Float;False;Constant;_Float5;Float 5;57;0;Create;True;0;0;0;False;0;False;0.5;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;1821;-3181.324,7893.963;Float;False;MoonLightColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;467;-2579.881,7753.497;Float;True;Moon;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;474;-4135.205,7862.869;Float;False;Property;_MoonNightColor;Moon Night Color;60;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0.9380562,1.089322,1.283019,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;472;-3751.017,8157.543;Inherit;False;2062;DaynightProgress;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;687;-4883.668,9048.18;Float;False;Property;_SunRadius;Sun Radius;53;0;Create;True;0;0;0;False;0;False;0.11;0.1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;685;-4147.354,8896.064;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;686;-4019.806,8894.779;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;692;-3726.323,8650.564;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;691;-3736.358,8896.849;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;268;-2993.844,9061.925;Inherit;False;291;SkyMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;690;-3567.428,8670.904;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;272;-3342.318,8634.219;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;271;-2790.808,8755.379;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;275;-2638.238,8676.038;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;274;-2646.928,8917.013;Float;False;Property;_SunSunsetColor;Sun Sunset Color;47;1;[HDR];Create;True;0;0;0;False;0;False;1,0.8823529,0.8823529,0;3.457286,2.261014,0.733858,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;273;-2634.397,8751.698;Float;False;Property;_SunDayColor;Sun Day Color;46;1;[HDR];Create;True;0;0;0;False;1;Header(Sun Settings);False;1,1,1,0;1.698113,1.280253,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;277;-2384.894,8939.406;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;278;-2401.209,8623.31;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;279;-2213.245,8833.03;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;266;-4874.464,8890.155;Float;False;Property;_SunHaloSize;Sun Halo Size;52;0;Create;True;0;0;0;False;0;False;0;0.445;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;706;-4367.441,9067.287;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;250;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;684;-4402.347,8882.778;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;263;-4877.813,8808.96;Float;False;Property;_SunHaloRadius;Sun Halo Radius;50;0;Create;True;0;0;0;False;0;False;0.11;0.256;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;270;-4031.158,8660.271;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;681;-3096.646,8619.916;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;265;-3303.515,8753.26;Inherit;False;1736;CloudMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;264;-4179.423,8658.33;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;276;-2609.094,9086.847;Inherit;False;2106;SunsetMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;280;-1911,8815.819;Float;True;Sun;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PannerNode;2378;-3120.083,2313.966;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,1;False;1;FLOAT;0.02;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2244;-2709.316,2293.926;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;2224;-3378.157,2337.737;Inherit;False;Property;_CloudSpeed;Cloud Speed;18;0;Create;True;0;0;0;False;0;False;0.2;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;3209;-3281.321,2485.654;Inherit;False;Cloud Speed;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;1736;212.789,2839.245;Float;True;CloudMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;3177;-40.58699,1887.489;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,1;False;1;FLOAT;0.02;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3179;255.6559,1905.967;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;3210;-334.8228,2007.213;Inherit;False;3209;Cloud Speed;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;3180;507.0421,1922.628;Inherit;True;Property;_CloudBottomMask;Cloud Bottom Mask;32;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;9bbf4626a1a7c174eb96009368469ec2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;3176;-689.9164,1912.416;Inherit;True;107;CloudsUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;2998;1408.522,1481.822;Float;True;CloudBottomMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3203;827.9063,2202.399;Inherit;False;Property;_CloudBottomOpacity;Cloud Bottom Opacity;34;0;Create;True;0;0;0;False;0;False;0;0.76;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2993;-1316.081,1544.156;Inherit;False;Property;_CloudBottomScale;Cloud Bottom Scale;33;0;Create;True;0;0;0;False;0;False;0;0.8043478;-2;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2994;-1321.029,1377.056;Inherit;False;Property;_CloudBottomBias;Cloud Bottom Bias;35;0;Create;True;0;0;0;False;0;False;0;0.1522039;-5;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;2996;-952.5714,1409.307;Inherit;True;ConstantBiasScale;-1;;1893;63208df05c83e8e49a48ffbdce2e43a0;0;3;3;FLOAT;1;False;1;FLOAT;2;False;2;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;1820;-1300.997,2686.285;Float;False;Property;_CloudCoverage1;Cloud Coverage;12;0;Create;True;0;0;0;False;1;Header(Clouds Settings);False;0;0.532;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;1878;-3467.834,1408.899;Float;False;Property;_BackgroundOpacityNight;Background Opacity Night;43;0;Create;True;0;0;0;False;0;False;0;1.28;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2471;-3448.539,1524.744;Float;False;Property;_BackgroundOpacityDay;Background Opacity Day;44;0;Create;True;0;0;0;False;0;False;0;1.077;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1874;-3209.62,1034.544;Inherit;True;Property;_Background;Background ;40;0;Create;True;0;0;0;False;0;False;-1;None;9bbf4626a1a7c174eb96009368469ec2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;269;-3114.869,8929.32;Float;False;CloudMasking;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;267;-3439.43,8929.012;Float;False;Property;_CloudMaskingSunMoon;Cloud Masking Sun/Moon;15;0;Create;True;0;0;0;False;0;False;0.85;1;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;250;-2248.732,4494.586;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;522;-1467.118,4489.844;Float;False;Property;_StarsColor;StarsColor;66;1;[HDR];Create;True;0;0;0;False;1;Header(Star Settings);False;1,1,1,0;0.7490196,0.7490196,0.7490196,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;462;-3525.968,7638.66;Inherit;True;Property;_Moon;Moon;62;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;558bd318dfe571a4287b5fcee3a4806d;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;473;-4129.001,8052.265;Float;False;Property;_MoonDayColor;Moon Day Color;59;0;Create;True;0;0;0;False;1;Header(Moon Settings);False;0,0,0,0;1,1,1,0.1960784;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;2257;-3897.168,1141.376;Inherit;False;Property;_BackgroundSpeed;Background Speed;39;0;Create;True;0;0;0;False;0;False;0.2;0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;1914;-2772.223,1242.512;Float;False;Property;_BackgroundPower;Background Power;37;0;Create;True;0;0;0;False;1;Header(Clouds Background);False;0;0.1;0.1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2239;-2266.061,1908.763;Inherit;True;Property;_CloudEdgeMask;Cloud Edge Mask;19;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;daee5e1bcc85467458941e16c068e8fc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;2240;-2850.335,2091.755;Inherit;False;Property;_CloudEdgeSize;Cloud Edge Size;21;0;Create;True;0;0;0;False;0;False;0;0.77;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2238;-3166,1986;Inherit;False;Property;_CloudEdgeSpeed;Cloud Edge Speed;22;0;Create;True;0;0;0;False;0;False;0.2;7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2225;-3046.433,2473.507;Inherit;False;Property;_CloudSize;Cloud Size;17;0;Create;True;0;0;0;False;0;False;1;0.06062649;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3181;-108.766,2076.81;Inherit;False;Property;_CloudBottomSize;Cloud Bottom Size;36;0;Create;True;0;0;0;False;0;False;0;0.6447861;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;1740;-822.9695,3007.901;Float;False;Property;_CloudBrightness;Cloud Brightness;13;0;Create;True;0;0;0;False;1;;False;0.4486982;0.33;0;0.999999;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;1735;-847.1293,2906.644;Float;False;Property;_CloudContrast;Cloud Contrast;14;0;Create;True;0;0;0;False;0;False;27.57542;5;0;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2242;-1801.288,2498.072;Float;False;Property;_CloudEdgeOpacity;Cloud Edge Opacity;20;0;Create;True;0;0;0;False;0;False;0;0.13;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2245;-2343.686,2284.89;Inherit;True;Property;_CloudMain;Cloud Main;16;1;[NoScaleOffset];Create;True;0;0;0;False;1;Header(Clouds Visual Settings);False;-1;None;4ac45dbaa42b55747bab15e821419ba1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;362;-2247.261,6488.779;Float;False;ScatteringMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;486;-2248.885,6799.732;Float;False;MoonScatteringMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;568;-1458.1,8143.829;Inherit;False;2062;DaynightProgress;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;3236;534.5182,7840.784;Inherit;True;3;0;COLOR;1,1,1,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;3235;39.51833,7982.393;Inherit;False;2062;DaynightProgress;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;618;-3999.453,6935.525;Float;False;Property;_AutoLightDirection;Auto Light Direction;75;0;Create;True;0;0;0;False;1;Header(Other Settings);False;0;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;293;-4887.916,6297.257;Inherit;True;254;WorldPositionNormalised;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;286;-4599.916,6249.257;Inherit;True;False;True;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;285;-4608.987,6433.057;Float;False;Property;_CloudsHorizonGradient;Clouds Horizon Gradient;23;0;Create;True;0;0;0;False;0;False;1;5.92;0;25;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;287;-4295.916,6313.257;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;452;-4458.27,7613.405;Inherit;False;747;MoonRotation;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;747;426.8088,8791.471;Float;False;MoonRotation;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;459;-3718.973,7640.026;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;3298;-613.6935,8766.278;Inherit;True;RotateAngleAxis;-1;;1894;72edad85bb5dea440905ae88eddfa489;0;2;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.FunctionNode;3300;-335.9893,8725.341;Inherit;True;RotateVector;-1;;1895;5c6ddc37cb38dfb458f9519ddf619b0c;0;2;1;FLOAT3;0,0,0;False;2;FLOAT4;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalizeNode;3307;-812.7877,8876.997;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;619;-4286.473,7016.859;Inherit;True;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleDivideOpNode;2095;-4074.884,5476.989;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;2092;-4272.785,5530.867;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;2090;-4403.338,5507.036;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;2094;-4328.668,5408.525;Inherit;False;262;SunMask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;2089;-4693.613,5468.27;Inherit;False;False;True;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;2088;-4921.614,5470.27;Inherit;False;622;LightDirection;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;622;-3749.051,6936.497;Float;False;LightDirection;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;281;-4890.388,8631.918;Inherit;False;254;WorldPositionNormalised;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DistanceOpNode;261;-4605.209,8673.339;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;262;-4452.279,8651.047;Float;True;SunMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;3317;1443.601,8334.957;Float;False;LightDirection;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;458;-4045.039,7515.542;Inherit;False;False;False;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;762;931.6661,9017.112;Float;False;MoonGradient;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;827;723.4645,9010.565;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;3301;-595.315,9030;Inherit;True;FromToRotation;-1;;1898;ad10913350839ec49a3853aee4185e18;0;2;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.FunctionNode;3302;-17.45965,8939.612;Inherit;True;RotateVector;-1;;1900;5c6ddc37cb38dfb458f9519ddf619b0c;0;2;1;FLOAT3;0,0,0;False;2;FLOAT4;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;3299;-777.7916,8676.866;Inherit;False;254;WorldPositionNormalised;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;761;418.0326,9027.206;Inherit;True;False;False;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;3324;-875.6018,8762.268;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;3315;-1443.17,8930.185;Inherit;True;622;LightDirection;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DistanceOpNode;3319;-1202.564,8795.229;Inherit;False;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RadiansOpNode;3308;-1042.525,8724.103;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;3309;-842.9927,9032.391;Float;False;Property;_Vector5;Vector 5;55;0;Create;True;0;0;0;False;0;False;0.2,0.2,0.2;0,0.5,0.4;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;3306;-1097.671,8907.48;Float;False;Property;_MoonRotationAxis2;Moon Rotation Axis;61;0;Create;True;0;0;0;False;0;False;0.85,1,0;-0.4,-1.1,-0.5;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;624;-4858.167,8724.476;Inherit;False;622;LightDirection;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ObjSpaceLightDirHlpNode;3325;-4009.584,7171.48;Inherit;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;453;-4507.663,7718.362;Float;False;Property;_MoonSize;Moon Size;65;0;Create;True;0;0;0;False;0;False;1;4.52;0;30;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3303;-1457.059,8696.23;Float;False;Constant;_MoonRotationAngle;Moon Rotation Angle;65;0;Create;True;0;0;0;False;0;False;90;90;90;90;0;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;662;-4173.556,6843;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector3Node;666;-4512.268,6983.421;Float;False;Constant;_Vector0;Vector 0;61;0;Create;True;0;0;0;False;0;False;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.AbsOpNode;574;-2021.467,-153.4823;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;899;-2325.822,-58.11905;Inherit;True;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.DotProductOpNode;2480;-4930.042,3738.555;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,1,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;2483;-4499.934,3620.546;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;2484;-4315.835,3798.727;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2482;-4736.421,3997.888;Float;False;Property;_HorizontLinePower;Horizont Line Power;10;0;Create;True;0;0;0;False;0;False;124.398;20;-5;200;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3328;-3690.47,3941.059;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;3330;-3983.487,4005.38;Inherit;False;Property;_HorizontLineOpacity;Horizont Line Opacity;11;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;3332;-3920.122,4102.754;Inherit;False;Property;_HorizontLineColor;Horizont Line Color;9;1;[HDR];Create;True;0;0;0;False;0;False;0.9622642,0.9622642,0.9622642,0;0.9622642,0.9622642,0.9622642,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;904;1268.458,5084.729;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;905;961.0112,5120.991;Inherit;True;Property;_CullMode;Cull Mode;77;1;[Enum];Create;True;0;0;1;UnityEngine.Rendering.CullMode;False;0;False;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;226;-2916.615,5213.689;Float;False;Property;_SunsetHorizonColor;Sunset Horizon Color;51;1;[HDR];Create;True;0;0;0;False;0;False;1,0.2705882,0.1686275,0;1.059274,0.3272102,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;294;-1467.253,4061.867;Inherit;True;228;SkyColor;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;2442;702.0107,4004.287;Float;False;Property;_SunsetColorBackground;Sunset Color Background ;45;1;[HDR];Create;True;0;0;0;False;0;False;1,0.2705882,0.1686275,0;2,0.6980392,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;2438;-62.31499,3858.619;Float;False;Property;_DayColorBackground;Day Color Background;41;1;[HDR];Create;True;0;0;0;False;0;False;0,0.1359026,0.2941176,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;2436;-66.93977,3618.783;Float;False;Property;_NightColorBackground;Night Color Background;42;1;[HDR];Create;True;0;0;0;False;0;False;0.0537526,0,0.1470588,0;0.1021271,0.1700009,0.4245283,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;2058;-3549.499,3552.852;Float;False;Property;_DayColorBottom;Day Color Bottom;2;1;[HDR];Create;True;0;0;0;False;0;False;0.1544118,0.5451319,1,0;0,0.2513504,0.4056604,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;2056;-3528.159,3747.962;Float;False;Property;_DayColorTop;Day Color Top;1;1;[HDR];Create;True;0;0;0;False;1;Header(Sky Color Settings);False;0.3455882,0.8104462,1,0;0.3304557,0.6472845,0.8867924,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;2049;-3924.947,4348.457;Float;False;Property;_NightColorBottom;Night Color Bottom;4;1;[HDR];Create;True;0;0;0;False;0;False;0.0537526,0,0.1470588,0;0.0350346,0.06512431,0.2646992,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;2048;-3924.937,4525.555;Float;False;Property;_NightColorTop;Night Color Top;3;1;[HDR];Create;True;0;0;0;False;0;False;0,0.1359026,0.2941176,0;0.02531061,0,0.2075472,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;299;-1482.064,7130.47;Float;False;Property;_CloudNightColor;Cloud Night Color;27;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,0;0.06653692,0.06007477,0.2830189,0.9843137;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;298;-1482.012,7313.261;Float;False;Property;_CloudDayColor;Cloud Day Color;25;1;[HDR];Create;True;0;0;0;False;1;Header(Clouds Color Settings);False;1,1,1,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;304;-1486.493,7573.96;Float;False;Property;_CloudSunsetColor;Cloud Sunset Color;48;1;[HDR];Create;True;0;0;0;False;0;False;1,0.8382353,0.8382353,0;0.9734172,0.2290393,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;563;-1474.713,7776.118;Float;False;Property;_CloudNightColorBottom;Cloud Night Color Bottom;28;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0.03671236,0.04369963,0.2358491,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;564;-1486.538,7943.152;Float;False;Property;_CloudDayColorBottom;Cloud Day Color Bottom;26;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0.8143467,0.8857358,0.9433962,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;565;-1480.105,8243.009;Float;False;Property;_CloudSunsetColorBottom;Cloud Sunset Color Bottom;49;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;1.788501,0.5337412,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
WireConnection;605;0;925;0
WireConnection;605;1;604;0
WireConnection;609;0;605;0
WireConnection;609;1;511;0
WireConnection;429;0;430;0
WireConnection;433;0;431;0
WireConnection;433;1;429;0
WireConnection;936;0;609;0
WireConnection;434;0;433;0
WireConnection;569;0;567;0
WireConnection;569;1;565;0
WireConnection;569;2;570;0
WireConnection;566;0;303;0
WireConnection;566;1;304;0
WireConnection;566;2;305;0
WireConnection;407;0;566;0
WireConnection;407;1;569;0
WireConnection;407;2;408;0
WireConnection;427;0;436;0
WireConnection;427;1;413;0
WireConnection;490;0;487;0
WireConnection;490;1;491;0
WireConnection;414;0;427;0
WireConnection;414;1;407;0
WireConnection;414;2;3236;0
WireConnection;920;0;914;0
WireConnection;917;0;911;0
WireConnection;917;1;920;0
WireConnection;928;0;921;0
WireConnection;928;1;929;0
WireConnection;921;0;917;0
WireConnection;921;2;922;0
WireConnection;907;1;928;0
WireConnection;567;0;563;0
WireConnection;567;1;564;0
WireConnection;567;2;568;0
WireConnection;586;0;585;0
WireConnection;633;0;632;0
WireConnection;505;0;633;0
WireConnection;911;0;913;0
WireConnection;914;0;913;0
WireConnection;510;0;586;0
WireConnection;508;0;505;0
WireConnection;925;0;907;3
WireConnection;509;0;587;0
WireConnection;585;0;1690;1
WireConnection;585;1;1690;2
WireConnection;585;2;1690;3
WireConnection;2047;0;2052;0
WireConnection;2047;1;2046;0
WireConnection;2050;0;2053;0
WireConnection;2051;0;2050;0
WireConnection;2052;0;2063;0
WireConnection;2053;0;2047;0
WireConnection;2053;1;2068;0
WireConnection;2054;0;2056;0
WireConnection;2054;1;2058;0
WireConnection;2054;2;2059;0
WireConnection;901;0;897;1
WireConnection;901;2;897;3
WireConnection;582;0;579;0
WireConnection;900;0;252;0
WireConnection;900;1;901;0
WireConnection;581;0;899;0
WireConnection;581;1;899;2
WireConnection;579;0;581;0
WireConnection;579;1;583;0
WireConnection;583;0;581;0
WireConnection;578;0;899;1
WireConnection;578;1;574;0
WireConnection;303;0;299;0
WireConnection;303;1;298;0
WireConnection;303;2;301;0
WireConnection;2060;0;2059;0
WireConnection;2057;0;2066;0
WireConnection;2485;0;2484;0
WireConnection;2059;0;2067;0
WireConnection;2481;0;2480;0
WireConnection;2055;0;2049;0
WireConnection;2055;1;2048;0
WireConnection;2055;2;2067;0
WireConnection;2067;0;2057;0
WireConnection;2067;2;2061;0
WireConnection;591;0;2506;0
WireConnection;587;0;506;0
WireConnection;506;0;503;0
WireConnection;506;1;504;0
WireConnection;506;2;501;0
WireConnection;2506;0;2507;0
WireConnection;2506;1;2505;0
WireConnection;2507;0;590;0
WireConnection;405;0;414;0
WireConnection;524;0;522;4
WireConnection;524;1;517;0
WireConnection;1120;0;1119;0
WireConnection;1121;0;1120;0
WireConnection;1118;0;295;0
WireConnection;1118;1;1121;0
WireConnection;297;0;1118;0
WireConnection;297;1;519;0
WireConnection;469;0;1172;0
WireConnection;469;1;470;0
WireConnection;469;2;468;0
WireConnection;1172;0;297;0
WireConnection;1172;1;1174;0
WireConnection;2440;0;2436;0
WireConnection;2440;1;2438;0
WireConnection;2440;2;2441;0
WireConnection;1947;0;469;0
WireConnection;1947;1;2443;0
WireConnection;1947;2;1942;0
WireConnection;519;0;294;0
WireConnection;519;1;2492;0
WireConnection;519;2;524;0
WireConnection;523;0;516;0
WireConnection;523;1;522;0
WireConnection;2492;0;523;0
WireConnection;2492;1;522;0
WireConnection;309;0;1947;0
WireConnection;309;1;412;0
WireConnection;309;2;411;0
WireConnection;412;0;406;0
WireConnection;903;0;309;0
WireConnection;903;1;904;0
WireConnection;0;2;903;0
WireConnection;2443;0;2440;0
WireConnection;2443;1;2442;0
WireConnection;2443;2;2446;0
WireConnection;514;0;1690;0
WireConnection;358;0;359;0
WireConnection;358;1;296;0
WireConnection;411;0;358;0
WireConnection;2237;0;2241;0
WireConnection;2237;2;2238;0
WireConnection;2381;0;2237;0
WireConnection;2381;1;2240;0
WireConnection;436;0;434;0
WireConnection;436;1;435;0
WireConnection;2062;0;2051;0
WireConnection;3205;0;3190;0
WireConnection;3205;1;3206;0
WireConnection;3206;0;3180;1
WireConnection;3206;1;3203;0
WireConnection;3010;0;3008;0
WireConnection;3010;1;3009;0
WireConnection;3010;2;3007;0
WireConnection;3199;0;3196;0
WireConnection;3195;0;3189;0
WireConnection;3195;1;3199;0
WireConnection;3201;0;3200;0
WireConnection;3201;1;3195;0
WireConnection;3011;0;3010;0
WireConnection;3011;1;3006;0
WireConnection;3011;2;3005;0
WireConnection;3200;0;3194;0
WireConnection;3200;1;3196;0
WireConnection;3003;0;3011;0
WireConnection;3003;1;2997;0
WireConnection;1737;0;1740;0
WireConnection;2248;0;2243;0
WireConnection;2248;1;1819;0
WireConnection;1819;0;1820;0
WireConnection;2243;0;2245;1
WireConnection;2243;1;2239;1
WireConnection;2243;2;2242;0
WireConnection;2997;0;2245;0
WireConnection;2997;1;2996;0
WireConnection;1738;5;2248;0
WireConnection;1738;13;1735;0
WireConnection;1739;0;1738;0
WireConnection;1739;2;1737;0
WireConnection;99;0;576;0
WireConnection;101;0;99;1
WireConnection;101;2;99;1
WireConnection;101;3;103;0
WireConnection;101;4;99;1
WireConnection;102;0;99;0
WireConnection;102;1;99;2
WireConnection;104;0;102;0
WireConnection;104;1;101;0
WireConnection;254;0;253;0
WireConnection;253;0;573;0
WireConnection;573;0;582;0
WireConnection;573;1;578;0
WireConnection;573;2;582;1
WireConnection;3190;0;3201;0
WireConnection;3190;1;3003;0
WireConnection;107;0;104;0
WireConnection;2256;0;1871;0
WireConnection;2256;2;2257;0
WireConnection;1873;0;2256;0
WireConnection;1873;1;1872;0
WireConnection;1912;0;1874;1
WireConnection;1912;1;2470;0
WireConnection;1912;2;1936;0
WireConnection;2470;0;1878;0
WireConnection;2470;1;2471;0
WireConnection;2470;2;2467;0
WireConnection;2124;0;1912;0
WireConnection;2124;1;1914;0
WireConnection;2123;0;2124;0
WireConnection;1917;0;2123;0
WireConnection;1690;1;1716;0
WireConnection;1716;0;1718;0
WireConnection;1716;1;1717;0
WireConnection;512;0;508;0
WireConnection;512;1;510;0
WireConnection;512;2;509;0
WireConnection;512;3;591;0
WireConnection;512;4;936;0
WireConnection;513;0;1109;0
WireConnection;1109;0;512;0
WireConnection;1109;1;1110;0
WireConnection;1105;0;1104;0
WireConnection;1110;0;1105;0
WireConnection;1047;0;1050;0
WireConnection;1076;0;1046;0
WireConnection;1076;1;1068;0
WireConnection;1076;2;1165;0
WireConnection;1175;0;1047;0
WireConnection;1068;0;1164;0
WireConnection;1068;1;1051;0
WireConnection;1164;0;1175;0
WireConnection;1167;0;1076;0
WireConnection;2093;0;2091;0
WireConnection;2097;0;2093;0
WireConnection;2099;0;2095;0
WireConnection;2099;1;2098;0
WireConnection;2100;0;2097;0
WireConnection;2100;1;2096;0
WireConnection;2101;0;2100;0
WireConnection;2102;0;2099;0
WireConnection;2103;0;2102;0
WireConnection;2104;0;2101;0
WireConnection;2105;0;2104;0
WireConnection;2105;1;2103;0
WireConnection;2105;2;2119;0
WireConnection;2110;0;2108;0
WireConnection;2111;0;2110;0
WireConnection;2111;1;2109;0
WireConnection;2113;0;2111;0
WireConnection;2113;1;2112;0
WireConnection;2117;0;2113;0
WireConnection;2118;0;2117;0
WireConnection;2119;0;2118;0
WireConnection;227;0;250;0
WireConnection;227;1;226;0
WireConnection;227;2;2106;0
WireConnection;2106;0;2105;0
WireConnection;2064;0;2054;0
WireConnection;2064;1;2060;0
WireConnection;2474;0;2064;0
WireConnection;2474;1;3328;0
WireConnection;228;0;227;0
WireConnection;663;0;660;0
WireConnection;664;0;660;0
WireConnection;321;0;317;0
WireConnection;321;1;488;0
WireConnection;320;0;316;0
WireConnection;324;0;321;0
WireConnection;322;0;320;0
WireConnection;322;1;319;0
WireConnection;326;0;324;0
WireConnection;326;1;325;0
WireConnection;492;0;326;0
WireConnection;331;0;322;0
WireConnection;290;0;289;0
WireConnection;289;0;287;0
WireConnection;289;1;288;0
WireConnection;291;0;290;0
WireConnection;327;0;331;0
WireConnection;327;1;328;0
WireConnection;454;0;452;0
WireConnection;454;1;453;0
WireConnection;461;0;460;0
WireConnection;471;0;474;0
WireConnection;471;1;473;0
WireConnection;471;2;472;0
WireConnection;726;0;463;0
WireConnection;726;1;476;0
WireConnection;465;0;462;0
WireConnection;465;1;1821;0
WireConnection;460;0;458;0
WireConnection;455;0;454;0
WireConnection;476;0;471;0
WireConnection;725;0;726;0
WireConnection;463;0;461;0
WireConnection;463;1;462;4
WireConnection;457;0;455;0
WireConnection;457;1;456;0
WireConnection;1821;0;471;0
WireConnection;467;0;465;0
WireConnection;685;0;262;0
WireConnection;685;1;687;0
WireConnection;686;0;685;0
WireConnection;686;3;706;0
WireConnection;692;0;270;0
WireConnection;691;0;686;0
WireConnection;690;0;692;0
WireConnection;690;1;691;0
WireConnection;272;0;690;0
WireConnection;271;0;265;0
WireConnection;271;1;269;0
WireConnection;271;2;268;0
WireConnection;275;0;271;0
WireConnection;277;0;273;0
WireConnection;277;1;274;0
WireConnection;277;2;276;0
WireConnection;278;0;681;0
WireConnection;278;2;275;0
WireConnection;279;0;278;0
WireConnection;279;1;277;0
WireConnection;706;0;687;0
WireConnection;684;0;266;0
WireConnection;270;0;264;0
WireConnection;270;3;684;0
WireConnection;681;0;272;0
WireConnection;264;0;262;0
WireConnection;264;1;263;0
WireConnection;280;0;279;0
WireConnection;2378;0;2241;0
WireConnection;2378;2;2224;0
WireConnection;2244;0;2378;0
WireConnection;2244;1;2225;0
WireConnection;3209;0;2224;0
WireConnection;1736;0;1739;0
WireConnection;3177;0;3176;0
WireConnection;3177;2;3210;0
WireConnection;3179;0;3177;0
WireConnection;3179;1;3181;0
WireConnection;3180;1;3179;0
WireConnection;2998;0;3205;0
WireConnection;2996;1;2994;0
WireConnection;2996;2;2993;0
WireConnection;1874;1;1873;0
WireConnection;269;0;267;0
WireConnection;250;0;2055;0
WireConnection;250;1;2474;0
WireConnection;250;2;2062;0
WireConnection;462;1;459;0
WireConnection;2239;1;2381;0
WireConnection;2245;1;2244;0
WireConnection;362;0;327;0
WireConnection;486;0;492;0
WireConnection;3236;0;490;0
WireConnection;3236;2;3235;0
WireConnection;618;0;662;0
WireConnection;618;1;619;0
WireConnection;286;0;293;0
WireConnection;287;0;286;0
WireConnection;287;1;285;0
WireConnection;747;0;3302;0
WireConnection;459;0;457;0
WireConnection;3298;2;3324;0
WireConnection;3298;3;3307;0
WireConnection;3300;1;3299;0
WireConnection;3300;2;3298;0
WireConnection;3307;0;3306;0
WireConnection;2095;0;2094;0
WireConnection;2095;1;2092;0
WireConnection;2092;0;2090;0
WireConnection;2090;0;2089;0
WireConnection;2089;0;2088;0
WireConnection;622;0;618;0
WireConnection;261;0;281;0
WireConnection;261;1;624;0
WireConnection;262;0;261;0
WireConnection;458;0;452;0
WireConnection;762;0;827;0
WireConnection;827;0;761;0
WireConnection;3301;1;3307;0
WireConnection;3301;2;3309;0
WireConnection;3302;1;3300;0
WireConnection;3302;2;3301;0
WireConnection;761;0;3302;0
WireConnection;3324;0;3308;0
WireConnection;3324;1;3319;0
WireConnection;3319;0;3303;0
WireConnection;3319;1;3315;0
WireConnection;3308;0;3319;0
WireConnection;662;0;663;0
WireConnection;662;2;664;0
WireConnection;662;3;666;0
WireConnection;662;4;664;0
WireConnection;574;0;899;1
WireConnection;899;0;900;0
WireConnection;2480;0;2066;0
WireConnection;2483;0;2481;0
WireConnection;2484;0;2483;0
WireConnection;2484;1;2482;0
WireConnection;3328;0;2485;0
WireConnection;3328;1;3330;0
WireConnection;3328;2;3332;0
WireConnection;904;0;905;0
ASEEND*/
//CHKSM=F40E9BDF871191A01BB146042CCF05F24CC2B30F