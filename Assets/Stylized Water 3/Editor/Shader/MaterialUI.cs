﻿// Stylized Water 3 by Staggart Creations (http://staggart.xyz)
// COPYRIGHT PROTECTED UNDER THE UNITY ASSET STORE EULA (https://unity.com/legal/as-terms)
//    • Copying or referencing source code for the production of new asset store, or public, content is strictly prohibited!
//    • Uploading this file to a public repository will subject it to an automated DMCA takedown request.


//#define DEFAULT_GUI

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.AnimatedValues;
#if URP
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
#endif

namespace StylizedWater3
{
    public partial class MaterialUI : ShaderGUI
    {
#if URP
        private MaterialEditor materialEditor;
        
        private MaterialProperty _ZWrite;
        private MaterialProperty _ZClip;
        private MaterialProperty _Cull;
        private MaterialProperty _ShadingMode;
        private MaterialProperty _Direction;
        private MaterialProperty _Speed;
        
        private MaterialProperty _SlopeStretching;
        private MaterialProperty _SlopeSpeed;
        private MaterialProperty _SlopeAngleThreshold;
        private MaterialProperty _SlopeAngleFalloff;
        private MaterialProperty _SlopeFoam;

        private MaterialProperty _BaseColor;
        private MaterialProperty _ShallowColor;
        private MaterialProperty _ColorAbsorption;
        
        private MaterialProperty _HorizonColor;
        private MaterialProperty _HorizonDistance;
        private MaterialProperty _DepthVertical;
        private MaterialProperty _DepthHorizontal;
        private MaterialProperty _VertexColorTransparency;
        private MaterialProperty _FogSource;
        
        private MaterialProperty _WaveTint;
        private MaterialProperty _WorldSpaceUV;
        private MaterialProperty _TranslucencyStrength;
        private MaterialProperty _TranslucencyStrengthDirect;
        private MaterialProperty _TranslucencyExp;
        private MaterialProperty _TranslucencyCurvatureMask;
        private MaterialProperty _EdgeFade;
        private MaterialProperty _ShadowStrength;

        private MaterialProperty _CausticsTex;
        private MaterialProperty _CausticsBrightness;
        private MaterialProperty _CausticsChromance;
        private MaterialProperty _CausticsTiling;
        private MaterialProperty _CausticsSpeed;
        private MaterialProperty _CausticsDistortion;
        private MaterialProperty _EnableDirectionalCaustics;
        private MaterialProperty _RefractionStrength;
        private MaterialProperty _RefractionChromaticAberration;

        private MaterialProperty _UnderwaterSurfaceSmoothness;
        private MaterialProperty _UnderwaterRefractionOffset;
        private MaterialProperty _UnderwaterReflectionStrength;

        private MaterialProperty _IntersectionFoamOn;
        private MaterialProperty _IntersectionSource;
        private MaterialProperty _IntersectionSharp;
        private MaterialProperty _IntersectionNoise;
        private MaterialProperty _IntersectionColor;
        private MaterialProperty _IntersectionLength;
        private MaterialProperty _IntersectionClipping;
        private MaterialProperty _IntersectionFalloff;
        private MaterialProperty _IntersectionTiling;
        private MaterialProperty _IntersectionDistortion;
        private MaterialProperty _IntersectionRippleDist;
        private MaterialProperty _IntersectionRippleStrength;
        private MaterialProperty _IntersectionRippleSpeed;
        private MaterialProperty _IntersectionSpeed;

        private MaterialProperty _FoamTex;
        private MaterialProperty _FoamColor;
        private MaterialProperty _FoamSpeed;
        private MaterialProperty _FoamSubSpeed;
        private MaterialProperty _FoamTiling;
        private MaterialProperty _FoamSubTiling;
        private MaterialProperty _FoamDistortion;
        private MaterialProperty _FoamCrestMinMaxHeight;
        private MaterialProperty _DistanceFoamFadeDist;
        private MaterialProperty _DistanceFoamTiling;
        
        private MaterialProperty _FoamBubblesSpread;
        private MaterialProperty _FoamBubblesStrength;
        private MaterialProperty _FoamBaseAmount;
        private MaterialProperty _FoamStrength;
        private MaterialProperty _FoamClipping;
        private MaterialProperty _VertexColorFoam;

        private MaterialProperty _FoamTexDynamic;
        private MaterialProperty _FoamSpeedDynamic;
        private MaterialProperty _FoamSubSpeedDynamic;
        private MaterialProperty _FoamTilingDynamic;
        private MaterialProperty _FoamSubTilingDynamic;
        private MaterialProperty _FoamClippingDynamic;

        private MaterialProperty _BumpMap;
        private MaterialProperty _BumpMapSlope;
        private MaterialProperty _BumpMapLarge;
        private MaterialProperty _NormalTiling;
        private MaterialProperty _NormalSubTiling;
        private MaterialProperty _NormalStrength;
        private MaterialProperty _NormalSpeed;
        private MaterialProperty _NormalSubSpeed;
        private MaterialProperty _DistanceNormalsFadeDist;
        private MaterialProperty _DistanceNormalsTiling;
        private MaterialProperty _SparkleIntensity;
        private MaterialProperty _SparkleSize;

        private MaterialProperty _SunReflectionSize;
        private MaterialProperty _SunReflectionStrength;
        private MaterialProperty _SunReflectionDistortion;
        private MaterialProperty _SunReflectionSharp;
        
        private MaterialProperty _PointSpotLightReflectionStrength;
        private MaterialProperty _PointSpotLightReflectionSize;
        private MaterialProperty _PointSpotLightReflectionDistortion;
        private MaterialProperty _PointSpotLightReflectionSharp;
        
        private MaterialProperty _ReflectionStrength;
        private MaterialProperty _ReflectionDistortion;
        private MaterialProperty _ReflectionBlur;
        private MaterialProperty _ReflectionFresnel;
        private MaterialProperty _ReflectionLighting;
        private MaterialProperty _ScreenSpaceReflectionsEnabled;

        private MaterialProperty _WaveProfile;
        private MaterialProperty _WaveSpeed;
        private MaterialProperty _WaveFrequency;
        private MaterialProperty _WaveHeight;
        private MaterialProperty _VertexColorWaveFlattening;
        private MaterialProperty _WaveNormalStr;
        private MaterialProperty _WaveFadeDistance;
        private MaterialProperty _WaveSteepness;
        private MaterialProperty _WaveMaxLayers;
        
        private MaterialProperty _TessValue;
        private MaterialProperty _TessMin;
        private MaterialProperty _TessMax;

        private bool tesselationEnabled;

        private UI.Material.Section generalSection;
        private UI.Material.Section renderingSection;
        private UI.Material.Section lightingSection;
        private UI.Material.Section colorSection;
        private UI.Material.Section underwaterSection;
        private UI.Material.Section normalsSection;
        private UI.Material.Section lightReflectionSection;
        private UI.Material.Section environmentReflectionSection;
        private UI.Material.Section intersectionSection;
        private UI.Material.Section foamSection;
        private UI.Material.Section wavesSection;
        private List<UI.Material.Section> sections;

        //Keyword states
        private MaterialProperty _LightingOn;
        private MaterialProperty _ReceiveShadows;
        private MaterialProperty _FlatShadingOn;
        private MaterialProperty _TranslucencyOn;
        private MaterialProperty _RiverModeOn;
        private MaterialProperty _SpecularReflectionsOn;
        private MaterialProperty _EnvironmentReflectionsOn;
        private MaterialProperty _DisableDepthTexture;
        private MaterialProperty _CausticsOn;
        private MaterialProperty _NormalMapOn;
        private MaterialProperty _DistanceNormalsOn;
        private MaterialProperty _FoamOn;
        private MaterialProperty _FoamDistanceOn;
        private MaterialProperty _RefractionOn;
        private MaterialProperty _WavesOn;
        
        private MaterialProperty _ReceiveDynamicEffectsHeight;
        private MaterialProperty _ReceiveDynamicEffectsFoam;
        private MaterialProperty _ReceiveDynamicEffectsNormal;

        private MaterialProperty _CurvedWorldBendSettings;
        
        private GUIContent simpleShadingContent;
        private GUIContent advancedShadingContent;

        private bool initialized;
        private bool transparentShadowsEnabled;
        private bool depthAfterTransparents = false;
        private bool underwaterRenderingInstalled;
        private bool dynamicEffectsInstalled;
        
        private List<Texture2D> foamTextures;
        private List<Texture2D> normalMapTextures;
        private List<Texture2D> causticsTextures;

        private StylizedWaterRenderFeature renderFeature;
        [NonSerialized]
        private WaveProfile waveProfile;

        private WaterShaderImporter importer;
        private bool requiresRecompile;
        private string recompileMessage;
        
        private void FindProperties(MaterialProperty[] props, Material material)
        {
            tesselationEnabled = material.HasProperty("_TessValue");

            if (tesselationEnabled)
            {
                _TessValue = FindProperty("_TessValue", props);
                _TessMin = FindProperty("_TessMin", props);
                _TessMax = FindProperty("_TessMax", props);
            }

            _Cull = FindProperty("_Cull", props);
            _ZWrite = FindProperty("_ZWrite", props);
            _ZClip = FindProperty("_ZClip", props);
            _ShadingMode = FindProperty("_ShadingMode", props);
            _ShadowStrength = FindProperty("_ShadowStrength", props);
            _Direction = FindProperty("_Direction", props);
            _Speed = FindProperty("_Speed", props);

            _SlopeStretching = FindProperty("_SlopeStretching", props);
            _SlopeSpeed = FindProperty("_SlopeSpeed", props);
            _SlopeAngleThreshold = FindProperty("_SlopeAngleThreshold", props);
            _SlopeAngleFalloff = FindProperty("_SlopeAngleFalloff", props);
            _SlopeFoam = FindProperty("_SlopeFoam", props);
            
            _DisableDepthTexture = FindProperty("_DisableDepthTexture", props);
            _RefractionOn = FindProperty("_RefractionOn", props);

            _BaseColor = FindProperty("_BaseColor", props);
            _ShallowColor = FindProperty("_ShallowColor", props);
            _ColorAbsorption = FindProperty("_ColorAbsorption", props);
            //_Smoothness = FindProperty("_Smoothness", props);
            //_Metallic = FindProperty("_Metallic", props);
            _HorizonColor = FindProperty("_HorizonColor", props);
            _HorizonDistance = FindProperty("_HorizonDistance", props);
            _DepthVertical = FindProperty("_DepthVertical", props);
            _DepthHorizontal = FindProperty("_DepthHorizontal", props);
            _VertexColorTransparency = FindProperty("_VertexColorTransparency", props);
            _FogSource = FindProperty("_FogSource", props);

            _WaveTint = FindProperty("_WaveTint", props);
            _WorldSpaceUV = FindProperty("_WorldSpaceUV", props);
            _TranslucencyStrength = FindProperty("_TranslucencyStrength", props);
            _TranslucencyStrengthDirect = FindProperty("_TranslucencyStrengthDirect", props);
            _TranslucencyExp = FindProperty("_TranslucencyExp", props);
            _TranslucencyCurvatureMask = FindProperty("_TranslucencyCurvatureMask", props);
            _EdgeFade = FindProperty("_EdgeFade", props);

            _CausticsOn = FindProperty("_CausticsOn", props);
            _CausticsTex = FindProperty("_CausticsTex", props);
            _CausticsBrightness = FindProperty("_CausticsBrightness", props);
            _CausticsChromance = FindProperty("_CausticsChromance", props);
            _CausticsTiling = FindProperty("_CausticsTiling", props);
            _CausticsSpeed = FindProperty("_CausticsSpeed", props);
            _CausticsDistortion = FindProperty("_CausticsDistortion", props);
            _EnableDirectionalCaustics = FindProperty("_EnableDirectionalCaustics", props);
            _RefractionStrength = FindProperty("_RefractionStrength", props);
            _RefractionChromaticAberration = FindProperty("_RefractionChromaticAberration", props);
            
            _UnderwaterSurfaceSmoothness = FindProperty("_UnderwaterSurfaceSmoothness", props);
            _UnderwaterRefractionOffset = FindProperty("_UnderwaterRefractionOffset", props);
            _UnderwaterReflectionStrength = FindProperty("_UnderwaterReflectionStrength", props);
            
            _IntersectionSource = FindProperty("_IntersectionSource", props);
            _IntersectionSharp = FindProperty("_IntersectionSharp", props);

            _IntersectionNoise = FindProperty("_IntersectionNoise", props);
            _IntersectionColor = FindProperty("_IntersectionColor", props);
            _IntersectionLength = FindProperty("_IntersectionLength", props);
            _IntersectionClipping = FindProperty("_IntersectionClipping", props);
            _IntersectionFalloff = FindProperty("_IntersectionFalloff", props);
            _IntersectionTiling = FindProperty("_IntersectionTiling", props);
            _IntersectionDistortion = FindProperty("_IntersectionDistortion", props);
            _IntersectionRippleDist = FindProperty("_IntersectionRippleDist", props);
            _IntersectionRippleStrength = FindProperty("_IntersectionRippleStrength", props);
            _IntersectionRippleSpeed = FindProperty("_IntersectionRippleSpeed", props);
            _IntersectionSpeed = FindProperty("_IntersectionSpeed", props);
            
            _FoamTex = FindProperty("_FoamTex", props);
            _FoamColor = FindProperty("_FoamColor", props);
            _FoamSpeed = FindProperty("_FoamSpeed", props);
            _FoamSubSpeed = FindProperty("_FoamSubSpeed", props);
            _FoamTiling = FindProperty("_FoamTiling", props);
            _FoamSubTiling = FindProperty("_FoamSubTiling", props);
            _FoamDistortion = FindProperty("_FoamDistortion", props);
            _FoamBaseAmount = FindProperty("_FoamBaseAmount", props);
            _FoamStrength = FindProperty("_FoamStrength", props);
            _FoamClipping = FindProperty("_FoamClipping", props);
            _FoamCrestMinMaxHeight = FindProperty("_FoamCrestMinMaxHeight", props);
            _DistanceFoamFadeDist = FindProperty("_DistanceFoamFadeDist", props);
            _DistanceFoamTiling = FindProperty("_DistanceFoamTiling", props);
            
            _FoamBubblesSpread = FindProperty("_FoamBubblesSpread", props);
            _FoamBubblesStrength = FindProperty("_FoamBubblesStrength", props);
            _VertexColorFoam = FindProperty("_VertexColorFoam", props);
            
            _FoamTexDynamic = FindProperty("_FoamTexDynamic", props);
            _FoamSpeedDynamic = FindProperty("_FoamSpeedDynamic", props);
            _FoamSubSpeedDynamic = FindProperty("_FoamSubSpeedDynamic", props);
            _FoamTilingDynamic = FindProperty("_FoamTilingDynamic", props);
            _FoamSubTilingDynamic = FindProperty("_FoamSubTilingDynamic", props);
            _FoamClippingDynamic = FindProperty("_FoamClippingDynamic", props);

            _BumpMap = FindProperty("_BumpMap", props);
            _BumpMapSlope = FindProperty("_BumpMapSlope", props);
            _NormalTiling = FindProperty("_NormalTiling", props);
            _NormalSubTiling = FindProperty("_NormalSubTiling", props);
            _NormalStrength = FindProperty("_NormalStrength", props);
            _NormalSpeed = FindProperty("_NormalSpeed", props);
            _NormalSubSpeed = FindProperty("_NormalSubSpeed", props);

            _BumpMapLarge = FindProperty("_BumpMapLarge", props);
            _DistanceNormalsFadeDist = FindProperty("_DistanceNormalsFadeDist", props);
            _DistanceNormalsTiling = FindProperty("_DistanceNormalsTiling", props);
            
            _SparkleIntensity = FindProperty("_SparkleIntensity", props);
            _SparkleSize = FindProperty("_SparkleSize", props);

            _SunReflectionSize = FindProperty("_SunReflectionSize", props);
            _SunReflectionStrength = FindProperty("_SunReflectionStrength", props);
            _SunReflectionDistortion = FindProperty("_SunReflectionDistortion", props);
            _SunReflectionSharp = FindProperty("_SunReflectionSharp", props);
            
            _PointSpotLightReflectionStrength = FindProperty("_PointSpotLightReflectionStrength", props);
            _PointSpotLightReflectionSize = FindProperty("_PointSpotLightReflectionSize", props);
            _PointSpotLightReflectionDistortion = FindProperty("_PointSpotLightReflectionDistortion", props);
            _PointSpotLightReflectionSharp = FindProperty("_PointSpotLightReflectionSharp", props);
            
            _ReflectionStrength = FindProperty("_ReflectionStrength", props);
            _ReflectionDistortion = FindProperty("_ReflectionDistortion", props);
            _ReflectionBlur = FindProperty("_ReflectionBlur", props);
            _ReflectionFresnel = FindProperty("_ReflectionFresnel", props);
            _ReflectionLighting = FindProperty("_ReflectionLighting", props);
            _ScreenSpaceReflectionsEnabled = FindProperty("_ScreenSpaceReflectionsEnabled", props);
            
            _WaveProfile = FindProperty("_WaveProfile", props);
            _WaveSpeed = FindProperty("_WaveSpeed", props);
            _WaveHeight = FindProperty("_WaveHeight", props);
            _WaveFrequency = FindProperty("_WaveFrequency", props);
            _VertexColorWaveFlattening = FindProperty("_VertexColorWaveFlattening", props);
            _WaveNormalStr = FindProperty("_WaveNormalStr", props);
            _WaveFadeDistance = FindProperty("_WaveFadeDistance", props);
            _WaveSteepness = FindProperty("_WaveSteepness", props);
            _WaveMaxLayers = FindProperty("_WaveMaxLayers", props);

            //Keyword states
            _LightingOn = FindProperty("_LightingOn", props);
            _ReceiveShadows = FindProperty("_ReceiveShadows", props);
            _FlatShadingOn = FindProperty("_FlatShadingOn", props);
            _TranslucencyOn = FindProperty("_TranslucencyOn", props);
            _RiverModeOn = FindProperty("_RiverModeOn", props);
            _FoamOn = FindProperty("_FoamOn", props);
            _FoamDistanceOn = FindProperty("_FoamDistanceOn", props);
            _SpecularReflectionsOn = FindProperty("_SpecularReflectionsOn", props);
            _EnvironmentReflectionsOn = FindProperty("_EnvironmentReflectionsOn", props);
            _IntersectionFoamOn = FindProperty("_IntersectionFoamOn", props);
            _NormalMapOn = FindProperty("_NormalMapOn", props);
            _DistanceNormalsOn = FindProperty("_DistanceNormalsOn", props);
            _WavesOn = FindProperty("_WavesOn", props);
            
            _ReceiveDynamicEffectsHeight = FindProperty("_ReceiveDynamicEffectsHeight", props);
            _ReceiveDynamicEffectsFoam = FindProperty("_ReceiveDynamicEffectsFoam", props);
            _ReceiveDynamicEffectsNormal = FindProperty("_ReceiveDynamicEffectsNormal", props);

            if(material.HasProperty("_CurvedWorldBendSettings")) _CurvedWorldBendSettings = FindProperty("_CurvedWorldBendSettings", props);
            
            simpleShadingContent = new GUIContent("Simple", 
             "Mobile friendly");

            advancedShadingContent = new GUIContent("Advanced",
                "Advanced mode does:\n\n" +
                "• Physically-based refraction + chromatic aberration\n" +
                "• Caustics & Translucency shading for point/spot lights\n" +
                "• Caustics masked in underwater shadows" +
                "• Double sampling of water depth/fog, for accurate refraction\n" +
                "• Accurate blending of light color for translucency shading\n" +
                "• Additional texture sample for distance normals");

            if (_WaveProfile.textureValue && !waveProfile)
            {
                waveProfile = WaveProfileEditor.LoadFromLUT(_WaveProfile.textureValue);
            }
        }

        private ShaderMessage[] shaderMessages;
        private void OnEnable(MaterialEditor materialEditorIn)
        {
            sections = new List<UI.Material.Section>();
            sections.Add(generalSection = new UI.Material.Section(materialEditorIn,"GENERAL", new GUIContent("General")));
            sections.Add(renderingSection = new UI.Material.Section(materialEditorIn,"RENDERING", new GUIContent("Rendering")));
            sections.Add(lightingSection = new UI.Material.Section(materialEditorIn,"LIGHTING", new GUIContent("Lighting")));
            sections.Add(colorSection = new UI.Material.Section(materialEditorIn,"COLOR", new GUIContent("Color", "Controls for the base color of the water and transparency")));
            sections.Add(underwaterSection = new UI.Material.Section(materialEditorIn,"UNDERWATER", new GUIContent("Underwater", "Pertains the appearance of anything seen under the water surface. Not related to any actual underwater rendering")));
            sections.Add(normalsSection = new UI.Material.Section(materialEditorIn,"NORMALS", new GUIContent("Normals", "Normal maps represent the small-scale curvature of the water surface. This is used for lighting and reflections")));
            sections.Add(lightReflectionSection = new UI.Material.Section(materialEditorIn,"LIGHT_REFLECTIONS", new GUIContent("Light Reflections", "Realtime specular reflection highlight from directional, point and spot lights. ")));
            sections.Add(environmentReflectionSection = new UI.Material.Section(materialEditorIn,"ENVIRONMENT_REFLECTIONS", new GUIContent("Environment Reflections", "Reflections from reflection probes, planar- and screen-space reflections.")));
            sections.Add(foamSection = new UI.Material.Section(materialEditorIn,"FOAM", new GUIContent("Surface Foam")));
            sections.Add(intersectionSection = new UI.Material.Section(materialEditorIn,"INTERSECTION", new GUIContent("Intersection Foam", "Draws a foam effects on opaque objects that are touching the water")));
            sections.Add(wavesSection = new UI.Material.Section(materialEditorIn,"WAVES", new GUIContent("Waves", "Parametric gerstner waves, which modify the surface curvature and animate the mesh's vertices")));
            
            underwaterRenderingInstalled = StylizedWaterEditor.UnderwaterRenderingInstalled();
            dynamicEffectsInstalled = StylizedWaterEditor.DynamicEffectsInstalled();

            #if URP
            transparentShadowsEnabled = PipelineUtilities.TransparentShadowsEnabled();
            depthAfterTransparents = PipelineUtilities.IsDepthAfterTransparents();
            #endif

            Material mat = (Material)materialEditorIn.target;

            foreach (UnityEngine.Object target in materialEditorIn.targets)
            {
                MaterialChanged((Material)target);
            }

            shaderMessages = ShaderConfigurator.GetErrorMessages(mat.shader);

            importer = WaterShaderImporter.GetForShader(mat.shader);
            requiresRecompile = importer.RequiresRecompilation(out recompileMessage);
            
            string rootFolder = AssetInfo.GetRootFolder();
            LoadTextures(rootFolder + "Materials/Textures/Foam", ref foamTextures);
            //Default white texture
            foamTextures.Add(Texture2D.whiteTexture);
            LoadTextures(rootFolder + "Materials/Textures/Normals", ref normalMapTextures);
            //Default flat normal
            normalMapTextures.Add(Texture2D.normalTexture);
            LoadTextures(rootFolder + "Materials/Textures/Caustics", ref causticsTextures);
            
            renderFeature = StylizedWaterRenderFeature.GetDefault();

            initialized = true;
        }

        private void LoadTextures(string folderPath, ref List<Texture2D> collection)
        {
            string[] contentGUIDS = AssetDatabase.FindAssets("t:Texture2D", new[] { folderPath });

            collection = new List<Texture2D>();
            
            foreach (var guid in contentGUIDS)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                
                Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                
                collection.Add(tex);
            }
        }
        
        public override void OnClosed(Material material)
        {
            initialized = false;
        }

        partial void DrawDynamicEffectsUI();

        //https://github.com/Unity-Technologies/Graphics/blob/648184ec8405115e2fcf4ad3023d8b16a191c4c7/com.unity.render-pipelines.universal/Editor/ShaderGUI/BaseShaderGUI.cs
        public override void OnGUI(MaterialEditor materialEditorIn, MaterialProperty[] props)
        {
            this.materialEditor = materialEditorIn;

            materialEditor.SetDefaultGUIWidths();
            materialEditor.UseDefaultMargins();
            EditorGUIUtility.labelWidth = 0f;

            Material material = materialEditor.target as Material;

            //Requires refetching for undo/redo to function
            FindProperties(props, material);

#if DEFAULT_GUI
            base.OnGUI(materialEditor, props);
            return;
#endif
            
            if (!initialized)
            {
                OnEnable(materialEditor);
            }
            
            ShaderPropertiesGUI(material);
            
            UI.DrawFooter();
        }

        public void ShaderPropertiesGUI(Material material)
        {
            DrawHeader();
            
            DrawDynamicEffectsUI();
            
            EditorGUILayout.Space();
            
            EditorGUI.BeginChangeCheck();
            
            DrawGeneral();
            DrawRendering(material);
            DrawLighting();
            DrawColor();
            DrawNormals();
            DrawUnderwater();
            DrawFoam();
            DrawIntersection();
            DrawLightReflections();
            DrawEnvironmentReflections();
            DrawWaves();

            EditorGUILayout.Space();

            if (material.HasProperty("_CurvedWorldBendSettings"))
            {
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Curved World 2020", EditorStyles.boldLabel);
                DrawShaderProperty(_CurvedWorldBendSettings, _CurvedWorldBendSettings.displayName);
                EditorGUILayout.Space();
            }
            
            if (EditorGUI.EndChangeCheck())
            {
                foreach (var obj in  materialEditor.targets)
                    MaterialChanged((Material)obj);
            }
        }

        public override void OnMaterialInteractivePreviewGUI(MaterialEditor materialEditor, Rect r, GUIStyle background)
        {
            //base.OnMaterialInteractivePreviewGUI(materialEditor, r, background);
        }

        //Material sphere preview is mostly useless, due to simplistic rendering. Overlay an icon instead
        public override void OnMaterialPreviewGUI(MaterialEditor materialEditor, Rect rect, GUIStyle background)
        {
            GUI.DrawTexture(rect, UI.AssetIcon, ScaleMode.ScaleToFit);
        }

        void DrawNotifications()
        {
            UI.DrawNotification(!UniversalRenderPipeline.asset, "Universal Render Pipeline is currently not active!", "Show me", StylizedWaterEditor.OpenGraphicsSettings, MessageType.Error);

            if (!UniversalRenderPipeline.asset) return;
            
            if (UniversalRenderPipeline.asset && initialized)
            {
                UI.DrawNotification(
                    UniversalRenderPipeline.asset.supportsCameraDepthTexture == false &&
                    _DisableDepthTexture.floatValue == 0f,
                    "Depth texture is disabled, which is required for the material's current configuration",
                    "Enable",
                    StylizedWaterEditor.EnableDepthTexture,
                    MessageType.Error);
                
                UI.DrawNotification(
                    UniversalRenderPipeline.asset.supportsCameraOpaqueTexture == false && (_RefractionOn.floatValue == 1f || _ScreenSpaceReflectionsEnabled.floatValue > 0),
                    "Opaque texture is disabled, which is required for the material's current configuration",
                    "Enable",
                    StylizedWaterEditor.EnableOpaqueTexture,
                    MessageType.Error);
            }
            
            #if UNITY_6000_0_OR_NEWER
            if (tesselationEnabled && UniversalRenderPipeline.asset.gpuResidentDrawerMode != GPUResidentDrawerMode.Disabled)
            {
                UI.DrawNotification(true, "[Unity 6+] Using the GPU Resident Drawer with Tessellation enabled is not supported!" +
                                          "\n\nEither disable Tessellation (under the Rendering tab), or disable GPU Resident Drawer in your pipeline settings.", MessageType.Error);
            }
            #endif
            
            #if URP
            UI.DrawNotification(PipelineUtilities.RenderGraphEnabled() == false, "Render Graph is disabled in your project, some rendering functionality will not be available.", "Enable", () =>
            {
                RenderGraphSettings settings = GraphicsSettings.GetRenderPipelineSettings<RenderGraphSettings>();
                settings.enableRenderCompatibilityMode = false;
            }, MessageType.Error);
            #endif
            
            UI.DrawNotification(depthAfterTransparents && _DisableDepthTexture.floatValue < 0.5, "\nDepth Texture Mode is set to \"After Transparents\" on the default renderer\n\nWater material may not render properly\n", MessageType.Warning);
            
            if (shaderMessages != null && shaderMessages.Length > 0)
            {
                Material targetMat = (Material)materialEditor.target;
                UI.DrawNotification(shaderMessages != null, $"Shader has {shaderMessages.Length} compile errors.\n\nCheck the inspector to view them", "View", () => Selection.activeObject = targetMat.shader, MessageType.Error);
            }
            
            UI.DrawNotification(
                requiresRecompile,
                "\n" +
                "The shader requires to be recompiled." +
                "\n" 
                + recompileMessage 
                + "\n",
                "Repair",
                () =>
                {
                    importer.Reimport();
                    requiresRecompile = false;
                },
                MessageType.Warning);
        }

        private void DrawRenderFeatureNotification()
        {
            if (!renderFeature)
            {
                UI.DrawNotification(true, "The Stylized Water render feature hasn't been added to the default renderer." +
                                          "\n\nFeatures such as Directional Caustics and Screen-space Reflections are unavailable.", "Add", () =>
                {
                    PipelineUtilities.ValidateRenderFeatureSetup<StylizedWaterRenderFeature>("Stylized Water 3");
                    renderFeature = StylizedWaterRenderFeature.GetDefault();
                }, MessageType.Info);
            }
        }

        private void MaterialChanged(Material material)
        {
            if (material == null) throw new ArgumentNullException(nameof(material));

            SetMaterialKeywords(material);
            
            material.SetTexture("_CausticsTex", _CausticsTex.textureValue);
            material.SetTexture("_BumpMap", _BumpMap.textureValue);
            material.SetTexture("_BumpMapSlope", _BumpMapSlope.textureValue);
            material.SetTexture("_BumpMapLarge", _BumpMapLarge.textureValue);
            material.SetTexture("_FoamTex", _FoamTex.textureValue);
            material.SetTexture("_IntersectionNoise", _IntersectionNoise.textureValue);

            if(_WaveFrequency.hasMixedValue == false) _WaveFrequency.floatValue = Mathf.Max(_WaveFrequency.floatValue, 0.1f);
            
            if (dynamicEffectsInstalled)
            {
                material.SetTexture("_FoamTexDynamic", _FoamTexDynamic.textureValue);
            }
            
            UpgradeObsoleteProperties(material);
        }

        private void SetMaterialKeywords(Material material)
        {
#if URP
            //Keywords;
            CoreUtils.SetKeyword(material, "_ADVANCED_SHADING", material.GetFloat("_ShadingMode") == 1f);

            if (material.GetFloat("_FoamOn") > 0.5)
            {
                CoreUtils.SetKeyword(material, "_SURFACE_FOAM_SINGLE", true);
                CoreUtils.SetKeyword(material, "_SURFACE_FOAM_DUAL", material.GetFloat("_FoamDistanceOn") > 0.5);
            }
            else
            {
                CoreUtils.SetKeyword(material, "_SURFACE_FOAM_SINGLE", false);
                CoreUtils.SetKeyword(material, "_SURFACE_FOAM_DUAL", false);
            }
#endif
        }
        
        private void DrawHeader()
        {
            Rect rect = EditorGUILayout.BeginHorizontal();
            
            //Negate room made for parameter locking (material variants functionality)
            rect.xMin -= 15f;
            rect.yMin += 5f;

            GUIContent c = new GUIContent("Version " + AssetInfo.INSTALLED_VERSION);
            rect.width = EditorStyles.label.CalcSize(c).x;
            //rect.x += (rect.width * 2f);
            rect.y -= 3f;
            GUI.Label(rect, c, EditorStyles.label);

            rect.x += rect.width + 3f;
            rect.y -= 2f;
            rect.width = 16f;
            rect.height = 16f;
            
            GUI.DrawTexture(rect, EditorGUIUtility.IconContent("preAudioLoopOff").image);
            if (Event.current.type == EventType.MouseDown)
            {
                if (rect.Contains(Event.current.mousePosition) && Event.current.button == 0)
                {
                    AssetInfo.VersionChecking.GetLatestVersionPopup();
                    Event.current.Use();
                }
            }

            if (rect.Contains(Event.current.mousePosition))
            {
                Rect tooltipRect = rect;
                tooltipRect.y -= 20f;
                tooltipRect.width = 120f;
                GUI.Label(tooltipRect, "Check for update", GUI.skin.button);
            }
            
            c = new GUIContent(" Open asset window", EditorGUIUtility.IconContent("_Help").image, "Show help and third-party integrations");
            
            Rect assetWindowBtnRtc = EditorGUILayout.GetControlRect();
            assetWindowBtnRtc.width = (EditorStyles.miniLabel.CalcSize(c).x + 32f);
            assetWindowBtnRtc.x = EditorGUIUtility.currentViewWidth - assetWindowBtnRtc.width - 17f;
            assetWindowBtnRtc.height = 20f;

            if (GUI.Button(assetWindowBtnRtc, c))
            {
                HelpWindow.ShowWindow();
            }
            
            Rect tooltipBtnRtc = EditorGUILayout.GetControlRect();
            tooltipBtnRtc.width = 130f;
            tooltipBtnRtc.height = assetWindowBtnRtc.height;
            tooltipBtnRtc.x = assetWindowBtnRtc.x - assetWindowBtnRtc.width + 15f;
            
            UI.ExpandTooltips = GUI.Toggle(tooltipBtnRtc, UI.ExpandTooltips, new GUIContent(" Toggle tooltips", EditorGUIUtility.IconContent(UI.iconPrefix + (UI.ExpandTooltips ? "animationvisibilitytoggleon" : "animationvisibilitytoggleoff")).image), "Button");
            
            EditorGUILayout.EndHorizontal();
            
            GUILayout.Space(3f);
            
            DrawNotifications();

            if (importer.configurationState.fogIntegration.asset != FogIntegration.Assets.UnityFog)
            {
                EditorGUILayout.LabelField($"Active fog integration: {importer.configurationState.fogIntegration.name}" + (importer.settings.autoIntegration ? " (Automatic)" : ""), EditorStyles.miniLabel);
            }
        }
        
        #region Sections
        private void DrawGeneral()
        {
            generalSection.DrawHeader(() => SwitchSection(generalSection));
            
            if (EditorGUILayout.BeginFadeGroup(generalSection.anim.faded))
            {
                EditorGUILayout.Space();
                
                using (new EditorGUI.DisabledGroupScope(_RiverModeOn.floatValue > 0))
                {
                    DrawShaderProperty(_WorldSpaceUV, new GUIContent(_WorldSpaceUV.displayName, "Use either the mesh's UV or world-space position coordinates as a base for texture tiling"));
                }
                if(_RiverModeOn.floatValue > 0) EditorGUILayout.HelpBox("Shader will use always the mesh's UV coordinates when River Mode is enabled.", MessageType.None);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Animation", EditorStyles.boldLabel);

                UI.Material.DrawVector2(_Direction, "Direction");
                UI.Material.DrawFloatField(_Speed, label:"Speed");
                
                UI.DrawNotification(WaterObject.CustomTime > 0, $"Shader animations are driven by a custom time value set through script ({WaterObject.CustomTime}).", MessageType.Info);
                
                if (EditorWindow.focusedWindow && EditorWindow.focusedWindow.GetType() == typeof(SceneView))
                {
                    if (SceneView.lastActiveSceneView.sceneViewState.alwaysRefreshEnabled == false)
                    {
                        UI.DrawNotification("The \"Always Refresh\" option is disabled in the scene view. Water surface animations will appear to be jumpy", messageType:MessageType.None);
                    }
                }

                EditorGUILayout.Space();

                DrawShaderProperty(_RiverModeOn, new GUIContent("River Mode",
                        "When enabled, all animations flow in the vertical UV direction and stretch on slopes, creating faster flowing water." +
                        " \n\nSurface Foam also draws on slopes + A separate normal map can be used for slopes"));

                if (_RiverModeOn.floatValue > 0 || _RiverModeOn.hasMixedValue)
                {
                    DrawShaderProperty(_SlopeAngleThreshold, new GUIContent(_SlopeAngleThreshold.displayName, "Surface angle at which it is considered a slope for river-based shading"), 1);
                    DrawShaderProperty(_SlopeAngleFalloff, new GUIContent(_SlopeAngleFalloff.displayName, "Surface angle over which the slope gradient should smoothly fade out over."), 1);
                    
                    EditorGUILayout.Space();

                    DrawShaderProperty(_SlopeStretching, new GUIContent("Slope stretching", null, "On slopes, stretches textures by this much. Creates the illusion of faster flowing water"), 1);
                    DrawShaderProperty(_SlopeSpeed, new GUIContent("Slope speed", null, "On slopes, animation speed is multiplied by this value"), 1);
                }

                EditorGUILayout.Space();
            }
            EditorGUILayout.EndFadeGroup();
        }

        private void DrawRendering(Material material)
        {
            renderingSection.DrawHeader(() => SwitchSection(renderingSection));

            if (EditorGUILayout.BeginFadeGroup(renderingSection.anim.faded))
            {
                EditorGUILayout.Space();
                
                using (new EditorGUILayout.HorizontalScope())
                {
                    MaterialEditor.BeginProperty(_ShadingMode);

                    if (_ShadingMode.hasMixedValue)
                    {
                        DrawShaderProperty(_ShadingMode, advancedShadingContent);
                    }
                    else
                    {
                        EditorGUI.BeginChangeCheck();
                        EditorGUI.showMixedValue = _ShadingMode.hasMixedValue;
                        EditorGUILayout.LabelField(_ShadingMode.displayName, GUILayout.Width(EditorGUIUtility.labelWidth));

                        float shadingMode = GUILayout.Toolbar((int)_ShadingMode.floatValue, new GUIContent[] { simpleShadingContent, advancedShadingContent, }, GUILayout.MaxWidth((250f)));

                        if (EditorGUI.EndChangeCheck())
                        {
                            _ShadingMode.floatValue = shadingMode;
                        }
                        EditorGUI.showMixedValue = false;
                    }
                    
                    MaterialEditor.EndProperty();
                }

                if ((EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android ||
                     EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS ||EditorUserBuildSettings.activeBuildTarget == BuildTarget.Switch) && _ShadingMode.floatValue == 1f)
                {
                    EditorGUILayout.Space();
                    
                    UI.DrawNotification("The current shading mode is not intended to be used on mobile hardware", MessageType.Warning);
                }
                
                EditorGUILayout.Space();

                materialEditor.EnableInstancingField();

                materialEditor.RenderQueueField();
                GUILayout.Space(-3f);
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();
                    
                    if (GUILayout.Button(new GUIContent(EditorGUIUtility.IconContent(UI.iconPrefix + "Toolbar Minus")), EditorStyles.miniButtonLeft, GUILayout.MaxWidth(EditorGUIUtility.fieldWidth / 2))) material.renderQueue--;
                    if (GUILayout.Button(new GUIContent(EditorGUIUtility.IconContent(UI.iconPrefix + "Toolbar Plus")), EditorStyles.miniButtonRight, GUILayout.MaxWidth(EditorGUIUtility.fieldWidth / 2))) material.renderQueue++;
                }
                
                if (material.renderQueue <= 2450 || material.renderQueue >= 3500)
                {
                    UI.DrawNotification("Material must be on the Transparent render queue (2450-3500). Otherwise incurs rendering artefacts", MessageType.Error);
                }
                //materialEditor.DoubleSidedGIField();
                
                EditorGUILayout.Space();

                DrawShaderProperty(_Cull, new GUIContent(_Cull.displayName, "Controls which sides of the water mesh surface are rendered invisible (culled)"));
                DrawShaderProperty(_ZWrite, new GUIContent("Depth writing (ZWrite)", "Enable to have the water perform depth-based sorting on itself. Allows for intersecting transparent geometry. Advisable with high waves." +
                                                                                                "\n\nIf this is disabled, other transparent materials will either render behind or in front of the water, depending on their render queue/priority set in their materials"));
                DrawShaderProperty(_ZClip, new GUIContent("Frustum clipping (ZClip)", "Enable to clip the surface when it extends beyond the camera's far clipping plane. This is default for all shaders." +
                                                                                                 "\n\nDisable to aid in creating water that expands towards the horizon." +
                                                                                                 "\n\nNote: Effects such as edge fading and intersection foam still consider the camera's far clipping plane, this is normal."));
                
                EditorGUILayout.Space();

                DrawShaderProperty(_DisableDepthTexture, new GUIContent("Disable depth texture", "Depth texture is made available by the render pipeline and is used to measure the distance between the water surface and opaque geometry behind/in front of it, as well as their position.\n\n" +
                                                                                                            "This is used for a variety of effects, such as the color gradient, intersection effects, caustics and refraction." +
                                                                                                            "\n\nDisable if targeting a bare bones rendering set up without a depth pre-pass present."));

                EditorGUILayout.Space();
                
                EditorGUI.BeginChangeCheck();
                var tessellationTooltip = "Dynamically subdivides the mesh's triangles to create denser topology near the camera." +
                                          "\n\nThis allows for more detailed wave animations." +
                                          "\n\nOnly supported on GPUs with Shader Model 4.6+ and the Metal graphics API on Mac/iOS. Should it fail, it will fall back to the non-tessellated shader automatically";
                tesselationEnabled = EditorGUILayout.Toggle(new GUIContent("Tessellation", tessellationTooltip), tesselationEnabled);
                if(UI.ExpandTooltips) EditorGUILayout.HelpBox(tessellationTooltip, MessageType.None);
                
                if (EditorGUI.EndChangeCheck())
                {
                    foreach (UnityEngine.Object target in materialEditor.targets)
                    {
                        Material mat = (Material)target;
                        bool usingTessellation = mat.shader.name.Contains(ShaderParams.ShaderNames.TESSELLATION_NAME_SUFFIX);

                        string newShaderName = mat.shader.name;

                        if (usingTessellation)
                        {
                            newShaderName = mat.shader.name.Replace(ShaderParams.ShaderNames.TESSELLATION_NAME_SUFFIX, string.Empty);
                        }
                        else
                        {
                            newShaderName += ShaderParams.ShaderNames.TESSELLATION_NAME_SUFFIX;
                        }
                        
                        #if SWS_DEV
                        //Debug.Log("Switching to shader: " + newShaderName);
                        #endif
                        
                        Shader newShader = Shader.Find(newShaderName);
                        if(newShader) AssignNewShaderToMaterial(material, material.shader, newShader);
                        #if SWS_DEV
                        else
                        {
                             Debug.Log("Failed to find tessellation shader with name: " + newShaderName);
                        }
                        #endif
                    }
                }
                
                if (tesselationEnabled && _TessValue != null)
                {
                    UI.DrawNotification(_FlatShadingOn.floatValue > 0 || _FlatShadingOn.hasMixedValue, "Flat shading is enabled, tessellation should not be used to achieve the desired effect", MessageType.Warning);
                    
                    EditorGUI.indentLevel++;

                    DrawShaderProperty(_TessValue, _TessValue.displayName);
                    #if UNITY_PS4 || UNITY_XBOXONE || UNITY_GAMECORE
                    //AMD recommended performance optimization
                    EditorGUILayout.HelpBox("Value is internally limited to 15 for the current target platform (AMD-specific optimization)", MessageType.None);
                    #endif
                    UI.Material.DrawFloatField(_TessMin);
                    _TessMin.floatValue = Mathf.Clamp(_TessMin.floatValue, 0f, _TessMax.floatValue - 0.01f);
                    UI.Material.DrawFloatField(_TessMax);
                    EditorGUI.indentLevel--;
                    
                    UI.DrawNotification(material.enableInstancing, "Tessellation does not work correctly when GPU instancing is enabled", MessageType.Warning);
                }
                
                EditorGUILayout.Space();

                if (dynamicEffectsInstalled)
                {
                    EditorGUILayout.LabelField("Apply Dynamic Effects", EditorStyles.boldLabel);

                    if (UI.ExpandTooltips)
                    {
                        EditorGUILayout.HelpBox("This functionality is specific to the Dynamic Effects extension", MessageType.None);
                    }

                    DrawShaderProperty(_ReceiveDynamicEffectsHeight, new GUIContent("Height", "Specify if this material should apply dynamic effects displacement to itself."));
                    DrawShaderProperty(_ReceiveDynamicEffectsFoam, new GUIContent("Foam"));
                    DrawShaderProperty(_ReceiveDynamicEffectsNormal, new GUIContent("Normals"));

                    EditorGUILayout.Space();
                }
            }
            EditorGUILayout.EndFadeGroup();
        }

        private void DrawLighting()
        {
            lightingSection.DrawHeader(() => SwitchSection(lightingSection));

            if (EditorGUILayout.BeginFadeGroup(lightingSection.anim.faded))
            {
                EditorGUILayout.Space();
                
                DrawShaderProperty(_LightingOn, new GUIContent("Enable lighting", "Color from lights and ambient light (Flat/Gradient/Skybox) will affect the material. If using overall Unlit shaders in the scene, or fixed lighting, disable to skip lighting calculations."));

                DrawShaderProperty(_FlatShadingOn, new GUIContent("Flat/low-poly shading", "When enabled, normals are calculated per mesh face, resulting in a faceted appearance (low poly look). The mesh needs sufficient vertices to really sell the effect (eg. a quad mesh won't do)"));
                UI.DrawNotification(_FlatShadingOn.floatValue > 0 && tesselationEnabled, "Tessellation is enabled, it should not be used to achieve the desired effect", MessageType.Warning);

                UI.DrawNotification(_FlatShadingOn.floatValue > 0f && _WavesOn.floatValue == 0f, "Flat shading has little effect if waves are disabled", MessageType.Warning);

                DrawShaderProperty(_ReceiveShadows, new GUIContent("Receive shadows", "Allows the material to receive realtime shadows from other objects.\n\nAlso enables light-based effects such as reflections and caustics to hide themselves in shadows."));
                if ((_ReceiveShadows.floatValue > 0 || _ReceiveShadows.hasMixedValue) && !transparentShadowsEnabled && _ShadingMode.floatValue != 0)
                {
                    #if URP
                    transparentShadowsEnabled = PipelineUtilities.TransparentShadowsEnabled();
                    #endif
                }
                UI.DrawNotification((_ReceiveShadows.floatValue > 0 || _ReceiveShadows.hasMixedValue) && !transparentShadowsEnabled,
                    "Transparent shadows are disabled in the default Forward renderer", "Show me",
                    StylizedWaterEditor.SelectForwardRenderer, MessageType.Warning);
                
                using (new EditorGUI.DisabledScope(_LightingOn.floatValue < 1f || _LightingOn.hasMixedValue))
                {
                    if ((_ReceiveShadows.floatValue > 0 || _ReceiveShadows.hasMixedValue))
                    {
                        DrawShaderProperty(_ShadowStrength, "Strength", 1);
                    }
                    
                    DrawShaderProperty(_NormalStrength, new GUIContent("Diffuse lighting", "Controls how much the curvature of the normal map affects directional lighting"));
                }

                EditorGUILayout.Space();

                using (new EditorGUI.DisabledScope(_NormalMapOn.floatValue == 0f))
                {
                    EditorGUILayout.LabelField("Sparkles", EditorStyles.boldLabel);
                    DrawShaderProperty(_SparkleIntensity, new GUIContent("Intensity", "The color/intensity of the main directional light is multiplied on top of this."));
                    DrawShaderProperty(_SparkleSize, "Size");
                }
                UI.DrawNotification(_NormalMapOn.floatValue == 0f, "Sparkles require the normal map feature to be enabled", MessageType.None);
                
                EditorGUILayout.Space();

                DrawShaderProperty(_TranslucencyOn, new GUIContent("Translucency", "Creates the appearance of sun light passing through the water and scattering.\n\nNote that this is mostly visible at grazing light angle"));

                if (_TranslucencyOn.floatValue > 0 || _TranslucencyOn.hasMixedValue)
                {
                    DrawShaderProperty(_TranslucencyStrength, new GUIContent("Intensity", "Acts as a multiplier for the light's intensity"), 1);
                    DrawShaderProperty(_TranslucencyExp, new GUIContent("Exponent", "Essentially controls the width/scale of the effect"), 1);
                    DrawShaderProperty(_TranslucencyCurvatureMask, new GUIContent("Curvature mask", "Masks the effect by the orientation of the surface. Surfaces facing away from the sun will receive less of an effect. On sphere mesh, this would push the effect towards the edges/silhouette."), 1);
                    
                    EditorGUILayout.Space();

                    DrawShaderProperty(_TranslucencyStrengthDirect, new GUIContent("Direct Light Intensity", "Simulate light scattering from direct sun light. Typically seen in glacial lakes."), 1);
                }
                
                EditorGUILayout.Space();
            }
            EditorGUILayout.EndFadeGroup();
        }

        private void DrawColor()
        {
            colorSection.DrawHeader(() => SwitchSection(colorSection));

            if (EditorGUILayout.BeginFadeGroup(colorSection.anim.faded))
            {
                EditorGUILayout.Space();

                UI.Material.DrawColorField(_BaseColor, true, _BaseColor.displayName, "Base water color, alpha channel controls transparency");
                UI.Material.DrawColorField(_ShallowColor, true, _ShallowColor.displayName, "Water color in shallow areas, alpha channel controls transparency. Note that the caustics effect is visible here, setting the alpha to 100% hides caustics");
                if (_ShadingMode.floatValue == 1 || _ShadingMode.hasMixedValue) //Advanced shading
                {
                    using (new EditorGUI.DisabledGroupScope(_RefractionOn.floatValue == 0 && !_RefractionOn.hasMixedValue))
                    {
                        DrawShaderProperty(_ColorAbsorption, new GUIContent(_ColorAbsorption.displayName, "Darkens the underwater color, based on the water's depth. This is a particular physical property of water that contributes to a realistic appearance."));
                    }
                    if (_RefractionOn.floatValue == 0) EditorGUILayout.HelpBox("Requires the Refraction feature to be enabled", MessageType.None);
                    
                    EditorGUILayout.Space();
                }
                
                //DrawShaderProperty(_Smoothness);
                //DrawShaderProperty(_Metallic);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Fog/Density", EditorStyles.boldLabel);
                DrawShaderProperty(_FogSource, new GUIContent("Depth source", "The Green vertex color channel subtracts (visual) depth from the water, making it appear shallow. When River Mode is enabled, this controls the complete opacity of the material instead"));
                if (_FogSource.floatValue == 0 || _FogSource.hasMixedValue)
                {
                    if (_DisableDepthTexture.floatValue > 0)
                    {
                        UI.DrawNotification("Depth texture is disabled", MessageType.Warning);
                    }
                    using (new EditorGUI.DisabledGroupScope(_DisableDepthTexture.floatValue == 1f && !_DisableDepthTexture.hasMixedValue))
                    {
                        DrawShaderProperty(_DepthVertical, new GUIContent("Distance Depth", "Distance measured from the water surface, to the geometry behind it, along the camera's viewing angle. Water turns denser the more the camera looks along the water surface, and through it."));
                        DrawShaderProperty(_DepthHorizontal, new GUIContent("Vertical Depth", "Density as measured from the water surface, straight down. This acts as a type of artificial height fog."));
                    }   
                }
                
                EditorGUILayout.Space();

                DrawShaderProperty(_VertexColorTransparency, new GUIContent("Vertex color transparency (G)", "The Green vertex color channel adds transparency to the water, making it appear invisible."));

                using (new EditorGUI.DisabledGroupScope(_DisableDepthTexture.floatValue == 1f && !_DisableDepthTexture.hasMixedValue))
                {
                    UI.Material.DrawFloatField(_EdgeFade, "Edge fading", "Fades out the water where it intersects with opaque geometry.\n\nRequires the depth texture option to be enabled");
                    _EdgeFade.floatValue = Mathf.Max(0f, _EdgeFade.floatValue);
                }
                EditorGUILayout.Space();

                UI.Material.DrawColorField(_HorizonColor, true, _HorizonColor.displayName, "Color as perceived on the horizon, where looking across the water");
                DrawShaderProperty(_HorizonDistance, _HorizonDistance.displayName);

                DrawShaderProperty(_WaveTint, new GUIContent(_WaveTint.displayName, "Adds a bright/dark tint based on wave height\n\nWaves feature must be enabled"));

                EditorGUILayout.Space();
            }
            EditorGUILayout.EndFadeGroup();
        }

        private void DrawNormals()
        {
            normalsSection.DrawHeader(() => SwitchSection(normalsSection));

            if (EditorGUILayout.BeginFadeGroup(normalsSection.anim.faded))
            {
                EditorGUILayout.Space();

                DrawShaderProperty(_NormalMapOn,  new GUIContent("Enable", "Normals add small-scale detail curvature to the water surface, which in turn is used in various lighting techniques"));
                
                EditorGUILayout.Space();

                if (_NormalMapOn.floatValue > 0f || _NormalMapOn.hasMixedValue)
                {
                    DrawTextureSelector(_BumpMap, ref normalMapTextures);

                    if (_RiverModeOn.floatValue > 0f || _RiverModeOn.hasMixedValue)
                    {
                        DrawTextureSelector(_BumpMapSlope, ref normalMapTextures);
                    }
                    
                    EditorGUILayout.LabelField("Tiling & Offset", EditorStyles.boldLabel);
                    //UI.Material.DrawFloatTicker(_NormalTiling, tooltip:"Determines how often the texture repeats over the UV coordinates. Smaller values result in the texture being stretched larger, higher numbers means it becomes smaller");
                    UI.Material.DrawVector2Ticker(_NormalTiling, "Tiling");

                    EditorGUI.indentLevel++;
                        UI.Material.DrawFloatTicker(_NormalSubTiling, "Sub-layer (multiplier)", "The effect uses a 2nd texture sample, for variety. This value controls the speed of this layer");
                    EditorGUI.indentLevel--;
                    UI.Material.DrawFloatTicker(_NormalSpeed, tooltip:"[Multiplied by the animation speed set under the General tab]\n\nControls how fast the texture moves in the animation direction. A negative value (-) makes it move in the opposite direction", showReverse:true);
                    EditorGUI.indentLevel++;
                        UI.Material.DrawFloatTicker(_NormalSubSpeed, "Sub-layer (multiplier)", tooltip: "Multiplier for the 2nd texture sample.", showReverse:true);
                    EditorGUI.indentLevel--;
                    if (_RiverModeOn.floatValue > 0 && _NormalSubSpeed.floatValue < 0)
                    {
                        EditorGUILayout.HelpBox("River Mode is enabled, negative speed values create upstream animations", MessageType.None);
                    }
                    
                    EditorGUILayout.Space();

                    DrawShaderProperty(_DistanceNormalsOn, new GUIContent("Distance normals", "Resamples normals in the distance, at a larger scale. At the cost some additional shading calculations, tiling artifacts can be greatly reduced"));

                    if (_DistanceNormalsOn.floatValue > 0 || _DistanceNormalsOn.hasMixedValue)
                    {
                        DrawTextureSelector(_BumpMapLarge, ref normalMapTextures);

                        UI.Material.DrawFloatTicker(_DistanceNormalsTiling, "Tiling");

                        UI.Material.DrawMinMaxSlider(_DistanceNormalsFadeDist, 0f, 500, "Blend distance range", tooltip:"Min/max distance range (from the camera) the effect should to blend in");
                    }
                }

                EditorGUILayout.Space();
            }
            EditorGUILayout.EndFadeGroup();
        }

        private void DrawUnderwater()
        {
            underwaterSection.DrawHeader(() => SwitchSection(underwaterSection));

            if (EditorGUILayout.BeginFadeGroup(underwaterSection.anim.faded))
            {
                EditorGUILayout.Space();

                DrawShaderProperty(_CausticsOn, new GUIContent("Caustics", "Caustics are normally a complex optical effect, created by light passing through a surface and refracting." +
                                                                           "\n\nA static caustics texture can be used to approximate this effect by projecting it onto the opaque geometry behind the water surface." +
                                                                           "\n\nIf Advanced shading is enabled, point- and spot lights also create this effect."));
                
                if (_CausticsOn.floatValue == 1 || _CausticsOn.hasMixedValue)
                {
                    DrawTextureSelector(_CausticsTex, ref causticsTextures);

                    UI.Material.DrawFloatField(_CausticsBrightness, "Brightness", "The intensity of the incoming light controls how strongly the effect is visible. This parameter acts as a multiplier.");
                    if(!_CausticsBrightness.hasMixedValue) _CausticsBrightness.floatValue = Mathf.Max(0, _CausticsBrightness.floatValue);
                    
                    DrawShaderProperty(_EnableDirectionalCaustics, new GUIContent("Directional Projection", "Projects the effect from the main directional light's direction. Feature needs to also be enabled on the render feature (default)." +
                                                                                                            "\n\nThis involves reconstructing the underwater geometry's normal, using 3 additional depth texture samples. Disable when performance is critical"));
                    DrawRenderFeatureNotification();

                    using (new EditorGUI.DisabledGroupScope((_DisableDepthTexture.floatValue == 1f && _CausticsOn.floatValue == 1f)))
                    {
                        if (renderFeature && _EnableDirectionalCaustics.floatValue > 0)
                        {
                            UI.DrawNotification(renderFeature.allowDirectionalCaustics == false, "Directional caustics are disabled on the render feature", MessageType.Warning);
                        }
                    }
                    
                    EditorGUILayout.Separator();
                    
                    DrawShaderProperty(_CausticsChromance, new GUIContent(_CausticsChromance.displayName, "Blends between grayscale and RGB caustics"));
                    DrawShaderProperty(_CausticsDistortion, new GUIContent(_CausticsDistortion.displayName, "Distort the caustics based on the normal map"));
                    
                    EditorGUILayout.Space();
                    
                    UI.Material.DrawFloatTicker(_CausticsTiling);
                    UI.Material.DrawFloatTicker(_CausticsSpeed);
                }
                if (_FogSource.floatValue == 1f && _CausticsOn.floatValue == 1f)
                {
                    UI.DrawNotification("Caustics project on the water surface itself, because the \"Disable depth texture\" option is enabled.", MessageType.None);
                    
                    UI.DrawNotification(_DisableDepthTexture.floatValue == 1 && !_DisableDepthTexture.hasMixedValue, "\nDepth texture is disabled, so water has no means of creating shallow water. Caustics will not seem visible.\n\nEnable the use of vertex color opacity to manually paint shallow water.\n", "Enable", () => _FogSource.floatValue = 1);
                }

                EditorGUILayout.Space();

                DrawShaderProperty(_RefractionOn, new GUIContent("Refraction", "Simulates how the surface behind the water appears distorted, because the light passes through the water's curvy surface"));

                if (_RefractionOn.floatValue == 1f || _RefractionOn.hasMixedValue)
                {
                    if (UniversalRenderPipeline.asset)
                    {
                        UI.DrawNotification(UniversalRenderPipeline.asset.opaqueDownsampling != Downsampling.None, "Opaque texture is rendering at a lower resolution, water may appear blurry");
                    }
                    
                    if (_NormalMapOn.floatValue == 0f && _WavesOn.floatValue == 0f)
                    {
                        UI.DrawNotification("Refraction will have little effect if normals and waves are disabled", MessageType.Warning);
                    }
                    
                    DrawShaderProperty(_RefractionStrength, new GUIContent("Strength", "Note: Distortion strength is influenced by the strength of the normal map texture"), 1);
                    if (_ShadingMode.floatValue == 1f || _ShadingMode.hasMixedValue)
                    {
                        DrawShaderProperty(_RefractionChromaticAberration, new GUIContent("Chromatic Aberration (Max)", 
                            "Creates a prism-like rainbow effect where the refraction is the strongest. Controls the maximum offset, and is based on refraction strength (both the parameter and the context)\n\nCan create some discrepancies in the underwater fog!"));
                    }
                }
                else
                {
                    if (underwaterRenderingInstalled && _ShadingMode.floatValue > 0.5)
                    {
                        UI.DrawNotification("[Underwater Rendering] It's recommended to keep Refraction enabled for correct shading of geometry above the water surface.", MessageType.Warning);
                    }
                }
                
                if (underwaterRenderingInstalled)
                {
                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("Underwater Surface Rendering", EditorStyles.boldLabel);
                    DrawShaderProperty(_UnderwaterSurfaceSmoothness, new GUIContent("Surface Smoothness", "Controls how distorted everything above the water appears from below"));
                    DrawShaderProperty(_UnderwaterRefractionOffset, new GUIContent("Refraction offset", "Creates a wide \"circle\" of visible air above the camera. Pushes it further away from the camera"));
                    DrawShaderProperty(_UnderwaterReflectionStrength, new GUIContent("Reflection Strength", "Visibility of the rendered reflections"));
                }

                EditorGUILayout.Space();
            }
            EditorGUILayout.EndFadeGroup();
        }

        private void DrawFoam()
        {
            foamSection.DrawHeader(() => SwitchSection(foamSection));
            
            if (EditorGUILayout.BeginFadeGroup(foamSection.anim.faded))
            {
                EditorGUILayout.Space();
                
                DrawShaderProperty(_FoamOn, new GUIContent("Enable", "Draws a cross-animated foam texture on the water surface"));
                if (_FoamOn.floatValue > 0 || _FoamOn.hasMixedValue)
                {
                    UI.Material.DrawColorField(_FoamColor, true, "Color", "Color of the foam, the alpha channel controls opacity");
                    
                    DrawTextureSelector(_FoamTex, ref foamTextures);

                    EditorGUILayout.LabelField("Application", EditorStyles.boldLabel);
                    DrawShaderProperty(_VertexColorFoam, new GUIContent("Vertex color painting (A)",
                        "Enable the usage of the vertex color Alpha channel to add foam"));
                    
                    DrawShaderProperty(_FoamBaseAmount, new GUIContent("Base amount", "Adds a uniform amount of foam"));
                    
                    if (_RiverModeOn.floatValue > 0 || _RiverModeOn.hasMixedValue)
                    {
                        DrawShaderProperty(_SlopeFoam, new GUIContent(_SlopeFoam.displayName, "Control the amount of Surface Foam that draws on slopes"));
                    }

                    DrawShaderProperty(_FoamCrestMinMaxHeight, new GUIContent(_FoamCrestMinMaxHeight.displayName, "Add foam between a min and max wave height"));
                    
                    DrawShaderProperty(_FoamClipping, new GUIContent("Clipping", "Gradually cuts off the texture, based on its gradient"));
                    DrawShaderProperty(_FoamStrength, new GUIContent("Strength", "Scales the amount of foam"));
                    
                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("Bubbles", EditorStyles.boldLabel);

                    DrawShaderProperty(_FoamBubblesSpread, new GUIContent("Spread", "Blends in the GREEN channel of the foam texture, which is a blurred version of the foam map"));
                    DrawShaderProperty(_FoamBubblesStrength, new GUIContent("Strength", "Blends in the GREEN channel of the foam texture, which is a blurred version of the foam map"));
                    
                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("Tiling & Offset", EditorStyles.boldLabel);
                    //UI.Material.DrawFloatTicker(_FoamTiling, tooltip:"Determines how often the texture repeats over the UV coordinates. Smaller values result in the texture being stretched larger, higher numbers means it becomes smaller");
                    UI.Material.DrawVector2Ticker(_FoamTiling, "Tiling");
                    EditorGUI.indentLevel++;
                    UI.Material.DrawFloatTicker(_FoamSubTiling, "Sub-layer (multiplier)", "The effect uses a 2nd texture sample, for variety. This value controls the speed of this layer");
                    EditorGUI.indentLevel--;
                    UI.Material.DrawFloatTicker(_FoamSpeed, tooltip:"[Multiplied by the animation speed set under the General tab]\n\nControls how fast the texture moves in the animation direction. A negative value (-) makes it move in the opposite direction", showReverse:true);
                    EditorGUI.indentLevel++;
                    UI.Material.DrawFloatTicker(_FoamSubSpeed, "Sub-layer (multiplier)", tooltip:"Multiplier for the 2nd texture sample.", showReverse:true);
                    EditorGUI.indentLevel--;
                    if (_RiverModeOn.floatValue > 0 && _FoamSubSpeed.floatValue < 0)
                    {
                        EditorGUILayout.HelpBox("River Mode is enabled, negative speed values create upstream animations", MessageType.None);
                    }
                    EditorGUILayout.Space();
                    
                    EditorGUILayout.LabelField("Distance foam", EditorStyles.boldLabel);

                    DrawShaderProperty(_FoamDistanceOn, new GUIContent("Enable", "Blends in a 2nd layer of surface foam, visible within the configured viewing range. Mitigates visible tiling at the cost of more shading calculations"));

                    if (_FoamDistanceOn.floatValue > 0 || _FoamDistanceOn.hasMixedValue)
                    {
                        DrawShaderProperty(_DistanceFoamFadeDist, "Fade start/end");
                        UI.Material.DrawFloatTicker(_DistanceFoamTiling, "Tiling multiplier", tooltip:"Determines how often the texture repeats over the UV coordinates. Smaller values result in the texture being stretched larger, higher numbers means it becomes smaller");
                    }

                    EditorGUILayout.Space();
                    
                    if (dynamicEffectsInstalled && (_ReceiveDynamicEffectsFoam.floatValue > 0.5 || _ReceiveDynamicEffectsFoam.hasMixedValue))
                    {
                        EditorGUILayout.LabelField("Dynamic Effects", EditorStyles.boldLabel);
                        
                        DrawTextureSelector(_FoamTexDynamic, ref foamTextures);
                        DrawShaderProperty(_FoamClippingDynamic, new GUIContent("Clipping", "Gradually cuts off the texture, based on its gradient"));

                        EditorGUILayout.Separator();
                        
                        UI.Material.DrawFloatTicker(_FoamTilingDynamic, tooltip:"Determines how often the texture repeats over the UV coordinates. Smaller values result in the texture being stretched larger, higher numbers means it becomes smaller");
                        EditorGUI.indentLevel++;
                        UI.Material.DrawFloatTicker(_FoamSubTilingDynamic, "Sub-layer (multiplier)", "The effect uses a 2nd texture sample, for variety. This value controls the speed of this layer");
                        EditorGUI.indentLevel--;
                        UI.Material.DrawFloatTicker(_FoamSpeedDynamic, tooltip:"[Multiplied by the animation speed set under the General tab]\n\nControls how fast the texture moves in the animation direction. A negative value (-) makes it move in the opposite direction", showReverse:true);
                        EditorGUI.indentLevel++;
                        UI.Material.DrawFloatTicker(_FoamSubSpeedDynamic, "Sub-layer (multiplier)", tooltip:"Multiplier for the 2nd texture sample.", showReverse:true);
                        EditorGUI.indentLevel--;
                    }

                    EditorGUILayout.Space();

                    DrawShaderProperty(_FoamDistortion, new GUIContent(_FoamDistortion.displayName, "Distorts the foam by the amount of vertical displacement, such as that created by waves"));
                }

                EditorGUILayout.Space();
            }
            EditorGUILayout.EndFadeGroup();
        }

        private void DrawIntersection()
        {
            intersectionSection.DrawHeader(() => SwitchSection(intersectionSection));

            if (EditorGUILayout.BeginFadeGroup(intersectionSection.anim.faded))
            {
                EditorGUILayout.Space();
                
                DrawShaderProperty(_IntersectionFoamOn, new GUIContent("Enable", "Draws an animated foam effect on the geometry where it intersects with the water surface."));
                
                if (_IntersectionFoamOn.floatValue > 0 || _IntersectionFoamOn.hasMixedValue)
                {
                    EditorGUILayout.Space();
                    
                    DrawShaderProperty(_IntersectionSource, new GUIContent("Gradient source", null, "The effect requires a linear gradient to work with, something that represents the distance from the intersection point out towards the water." +
                                                                                                    "\n\nThis parameter control what's being used as the source to approximate this information."));
                    DrawShaderProperty(_IntersectionLength, new GUIContent(_IntersectionLength.displayName, "Distance from objects/shore"));
                    DrawShaderProperty(_IntersectionFalloff, new GUIContent(_IntersectionFalloff.displayName, "The falloff represents a gradient"));
                    
                    EditorGUILayout.Separator();
                    
                    DrawShaderProperty(_IntersectionSharp, new GUIContent(_IntersectionSharp.displayName));
                    if (_IntersectionSharp.floatValue == 1f || _IntersectionSharp.hasMixedValue)
                    {
                        EditorGUI.indentLevel++;
                        DrawShaderProperty(_IntersectionClipping, new GUIContent(_IntersectionClipping.displayName, "Clips the effect based on the texture's gradient."));
                        EditorGUI.indentLevel--;
                    }
                    
                    EditorGUILayout.Separator();
                    
                    if (_IntersectionSource.floatValue == 0 && _DisableDepthTexture.floatValue == 1f)
                    {
                        UI.DrawNotification("The depth texture option is disabled in the Rendering tab",
                            MessageType.Error);
                    }

                    
                    materialEditor.TextureProperty(_IntersectionNoise, "Texture (R=Noise)");
                    UI.Material.DrawColorField(_IntersectionColor, true, "Color", "Alpha channel controls the opacity");
                    
                    EditorGUILayout.Separator();
                    
                    EditorGUILayout.LabelField("Noise Tiling & Speed", EditorStyles.boldLabel);

                    UI.Material.DrawFloatTicker(_IntersectionTiling, "Tiling");
                    UI.Material.DrawFloatTicker(_IntersectionSpeed, "Speed", "This value is multiplied by the Animation Speed value in the General tab");

                    EditorGUILayout.Separator();

                    if (_NormalMapOn.floatValue > 0 || _NormalMapOn.hasMixedValue)
                    {
                        DrawShaderProperty(_IntersectionDistortion, new GUIContent("Distortion", "Offset the texture sample by the normal map"));
                    }
                    
                    EditorGUILayout.Separator();
                    
                    EditorGUILayout.LabelField("Ripples", EditorStyles.boldLabel);
                    UI.Material.DrawFloatTicker(_IntersectionRippleDist, "Frequency", "Distance between each ripples over the total intersection length");
                    UI.Material.DrawFloatTicker(_IntersectionRippleSpeed,"Speed", "Speed at which the ripples move");
                    DrawShaderProperty(_IntersectionRippleStrength, new GUIContent("Strength", "Sets how much the ripples should be blended in with the effect"));
                }

                EditorGUILayout.Space();
            }
            EditorGUILayout.EndFadeGroup();
        }

        private void DrawLightReflections()
        {
            lightReflectionSection.DrawHeader(() => SwitchSection(lightReflectionSection));

            if (EditorGUILayout.BeginFadeGroup(lightReflectionSection.anim.faded))
            {
                EditorGUILayout.Space();

                DrawShaderProperty(_SpecularReflectionsOn, new GUIContent("Enable", 
                    "Creates a specular reflection based on the relationship between the light-, camera and water surface angle." +
                    "\n\nA combination between the Size and Distortion parameter can achieve different visual styles"));
                
                if (_SpecularReflectionsOn.floatValue > 0f || _SpecularReflectionsOn.hasMixedValue)
                {
                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("Directional Light", EditorStyles.boldLabel);

                    DrawShaderProperty(_SunReflectionStrength, new GUIContent("Strength", "This value is multiplied over the sun light's intensity"));
                    if (UniversalRenderPipeline.asset)
                    {
                        if(UniversalRenderPipeline.asset.supportsHDR == false) EditorGUILayout.HelpBox("Note: HDR is disabled on the current pipeline asset", MessageType.None);
                    }
                    if(!_SunReflectionStrength.hasMixedValue) _SunReflectionStrength.floatValue = Mathf.Max(0, _SunReflectionStrength.floatValue);
                    
                    DrawShaderProperty(_SunReflectionSize, new GUIContent("Size", "Determines how wide the reflection appears"));
                    DrawShaderProperty(_SunReflectionSharp, new GUIContent("Sharp", "Tightens the reflection towards a hard edge"));
                    DrawShaderProperty(_SunReflectionDistortion, new GUIContent("Distortion", "Distortion is largely influenced by the strength of the normal map texture and wave curvature"));

                    if (_LightingOn.floatValue > 0f || _LightingOn.hasMixedValue)
                    {
                        EditorGUILayout.Space();

                        EditorGUILayout.LabelField("Point & Spot lights", EditorStyles.boldLabel);

                        DrawShaderProperty(_PointSpotLightReflectionStrength, new GUIContent("Strength", "This value is multiplied over the light's intensity"));
                        if (UniversalRenderPipeline.asset)
                        {
                            if(UniversalRenderPipeline.asset.supportsHDR == false) EditorGUILayout.HelpBox("Note: HDR is disabled on the current pipeline asset", MessageType.None);
                        }
                        if(!_PointSpotLightReflectionStrength.hasMixedValue) _PointSpotLightReflectionStrength.floatValue = Mathf.Max(0, _PointSpotLightReflectionStrength.floatValue);
                        
                        DrawShaderProperty(_PointSpotLightReflectionSize, new GUIContent("Size", "Specular reflection size for point/spot lights"));
                        DrawShaderProperty(_PointSpotLightReflectionSharp, new GUIContent("Sharp", "Tightens the reflection towards a hard edge"));
                        DrawShaderProperty(_PointSpotLightReflectionDistortion, new GUIContent("Distortion", "Distortion is largely influenced by the strength of the normal map texture and wave curvature"));
                    }
                }

                EditorGUILayout.Space();
            }
            EditorGUILayout.EndFadeGroup();
        }
        
        private void DrawEnvironmentReflections()
        {
            environmentReflectionSection.DrawHeader(() => SwitchSection(environmentReflectionSection));

            if (EditorGUILayout.BeginFadeGroup(environmentReflectionSection.anim.faded))
            {
                EditorGUILayout.Space();

                DrawShaderProperty(_EnvironmentReflectionsOn, new GUIContent("Enable", "Enable reflections from the skybox, reflection probes, screen-space- and planar -reflections."));
                
                var customReflection = RenderSettings.customReflectionTexture;

                if (_EnvironmentReflectionsOn.floatValue > 0 && RenderSettings.defaultReflectionMode == DefaultReflectionMode.Custom && !customReflection)
                {
                    UI.DrawNotification("Lighting settings: Environment reflections source is set to \"Custom\" without a cubemap assigned. No reflections may be visible", MessageType.Warning);
                }
                
                UI.DrawNotification(_EnvironmentReflectionsOn.floatValue > 0 && QualitySettings.realtimeReflectionProbes == false && PlanarReflectionRenderer.Instances.Count == 0,
                    "Realtime reflection probes are disabled in Quality Settings", "Enable", () =>
                    {
                        QualitySettings.realtimeReflectionProbes = true;
                    },MessageType.Error);

                EditorGUILayout.Space();

                if (_EnvironmentReflectionsOn.floatValue > 0 || _EnvironmentReflectionsOn.hasMixedValue)
                {
                    DrawShaderProperty(_ReflectionStrength, _ReflectionStrength.displayName);
                    if (_LightingOn.floatValue > 0f || _LightingOn.hasMixedValue)
                    {
                        DrawShaderProperty(_ReflectionLighting, new GUIContent(_ReflectionLighting.displayName, "Technically, lighting shouldn't be applied to the reflected image. If reflections aren't updated in realtime, but lighting is, this is still beneficial.\n\nThis controls how much lighting affects the reflection"));
                    }

                    EditorGUILayout.Space();

                    DrawShaderProperty(_ReflectionFresnel, new GUIContent(_ReflectionFresnel.displayName, "Masks the reflection by the viewing angle in relationship to the surface (including wave curvature), which is more true to nature (known as fresnel)"));
                    DrawShaderProperty(_ReflectionDistortion, new GUIContent(_ReflectionDistortion.displayName, "Distorts the reflection by the wave normals and normal map"));
                    DrawShaderProperty(_ReflectionBlur, new GUIContent(_ReflectionBlur.displayName, "Blurs the reflection probe, this can be used for a more general reflection of colors"));

                    EditorGUILayout.Space();

                    DrawShaderProperty(_ScreenSpaceReflectionsEnabled, new GUIContent(_ScreenSpaceReflectionsEnabled.displayName, "This technique simulates reflections based on what's already visible on the screen. " +
                                                                                                                                  "\nSSR calculates reflections from the water surface's curvature by using the opaque texture, rather than re-rendering the entire scene. " +
                                                                                                                                  "\n\n" +
                                                                                                                                  "While it improves visual quality with minimal performance impact compared to full reflections, SSR can produce artifacts or incomplete reflections for objects not visible on the screen," +
                                                                                                                                  "\nas it only has information from the camera’s current viewpoint available." +
                                                                                                                                  "\n\nWhere SSR fails to calculate a reflection it falls back on the reflection probe"));

                    UI.DrawNotification(
                        UniversalRenderPipeline.asset.supportsCameraOpaqueTexture == false && (_ScreenSpaceReflectionsEnabled.floatValue > 0),
                        "Opaque texture is disabled, which is required this effect",
                        "Enable",
                        StylizedWaterEditor.EnableOpaqueTexture,
                        MessageType.Error);

                    if (renderFeature)
                    {
                        UI.DrawNotification(renderFeature.screenSpaceReflectionSettings.allow == false, "Screen-space reflections are disabled on the active render feature");
                    }
                    else
                    {
                        UI.DrawNotification(_ScreenSpaceReflectionsEnabled.floatValue > 0.5, "Render feature hasn't been set up on the default renderer. SSR will have no effect.", MessageType.Warning);
                    }

                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField($"Planar Reflections renderers in scene: {PlanarReflectionRenderer.Instances.Count}", EditorStyles.miniLabel);
                    if (PlanarReflectionRenderer.Instances.Count > 0)
                    {
                        using (new EditorGUILayout.VerticalScope(EditorStyles.textArea))
                        {
                            foreach (PlanarReflectionRenderer r in PlanarReflectionRenderer.Instances)
                            {
                                using (new EditorGUILayout.HorizontalScope())
                                {
                                    EditorGUILayout.LabelField(r.name);
                                    if (GUILayout.Button("Select"))
                                    {
                                        Selection.activeGameObject = r.gameObject;
                                    }
                                }
                            }
                        }
                    }

                    EditorGUILayout.Space();
                }
            }
            EditorGUILayout.EndFadeGroup();
        }

        private void DrawWaves()
        {
            wavesSection.DrawHeader(() => SwitchSection(wavesSection));
            
            if (EditorGUILayout.BeginFadeGroup(wavesSection.anim.faded))
            {
                EditorGUILayout.Space();

                DrawShaderProperty(_WavesOn, "Enable");
                
                EditorGUILayout.Space();
                
                if (_WavesOn.floatValue == 1 || _WavesOn.hasMixedValue)
                {
                    DrawShaderProperty(_WaveProfile, "Wave Profile");
                    //EditorGUILayout.LabelField(AssetDatabase.GetAssetPath(_WaveProfile.textureValue), EditorStyles.miniLabel);
                    EditorGUI.indentLevel++;
                    UI.Material.DrawIntSlider(_WaveMaxLayers, tooltip:"Clamp the maximum number of wave layers being calculated");
                    EditorGUI.indentLevel--;
                    
                    EditorGUILayout.Space();
                    
                    UI.Material.DrawFloatTicker(_WaveSpeed, label: "Speed multiplier");
                    UI.Material.DrawFloatTicker(_WaveFrequency, label: "Wave length multiplier");
                    
                    EditorGUILayout.Space();
                    
                    EditorGUILayout.LabelField("Displacement", EditorStyles.boldLabel);
                    DrawShaderProperty(_WaveHeight, new GUIContent(_WaveHeight.displayName, "Scales the amplitude of the waves (this also scales the normal strength)"));
                    EditorGUI.indentLevel++;
                    DrawShaderProperty(_VertexColorWaveFlattening, new GUIContent("Vertex color flattening (B)",
                        "The Blue vertex color channel flattens waves"));
                    EditorGUI.indentLevel--;
                    DrawShaderProperty(_WaveSteepness, new GUIContent(_WaveSteepness.displayName, "Sharpness, depending on other settings here, a too high value will causes vertices to overlap. This also creates horizontal movement"));

                    EditorGUILayout.Space();
                    
                    EditorGUILayout.LabelField("Shading", EditorStyles.boldLabel);

                    if (_FlatShadingOn.floatValue < 0.5)
                    {
                        DrawShaderProperty(_WaveNormalStr, new GUIContent(_WaveNormalStr.displayName, "Normals affect how curved the surface is perceived for direct and ambient light. Without this, the water will appear flat"));
                    }
                    
                    UI.Material.DrawMinMaxSlider(_WaveFadeDistance, 0f, 1000f, "Fade Distance", "Fades out the waves between the start- and end distance. This can avoid tiling artifacts in the distance");
                }

                EditorGUILayout.Space();
            }
            EditorGUILayout.EndFadeGroup();
        }

        private void DrawShaderProperty(MaterialProperty prop, string label, int indent = 0)
        {
            DrawShaderProperty(prop, new GUIContent(label), indent);
        }

        private void DrawShaderProperty(MaterialProperty prop, GUIContent content, int indent = 0)
        {
            materialEditor.ShaderProperty(prop, content, indent);

            if (UI.ExpandTooltips && content.tooltip != string.Empty)
            {
                EditorGUILayout.HelpBox(content.tooltip, MessageType.None);
            }
        }

        private void DrawTextureSelector(MaterialProperty prop, ref List<Texture2D> textures)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                materialEditor.TextureProperty(prop, prop.displayName);

                GUILayout.Space(-2);

                if (GUILayout.Button(new GUIContent("▼", "Select a texture"), GUILayout.Height(65f), GUILayout.Width(21)))
                {
                    GenericMenu menu = new GenericMenu();

                    for (int i = 0; i < textures.Count; i++)
                    {
                        Texture2D tex = textures[i];
                        menu.AddItem(new GUIContent(textures[i].name, textures[i]), prop.textureValue && prop.textureValue == tex, () =>
                        {
                            prop.textureValue = tex;
                        });
                    }

                    menu.ShowAsContext();
                }
            }
        }
        
        private void SwitchSection(UI.Material.Section target)
        {
            foreach (var section in sections)
            {
                section.Expanded = (target == section) && !section.Expanded;
                //section.Expanded = true;
            }
        }
        #endregion

        private void UpgradeObsoleteProperties(Material material)
        {
            bool upgraded = false;
            float _VertexColorDepth = GetLegacyFloatProperty(material, "_VertexColorDepth");
            
            //Any material not yet upgraded would have this property...
            if (_VertexColorDepth >= 0)
            {
                upgraded = true;
                
                _VertexColorTransparency.floatValue = _VertexColorDepth;
                
                DeleteFloatProperty(material, "_VertexColorDepth");
            }
            
            float _ReceiveDynamicEffects = GetLegacyFloatProperty(material, "_ReceiveDynamicEffects");

            if (_ReceiveDynamicEffects > 0)
            {
                upgraded = true;
                
                _ReceiveDynamicEffectsHeight.floatValue = _ReceiveDynamicEffects;
                
                DeleteFloatProperty(material, "_ReceiveDynamicEffects");
            }
            
            if(upgraded)Debug.Log($"[Stylized Water 3] {material.name} upgraded to v3.0.3+ format");
        }
        
        private float GetLegacyFloatProperty(Material mat, string name)
        {
            SerializedObject materialObj = new SerializedObject(mat);
            
            //Note: Vectors are actually stored as colors
            SerializedProperty floatProperties = materialObj.FindProperty("m_SavedProperties.m_Floats");

            float prop = Mathf.NegativeInfinity;
            
            if (floatProperties != null && floatProperties.isArray) 
            {
                for (int j = floatProperties.arraySize-1; j >= 0; j--) 
                {
                    string propName = floatProperties.GetArrayElementAtIndex(j).displayName;

                    if (propName == name)
                    {
                        SerializedProperty val = floatProperties.GetArrayElementAtIndex(j).FindPropertyRelative("second");
                        
                        #if SWS_DEV
                        Debug.Log($"Found obsolete property \"{propName}\" with value: {val.floatValue} on material \"{mat.name}\"");
                        #endif

                        return val.floatValue;
                    }
                }
            }

            return prop;
        }
        
        private void DeleteFloatProperty(Material mat, string name)
        {
            SerializedObject materialObj = new SerializedObject(mat);
            
            SerializedProperty floatProperties = materialObj.FindProperty("m_SavedProperties.m_Floats");
            
            if (floatProperties != null && floatProperties.isArray) 
            {
                for (int j = floatProperties.arraySize-1; j >= 0; j--) 
                {
                    string propName = floatProperties.GetArrayElementAtIndex(j).displayName;

                    if (propName == name) 
                    {
                        floatProperties.DeleteArrayElementAtIndex(j);
                        materialObj.ApplyModifiedProperties();
                        
                        EditorUtility.SetDirty(mat);
                        
                        #if SWS_DEV
                        Debug.Log($"Deleted obsolete material property: {name}");
                        #endif
                    }
                }
            }
        }
#else
        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
        {
            UI.DrawNotification("The Universal Render Pipeline package v" + AssetInfo.MIN_URP_VERSION + " or newer is not installed", MessageType.Error);
        }
#endif
    }
}