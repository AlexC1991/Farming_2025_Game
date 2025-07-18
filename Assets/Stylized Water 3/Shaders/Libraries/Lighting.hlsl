// Stylized Water 3 by Staggart Creations (http://staggart.xyz)
// COPYRIGHT PROTECTED UNDER THE UNITY ASSET STORE EULA (https://unity.com/legal/as-terms)
//    • Copying or referencing source code for the production of new asset store, or public, content is strictly prohibited!
//    • Uploading this file to a public repository will subject it to an automated DMCA takedown request.

#ifndef WATER_LIGHTING_INCLUDED
#define WATER_LIGHTING_INCLUDED

#include "Common.hlsl"
#include "Reflections.hlsl"

#if !_UNLIT
#define LIT
#endif

#define SPECULAR_POWER_RCP 0.01562 // 1.0/32
#define SPECULAR_STEP_THRESHOLD 0.2

//Reusable for every light
struct TranslucencyData
{
	bool directionalLight;
	float3 subsurfaceColor;
	float3 lightColor;
	float3 lightDir;
	float3 viewDir;
	float3 normal;
	float curvature;
	float mask; //Actually the 'thickness'
	float strength;
	float strengthIncident;
	float exponent;

};

TranslucencyData PopulateTranslucencyData(float3 subsurfaceColor, float3 lightDir, float3 lightColor, float3 viewDir, float3 WorldNormal, float3 worldTangentNormal, float mask, float strength, float incidentStrength, float exponent, float offset, bool directionalLight)
{
	TranslucencyData d = (TranslucencyData)0;
	d.directionalLight = directionalLight;
	d.subsurfaceColor = subsurfaceColor;
	d.lightColor = lightColor;
	d.lightDir = lightDir;

	#if _ADVANCED_SHADING
	//Slightly include high frequency details
	d.normal = normalize(WorldNormal + (worldTangentNormal * 0.2));
	#else
	d.normal = WorldNormal;
	#endif
	d.curvature = offset;
	d.mask = mask; //Shadows, foam, intersection, etc
	d.strength = strength;
	d.strengthIncident = incidentStrength;
	d.viewDir = viewDir;
	d.exponent = exponent;

	return d;
}

//Single channel overlay
float BlendOverlay(float a, float b)
{
	return (b < 0.5) ? 2.0 * a * b : 1.0 - 2.0 * (1.0 - a) * (1.0 - b);
}

//RGB overlay
float3 BlendOverlay(float3 a, float3 b)
{
	return float3(BlendOverlay(a.r, b.r), BlendOverlay(a.g, b.g), BlendOverlay(a.b, b.b));
}

//In URP light intensity is pre-multiplied with the HDR color, extract via magnitude of color "vector"
float GetLightIntensity(float3 lightColor)
{
	//Luminance equals HDR output
	return (lightColor.r * 0.3 + lightColor.g * 0.59 + lightColor.b * 0.11);
}

float GetLightIntensity(Light light) { return GetLightIntensity(light.color); }

void ApplyTranslucency(float3 subsurfaceColor, float3 lightDir, float3 lightColor, float3 viewDir, float3 normal, float occlusion, float strength, float incidentStrength, float exponent, float offset, bool directionalLight, inout float3 emission)
{
	//Coefficient describing how much the surface orientation is between the camera and the direction of/to the light  
	half transmittance = saturate(dot(-viewDir, lightDir));
	//Exponentiate to tighten the falloff
	transmittance = saturate(pow(transmittance, exponent)) * strength;
	
	half incident = 0;
	if(directionalLight)
	{
		incident = saturate(dot(lightDir, normal)) * incidentStrength;
	}

	//Mask by normals facing away from the light (backfaces, in light-space)
	const half curvature = saturate(lerp(1.0, dot(normal, -lightDir), offset));
	transmittance *= curvature;

	const float lightIntensity = saturate(GetLightIntensity(lightColor));

	half attenuation = (transmittance + incident) * occlusion * lightIntensity;

#if _ADVANCED_SHADING
	if(directionalLight)
	{
		//Fade the effect out as the sun approaches the horizon (80 to 90 degrees)
		half sunAngle = saturate(dot(float3(0, 1, 0), lightDir));
		half angleMask = saturate(sunAngle * 10); /* 1.0/0.10 = 10 */
		attenuation *= angleMask;
	}
	
	//Modulate with light color to better match dynamic lighting conditions
	subsurfaceColor = BlendOverlay(saturate(lightColor), subsurfaceColor);
	
	emission += subsurfaceColor * attenuation;
#else //Simple shading
	emission += lerp(emission, subsurfaceColor, attenuation);
#endif
}

void ApplyTranslucency(TranslucencyData translucencyData, inout float3 emission)
{
	ApplyTranslucency(translucencyData.subsurfaceColor, translucencyData.lightDir, translucencyData.lightColor, translucencyData.viewDir, translucencyData.normal, translucencyData.mask, translucencyData.strength, translucencyData.strengthIncident, translucencyData.exponent, translucencyData.curvature, translucencyData.directionalLight, emission);
}

void AdjustShadowStrength(inout Light light, float strength, float vFace)
{
	light.shadowAttenuation = saturate(light.shadowAttenuation + (1.0 - (strength * vFace)));
}

//Specular Blinn-phong reflection in world-space
float3 SpecularReflection(Light light, float3 viewDirectionWS, float3 geometryNormalWS, float3 normalWS, float perturbation, float exponent, float intensity, bool sharp)
{
	//Blend between geometry/wave normals and normals from normal map (aka distortion)
	normalWS = lerp(geometryNormalWS, normalWS, perturbation);

	const float3 halfVec = normalize(light.direction + viewDirectionWS + (normalWS * perturbation));
	half NdotH = saturate(dot(geometryNormalWS, halfVec));

	float specular = pow(NdotH, exponent);
	
	if(sharp)
	{
		specular = step(SPECULAR_STEP_THRESHOLD, specular);
		intensity *= 0.5;
	}
	
	//Attenuation includes shadows, if available
	const float3 attenuatedLightColor = light.color * (light.distanceAttenuation * light.shadowAttenuation);

	//Mask reflection by surfaces "visible" by the light only
	float viewFactor = saturate(dot(geometryNormalWS, light.direction));

	#if _ADVANCED_SHADING
	//Create a linear gradient a little before the cutoff point, in order to maintain HDR values properly
	viewFactor = smoothstep(0.0, 0.15, viewFactor);
	#endif

	float3 specColor = attenuatedLightColor * specular * intensity * viewFactor;
	
	#if UNITY_COLORSPACE_GAMMA
	specColor = LinearToSRGB(specColor);
	#endif

	return specColor;
}

//Based on UniversalFragmentBlinnPhong (no BRDF)
float3 ApplyLighting(inout SurfaceData surfaceData, inout float3 sceneColor, Light mainLight, InputData inputData, WaterSurface water, TranslucencyData translucencyData, float shadowStrength, float vFace, bool isMatchingLightLayer)
{
	if(isMatchingLightLayer) ApplyTranslucency(translucencyData, surfaceData.emission.rgb);

	#if _CAUSTICS
	float causticsAttentuation = 1.0;
	#endif
	
#ifdef LIT
	#if _CAUSTICS && !defined(LIGHTMAP_ON)
	if(isMatchingLightLayer)
	{
		causticsAttentuation = GetLightIntensity(mainLight) * (mainLight.distanceAttenuation * mainLight.shadowAttenuation);
	}
	#endif
	
	MixRealtimeAndBakedGI(mainLight, water.diffuseNormal, inputData.bakedGI, shadowStrength.xxxx);

	/*
	//PBR shading
	BRDFData brdfData;
	InitializeBRDFData(surfaceData.albedo, surfaceData.metallic, surfaceData.specular, surfaceData.smoothness, surfaceData.alpha, brdfData);

	half3 diffuseColor = GlobalIllumination(brdfData, inputData.bakedGI, shadowStrength, inputData.water.diffuseNormal, inputData.viewDirectionWS);
	diffuseColor += LightingPhysicallyBased(brdfData, mainLight, water.diffuseNormal, inputData.viewDirectionWS);
	*/

	half3 directLight = 0;
	if(isMatchingLightLayer)
	{
		//Allow shadow strength to be overridden.
		AdjustShadowStrength(mainLight, shadowStrength, vFace);
		
		half3 attenuatedLightColor = mainLight.color * (mainLight.distanceAttenuation * mainLight.shadowAttenuation);
		directLight = LightingLambert(attenuatedLightColor, mainLight.direction, water.diffuseNormal);
	}
	half3 diffuseColor = inputData.bakedGI + directLight;
	
#if _ADDITIONAL_LIGHTS //Per pixel lights
	#ifndef _SPECULARHIGHLIGHTS_OFF
	half specularPower = (_PointSpotLightReflectionSize * SPECULAR_POWER_RCP);
	specularPower = lerp(8.0, 1.0, _PointSpotLightReflectionSize) * _PointSpotLightReflectionStrength;
	#endif
	
	uint pixelLightCount = GetAdditionalLightsCount();
	#if _LIGHT_LAYERS
	uint meshRenderingLayers = GetMeshRenderingLayer();
	#endif

	#if _TRANSLUCENCY
	float translucencyStrength = translucencyData.strength;
	float translucencyExp = translucencyData.exponent;
	#endif
	
	LIGHT_LOOP_BEGIN(pixelLightCount)

		Light light = GetAdditionalLight(lightIndex, inputData.positionWS, shadowStrength.xxxx);	

		#if _LIGHT_LAYERS
		if (IsMatchingLightLayer(light.layerMask, meshRenderingLayers))
		#endif
		{
			#if _ADVANCED_SHADING
			#if _CAUSTICS && !_LIGHT_COOKIES && defined(_ADDITIONAL_LIGHT_CAUSTICS) //Actually want to skip this when using cookies. Since they can be used for caustics instead
			//Light attenuation adds caustics, mask by shadows
			causticsAttentuation += GetLightIntensity(light) * (light.distanceAttenuation * light.shadowAttenuation) * _PointSpotLightReflectionStrength * (1-water.fog);
			#endif
			
			#if _TRANSLUCENCY && defined(_ADDITIONAL_LIGHT_TRANSLUCENCY)
			//Keep settings from main light pass, but override these
			translucencyData.directionalLight = false;
			if(water.vFace > 0)
			{
				translucencyData.lightDir = light.direction;
				translucencyData.lightColor = light.color * light.distanceAttenuation;
				translucencyData.strength = translucencyStrength * light.shadowAttenuation * (water.fog);
				translucencyData.exponent = translucencyExp * light.distanceAttenuation;
				
				ApplyTranslucency(translucencyData, surfaceData.emission.rgb);
			}
			#endif
			#endif

			#if _ADDITIONAL_LIGHT_SHADOWS //URP 11+
			AdjustShadowStrength(light, shadowStrength, vFace);
			#endif

			half3 attenuatedLightColor = light.color * (light.distanceAttenuation * light.shadowAttenuation);
			diffuseColor += LightingLambert(attenuatedLightColor, light.direction, water.diffuseNormal);

			#ifndef _SPECULARHIGHLIGHTS_OFF
			//Note: View direction fetched again using the function that takes orthographic projection into account
			surfaceData.specular += SpecularReflection(light, normalize(GetWorldSpaceViewDir(inputData.positionWS)), water.waveNormal, water.tangentWorldNormal, _PointSpotLightReflectionDistortion, lerp(4096, 64, _PointSpotLightReflectionSize), specularPower, _PointSpotLightReflectionSharp);
		#endif
	}
	LIGHT_LOOP_END
#endif

#ifdef _ADDITIONAL_LIGHTS_VERTEX //Previous calculated in vertex stage
	diffuseColor += inputData.vertexLighting;
#endif

#else //Unlit
	const half3 diffuseColor = 1;
#endif

	#if _CAUSTICS
	surfaceData.emission.rgb += water.caustics * causticsAttentuation * vFace;
	#endif

	float3 color = (surfaceData.albedo.rgb * diffuseColor) + surfaceData.emission.rgb + surfaceData.specular;
	
	#ifndef _ENVIRONMENTREFLECTIONS_OFF
	//Reflections blend in on top of everything
	color = lerp(color, water.reflections.rgb, water.reflectionMask * water.reflectionLighting * vFace);
	#endif

	#if _REFRACTION
	//Ensure the same effects are applied to the underwater scene color. Otherwise not visible on clear water
	sceneColor += (surfaceData.emission.rgb + surfaceData.specular) * vFace;
	#endif
	
	//Debug
	//return float4(surfaceData.emission.rgb, 1.0);	
	
	return color;
}

//Color of light ray passing through the water, hitting the sea floor (extinction)
//This applies to the scene color
float LightExtinction(float verticalDepth, float viewDepth, float density)
{
	return exp(-density * (verticalDepth + viewDepth));
}

//Energy loss of ray, as it travels deeper and scatters (absorption)
//This applies to the color of the underwater fog
float LightAbsorption(float absorption, float viewDepth)
{
	return saturate(exp(-absorption * viewDepth));
}
#endif