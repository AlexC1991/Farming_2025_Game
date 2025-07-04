using UnityEngine;
using UnityEditor;

namespace BioStart.Shaders
{
public class SkyBoxShaderHDREditor : ShaderGUI
{
    // MaterialProperty variables for all shader properties
    MaterialProperty _DayColorTop;
    MaterialProperty _DayColorBottom;
    MaterialProperty _NightColorTop;
    MaterialProperty _NightColorBottom;
    MaterialProperty _HorizonColor;
    MaterialProperty _SkyHorizonGradient;
    MaterialProperty _HorizonHeight;
    MaterialProperty _HorizonContrast;
    MaterialProperty _HorizontLineColor;
    MaterialProperty _HorizontLinePower;
    MaterialProperty _HorizontLineOpacity;

    MaterialProperty _CloudCoverage1;
    MaterialProperty _CloudBrightness;
    MaterialProperty _CloudContrast;
    MaterialProperty _CloudMaskingSunMoon;

    MaterialProperty _CloudMain;
    MaterialProperty _CloudSize;
    MaterialProperty _CloudSpeed;
    MaterialProperty _CloudEdgeMask;
    MaterialProperty _CloudEdgeOpacity;
    MaterialProperty _CloudEdgeSize;
    MaterialProperty _CloudEdgeSpeed;
    MaterialProperty _CloudsHorizonGradient;
    MaterialProperty _CloudsHorizonHeight;

    MaterialProperty _CloudDayColor;
    MaterialProperty _CloudDayColorBottom;
    MaterialProperty _CloudNightColor;
    MaterialProperty _CloudNightColorBottom;
    MaterialProperty _CloudBottomContrastDay1;
    MaterialProperty _CloudBottomContrastNight1;
    MaterialProperty _CloudBottomContrastSunset1;
    MaterialProperty _CloudBottomMask;
    MaterialProperty _CloudBottomScale;
    MaterialProperty _CloudBottomOpacity;
    MaterialProperty _CloudBottomBias;
    MaterialProperty _CloudBottomSize;

    MaterialProperty _BackgroundPower;
    MaterialProperty _BackgroundSize;
    MaterialProperty _BackgroundSpeed;
    MaterialProperty _Background;
    MaterialProperty _DayColorBackground;
    MaterialProperty _NightColorBackground;
    MaterialProperty _BackgroundOpacityNight;
    MaterialProperty _BackgroundOpacityDay;
    MaterialProperty _SunsetColorBackground;

    MaterialProperty _SunDayColor;
    MaterialProperty _SunSunsetColor;
    MaterialProperty _CloudSunsetColor;
    MaterialProperty _CloudSunsetColorBottom;
    MaterialProperty _SunHaloRadius;
    MaterialProperty _SunsetHorizonColor;
    MaterialProperty _SunHaloSize;
    MaterialProperty _SunRadius;
    MaterialProperty _SunScatteringRadius;
    MaterialProperty _SunScattering;
    MaterialProperty _SunsetHorizonHeight;
    MaterialProperty _SunsetSize1;

    MaterialProperty _MoonDayColor;
    MaterialProperty _MoonNightColor;
    MaterialProperty _Vector5;
    MaterialProperty _MoonRotationAxis2;
    MaterialProperty _Moon;
    MaterialProperty _MoonScattering;
    MaterialProperty _MoonScatteringRadius;
    MaterialProperty _MoonSize;

    MaterialProperty _StarsColor;
    MaterialProperty _Star;
    MaterialProperty _NightSkyBrightness;
    MaterialProperty _StarsHorizonFade;
    MaterialProperty _StarsBlinkBrightness;
    MaterialProperty _StarsBlinkSpeed;
    MaterialProperty _StarsBlinkNoise;
    MaterialProperty _StarsBlinkNoiseSize;
    MaterialProperty _StarSize1;

    MaterialProperty _AutoLightDirection;
    MaterialProperty _ManualLightDirection;
    MaterialProperty _CullMode;
    MaterialProperty _InvertWorld;
    MaterialProperty _MirrorWorld;

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        // Fetch material properties
        _DayColorTop = FindProperty("_DayColorTop", properties);
        _DayColorBottom = FindProperty("_DayColorBottom", properties);
        _NightColorTop = FindProperty("_NightColorTop", properties);
        _NightColorBottom = FindProperty("_NightColorBottom", properties);
        _HorizonColor = FindProperty("_HorizonColor", properties);
        _SkyHorizonGradient = FindProperty("_SkyHorizonGradient", properties);
        _HorizonHeight = FindProperty("_HorizonHeight", properties);
        _HorizonContrast = FindProperty("_HorizonContrast", properties);
        _HorizontLineColor = FindProperty("_HorizontLineColor", properties);
        _HorizontLinePower = FindProperty("_HorizontLinePower", properties);
        _HorizontLineOpacity = FindProperty("_HorizontLineOpacity", properties);

        _CloudCoverage1 = FindProperty("_CloudCoverage1", properties);
        _CloudBrightness = FindProperty("_CloudBrightness", properties);
        _CloudContrast = FindProperty("_CloudContrast", properties);
        _CloudMaskingSunMoon = FindProperty("_CloudMaskingSunMoon", properties);

        _CloudMain = FindProperty("_CloudMain", properties);
        _CloudSize = FindProperty("_CloudSize", properties);
        _CloudSpeed = FindProperty("_CloudSpeed", properties);
        _CloudEdgeMask = FindProperty("_CloudEdgeMask", properties);
        _CloudEdgeOpacity = FindProperty("_CloudEdgeOpacity", properties);
        _CloudEdgeSize = FindProperty("_CloudEdgeSize", properties);
        _CloudEdgeSpeed = FindProperty("_CloudEdgeSpeed", properties);
        _CloudsHorizonGradient = FindProperty("_CloudsHorizonGradient", properties);
        _CloudsHorizonHeight = FindProperty("_CloudsHorizonHeight", properties);

        _CloudDayColor = FindProperty("_CloudDayColor", properties);
        _CloudDayColorBottom = FindProperty("_CloudDayColorBottom", properties);
        _CloudNightColor = FindProperty("_CloudNightColor", properties);
        _CloudNightColorBottom = FindProperty("_CloudNightColorBottom", properties);
        _CloudBottomContrastDay1 = FindProperty("_CloudBottomContrastDay1", properties);
        _CloudBottomContrastNight1 = FindProperty("_CloudBottomContrastNight1", properties);
        _CloudBottomContrastSunset1 = FindProperty("_CloudBottomContrastSunset1", properties);
        _CloudBottomMask = FindProperty("_CloudBottomMask", properties);
        _CloudBottomScale = FindProperty("_CloudBottomScale", properties);
        _CloudBottomOpacity = FindProperty("_CloudBottomOpacity", properties);
        _CloudBottomBias = FindProperty("_CloudBottomBias", properties);
        _CloudBottomSize = FindProperty("_CloudBottomSize", properties);

        _BackgroundPower = FindProperty("_BackgroundPower", properties);
        _BackgroundSize = FindProperty("_BackgroundSize", properties);
        _BackgroundSpeed = FindProperty("_BackgroundSpeed", properties);
        _Background = FindProperty("_Background", properties);
        _DayColorBackground = FindProperty("_DayColorBackground", properties);
        _NightColorBackground = FindProperty("_NightColorBackground", properties);
        _BackgroundOpacityNight = FindProperty("_BackgroundOpacityNight", properties);
        _BackgroundOpacityDay = FindProperty("_BackgroundOpacityDay", properties);
        _SunsetColorBackground = FindProperty("_SunsetColorBackground", properties);

        _SunDayColor = FindProperty("_SunDayColor", properties);
        _SunSunsetColor = FindProperty("_SunSunsetColor", properties);
        _CloudSunsetColor = FindProperty("_CloudSunsetColor", properties);
        _CloudSunsetColorBottom = FindProperty("_CloudSunsetColorBottom", properties);
        _SunHaloRadius = FindProperty("_SunHaloRadius", properties);
        _SunsetHorizonColor = FindProperty("_SunsetHorizonColor", properties);
        _SunHaloSize = FindProperty("_SunHaloSize", properties);
        _SunRadius = FindProperty("_SunRadius", properties);
        _SunScatteringRadius = FindProperty("_SunScatteringRadius", properties);
        _SunScattering = FindProperty("_SunScattering", properties);
        _SunsetHorizonHeight = FindProperty("_SunsetHorizonHeight", properties);
        _SunsetSize1 = FindProperty("_SunsetSize1", properties);

        _MoonDayColor = FindProperty("_MoonDayColor", properties);
        _MoonNightColor = FindProperty("_MoonNightColor", properties);
        _Vector5 = FindProperty("_Vector5", properties);
        _MoonRotationAxis2 = FindProperty("_MoonRotationAxis2", properties);
        _Moon = FindProperty("_Moon", properties);
        _MoonScattering = FindProperty("_MoonScattering", properties);
        _MoonScatteringRadius = FindProperty("_MoonScatteringRadius", properties);
        _MoonSize = FindProperty("_MoonSize", properties);

        _StarsColor = FindProperty("_StarsColor", properties);
        _Star = FindProperty("_Star", properties);
        _NightSkyBrightness = FindProperty("_NightSkyBrightness", properties);
        _StarsHorizonFade = FindProperty("_StarsHorizonFade", properties);
        _StarsBlinkBrightness = FindProperty("_StarsBlinkBrightness", properties);
        _StarsBlinkSpeed = FindProperty("_StarsBlinkSpeed", properties);
        _StarsBlinkNoise = FindProperty("_StarsBlinkNoise", properties);
        _StarsBlinkNoiseSize = FindProperty("_StarsBlinkNoiseSize", properties);
        _StarSize1 = FindProperty("_StarSize1", properties);

        _AutoLightDirection = FindProperty("_AutoLightDirection", properties);
        _ManualLightDirection = FindProperty("_ManualLightDirection", properties);
        _CullMode = FindProperty("_CullMode", properties);
        _InvertWorld = FindProperty("_InvertWorld", properties);
        _MirrorWorld = FindProperty("_MirrorWorld", properties);

        // Begin drawing the custom inspector
        EditorGUILayout.Space();

        // Sky Color Settings
        EditorGUILayout.LabelField("Sky Color Settings", new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 13, 
        });
        EditorGUILayout.BeginVertical("box");
        materialEditor.ShaderProperty(_DayColorTop, "Day Color Top");
        materialEditor.ShaderProperty(_DayColorBottom, "Day Color Bottom");
        materialEditor.ShaderProperty(_NightColorTop, "Night Color Top");
        materialEditor.ShaderProperty(_NightColorBottom, "Night Color Bottom");
        materialEditor.ShaderProperty(_HorizonColor, "Horizon Color");
        materialEditor.ShaderProperty(_SkyHorizonGradient, "Sky Horizon Gradient");
        materialEditor.ShaderProperty(_HorizonHeight, "Horizon Height");
        materialEditor.ShaderProperty(_HorizonContrast, "Horizon Contrast");
        materialEditor.ShaderProperty(_HorizontLineColor, "Horizon Line Color");
        materialEditor.ShaderProperty(_HorizontLinePower, "Horizon Line Power");
        materialEditor.ShaderProperty(_HorizontLineOpacity, "Horizon Line Opacity");
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Cloud Settings", new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 13, 
        });
        EditorGUILayout.BeginVertical("box");
        materialEditor.ShaderProperty(_CloudCoverage1, "Cloud Coverage");
        materialEditor.ShaderProperty(_CloudBrightness, "Cloud Brightness");
        materialEditor.ShaderProperty(_CloudContrast, "Cloud Contrast");
        materialEditor.ShaderProperty(_CloudMaskingSunMoon, "Cloud Masking Sun/Moon");
        materialEditor.ShaderProperty(_CloudMain, "Cloud Main");
        materialEditor.ShaderProperty(_CloudSize, "Cloud Size");
        materialEditor.ShaderProperty(_CloudSpeed, "Cloud Speed");
        materialEditor.ShaderProperty(_CloudEdgeMask, "Cloud Edge Mask");
        materialEditor.ShaderProperty(_CloudEdgeOpacity, "Cloud Edge Opacity");
        materialEditor.ShaderProperty(_CloudEdgeSize, "Cloud Edge Size");
        materialEditor.ShaderProperty(_CloudEdgeSpeed, "Cloud Edge Speed");
        materialEditor.ShaderProperty(_CloudsHorizonGradient, "Clouds Horizon Gradient");
        materialEditor.ShaderProperty(_CloudsHorizonHeight, "Clouds Horizon Height");
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Cloud Day/Night Color Settings", new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 13, 
        });
        // Cloud Day and Night Color Settings
        EditorGUILayout.BeginVertical("box");
        materialEditor.ShaderProperty(_CloudDayColor, "Cloud Day Color");
        materialEditor.ShaderProperty(_CloudDayColorBottom, "Cloud Day Color Bottom");
        materialEditor.ShaderProperty(_CloudNightColor, "Cloud Night Color");
        materialEditor.ShaderProperty(_CloudNightColorBottom, "Cloud Night Color Bottom");
        materialEditor.ShaderProperty(_CloudBottomContrastDay1, "Cloud Bottom Contrast Day");
        materialEditor.ShaderProperty(_CloudBottomContrastNight1, "Cloud Bottom Contrast Night");
        materialEditor.ShaderProperty(_CloudBottomContrastSunset1, "Cloud Bottom Contrast Sunset");
        materialEditor.ShaderProperty(_CloudBottomMask, "Cloud Bottom Mask");
        materialEditor.ShaderProperty(_CloudBottomScale, "Cloud Bottom Scale");
        materialEditor.ShaderProperty(_CloudBottomOpacity, "Cloud Bottom Opacity");
        materialEditor.ShaderProperty(_CloudBottomBias, "Cloud Bottom Bias");
        materialEditor.ShaderProperty(_CloudBottomSize, "Cloud Bottom Size");
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Background Settings", new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 13,
        });
        // Background Settings
        EditorGUILayout.BeginVertical("box");
        materialEditor.ShaderProperty(_BackgroundPower, "Background Power");
        materialEditor.ShaderProperty(_BackgroundSize, "Background Size");
        materialEditor.ShaderProperty(_BackgroundSpeed, "Background Speed");
        materialEditor.ShaderProperty(_Background, "Background");
        materialEditor.ShaderProperty(_DayColorBackground, "Day Color Background");
        materialEditor.ShaderProperty(_NightColorBackground, "Night Color Background");
        materialEditor.ShaderProperty(_BackgroundOpacityNight, "Background Opacity Night");
        materialEditor.ShaderProperty(_BackgroundOpacityDay, "Background Opacity Day");
        materialEditor.ShaderProperty(_SunsetColorBackground, "Sunset Color Background");
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Sun Settings", new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 13,
        });
        // Sun Settings
        EditorGUILayout.BeginVertical("box");
        materialEditor.ShaderProperty(_SunDayColor, "Sun Day Color");
        materialEditor.ShaderProperty(_SunSunsetColor, "Sun Sunset Color");
        materialEditor.ShaderProperty(_CloudSunsetColor, "Cloud Sunset Color");
        materialEditor.ShaderProperty(_CloudSunsetColorBottom, "Cloud Sunset Color Bottom");
        materialEditor.ShaderProperty(_SunHaloRadius, "Sun Halo Radius");
        materialEditor.ShaderProperty(_SunsetHorizonColor, "Sunset Horizon Color");
        materialEditor.ShaderProperty(_SunHaloSize, "Sun Halo Size");
        materialEditor.ShaderProperty(_SunRadius, "Sun Radius");
        materialEditor.ShaderProperty(_SunScatteringRadius, "Sun Scattering Radius");
        materialEditor.ShaderProperty(_SunScattering, "Sun Scattering");
        materialEditor.ShaderProperty(_SunsetHorizonHeight, "Sunset Horizon Height");
        materialEditor.ShaderProperty(_SunsetSize1, "Sunset Size");
        EditorGUILayout.EndVertical();

        EditorGUILayout.LabelField("Moon Settings", new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 13,
        });

        EditorGUILayout.BeginVertical("box");
        materialEditor.ShaderProperty(_MoonDayColor, "Moon Day Color");
        materialEditor.ShaderProperty(_MoonNightColor, "Moon Night Color");
        materialEditor.ShaderProperty(_Vector5, "Vector 5");
        materialEditor.ShaderProperty(_MoonRotationAxis2, "Moon Rotation Axis");
        materialEditor.ShaderProperty(_Moon, "Moon");
        materialEditor.ShaderProperty(_MoonScattering, "Moon Scattering");
        materialEditor.ShaderProperty(_MoonScatteringRadius, "Moon Scattering Radius");
        materialEditor.ShaderProperty(_MoonSize, "Moon Size");
        EditorGUILayout.EndVertical();

        EditorGUILayout.LabelField("Stars Settings", new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 13,
        });

        // Stars Settings
        EditorGUILayout.BeginVertical("box");
        materialEditor.ShaderProperty(_StarsColor, "Stars Color");
        materialEditor.ShaderProperty(_Star, "Star");
        materialEditor.ShaderProperty(_NightSkyBrightness, "Night Sky Brightness");
        materialEditor.ShaderProperty(_StarsHorizonFade, "Stars Horizon Fade");
        materialEditor.ShaderProperty(_StarsBlinkBrightness, "Stars Blink Brightness");
        materialEditor.ShaderProperty(_StarsBlinkSpeed, "Stars Blink Speed");
        materialEditor.ShaderProperty(_StarsBlinkNoise, "Stars Blink Noise");
        materialEditor.ShaderProperty(_StarsBlinkNoiseSize, "Stars Blink Noise Size");
        materialEditor.ShaderProperty(_StarSize1, "Star Size");
        EditorGUILayout.EndVertical();

        EditorGUILayout.LabelField("Lighting and World Settings", new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 13,
        });
        // Lighting and World Settings

        EditorGUILayout.BeginVertical("box");
        materialEditor.ShaderProperty(_AutoLightDirection, "Auto Light Direction");
        materialEditor.ShaderProperty(_ManualLightDirection, "Manual Light Direction");
        materialEditor.ShaderProperty(_CullMode, "Cull Mode");
        materialEditor.ShaderProperty(_InvertWorld, "Invert World");
        materialEditor.ShaderProperty(_MirrorWorld, "Mirror World");
        EditorGUILayout.EndVertical();
    }
}
}
