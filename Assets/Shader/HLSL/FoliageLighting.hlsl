// MIT License

// Copyright (c) 2021 NedMakesGames

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

#ifndef FOLIAGE_LIGHTING_INCLUDED
#define FOLIAGE_LIGHTING_INCLUDED

// This is a neat trick to work around a bug in the shader graph when
// enabling shadow keywords. Created by @cyanilux
// https://github.com/Cyanilux/URP_ShaderGraphCustomLighting
// Licensed under the MIT License, Copyright (c) 2020 Cyanilux
#ifndef SHADERGRAPH_PREVIEW
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
#if (SHADERPASS != SHADERPASS_FORWARD)
#undef REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
#endif
#endif

struct FoliageLightingData
{
    // Position and orientation
    float3 positionWS;
    float3 meshNormalWS;
    float3 shapeNormalWS;
    float3 viewDirectionWS;
    float4 shadowCoord;

    // Surface attributes
    float3 albedo;
    float smoothness;
    float specularStrength;
    float ambientOcclusion;
    float3 subsurfaceColor;
    float thinness;
    float scatteringStrength;

    // Baked lighting
    float3 bakedGI;
    float subsurfaceAmbientStrength;
    float enviroReflectStrength;
    float4 shadowMask;
    float fogFactor;
};

// Translate a [0, 1] smoothness value to an exponent 
float GetSmoothnessPower(float rawSmoothness)
{
    return exp2(10 * rawSmoothness + 1);
}

#ifndef SHADERGRAPH_PREVIEW
float3 FoliageGlobalIllumination(FoliageLightingData d)
{
    float3 indirectDiffuse = d.albedo * d.bakedGI * d.ambientOcclusion +
        d.albedo * d.subsurfaceColor * (d.thinness * d.subsurfaceAmbientStrength);

    float3 reflectVector = reflect(-d.viewDirectionWS, d.meshNormalWS);
    // This is a rim light term, making reflections stronger along
    // the edges of view
    float fresnel = Pow4(1 - saturate(dot(d.viewDirectionWS, d.meshNormalWS)));
    // This function samples the baked reflections cubemap
    // It is located in URP/ShaderLibrary/Lighting.hlsl
    float3 indirectSpecular = GlossyEnvironmentReflection(reflectVector,
        RoughnessToPerceptualRoughness(1 - d.smoothness),
        d.ambientOcclusion) * (fresnel * d.enviroReflectStrength * d.specularStrength);;

    return indirectDiffuse + indirectSpecular;
}

float3 FoliageLightHandling(FoliageLightingData d, Light light)
{
    // Calculate radiance. Translucency radius ignores shadow attenuation
    float3 radiance = light.color * (light.distanceAttenuation * light.shadowAttenuation);
    float3 tlucencyRadiance = light.color * d.subsurfaceColor * light.distanceAttenuation;

    float diffuse = saturate(dot(d.shapeNormalWS, light.direction));
    float specularDot = saturate(dot(d.meshNormalWS, normalize(light.direction + d.viewDirectionWS)));
    float specular = pow(specularDot, GetSmoothnessPower(d.smoothness)) * diffuse * d.specularStrength;

    float3 scatteringDirection = normalize(-light.direction + d.meshNormalWS * d.scatteringStrength);
    float tlucencyDot = saturate(dot(d.viewDirectionWS, scatteringDirection));
    float tlucency = pow(tlucencyDot, GetSmoothnessPower(d.smoothness)) * d.thinness;

    float3 color = d.albedo * radiance * (diffuse + specular) +
        d.albedo * tlucencyRadiance * tlucency;

    return color;
}
#endif

float3 CalculateFoliageLighting(FoliageLightingData d)
{
#ifdef SHADERGRAPH_PREVIEW
    // In preview, estimate diffuse + specular
    float3 lightDir = float3(0.5, 0.5, 0);
    float intensity = saturate(dot(d.shapeNormalWS, lightDir)) +
        pow(saturate(dot(d.meshNormalWS, normalize(d.viewDirectionWS + lightDir))), GetSmoothnessPower(d.smoothness));
    return d.albedo * intensity;
#else
    // Get the main light. Located in URP/ShaderLibrary/Lighting.hlsl
    Light mainLight = GetMainLight(d.shadowCoord, d.positionWS, d.shadowMask);
    // In mixed subtractive baked lights, the main light must be subtracted
    // from the bakedGI value. This function in URP/ShaderLibrary/Lighting.hlsl takes care of that.
    MixRealtimeAndBakedGI(mainLight, d.shapeNormalWS, d.bakedGI);
    float3 color = FoliageGlobalIllumination(d);
    // Shade the main light
    color += FoliageLightHandling(d, mainLight);

#ifdef _ADDITIONAL_LIGHTS
    // Shade additional cone and point lights. Functions in URP/ShaderLibrary/Lighting.hlsl
    uint numAdditionalLights = GetAdditionalLightsCount();
    for (uint lightI = 0; lightI < numAdditionalLights; lightI++) {
        Light light = GetAdditionalLight(lightI, d.positionWS, d.shadowMask);
        color += FoliageLightHandling(d, light);
    }
#endif

    color = MixFog(color, d.fogFactor);

    return color;
#endif
}

void CalculateFoliageLighting_float(float3 Position, float3 MeshNormal, float3 ShapeNormal, float3 ViewDirection,
    float3 Albedo, float Smoothness, float SpecularStrength, float AmbientOcclusion,
    float3 SubsurfaceColor, float Thinness, float ScatteringStrength,
    float2 LightmapUV, float SubsurfaceAmbientStrength, float EnviroReflectStrength,
    out float3 Color)
{

    FoliageLightingData d;
    d.positionWS = Position;
    d.meshNormalWS = MeshNormal;
    d.shapeNormalWS = ShapeNormal;
    d.viewDirectionWS = ViewDirection;
    d.albedo = Albedo;
    d.smoothness = Smoothness;
    d.specularStrength = SpecularStrength;
    d.ambientOcclusion = AmbientOcclusion;
    d.subsurfaceColor = SubsurfaceColor;
    d.thinness = Thinness;
    d.scatteringStrength = ScatteringStrength;
    d.enviroReflectStrength = EnviroReflectStrength;
    d.subsurfaceAmbientStrength = SubsurfaceAmbientStrength;

#ifdef SHADERGRAPH_PREVIEW
    // In preview, there's no shadows or bakedGI
    d.shadowCoord = 0;
    d.bakedGI = 0;
    d.shadowMask = 0;
    d.fogFactor = 0;
#else
    // Calculate the main light shadow coord
    // There are two types depending on if cascades are enabled
    float4 positionCS = TransformWorldToHClip(Position);
#if SHADOWS_SCREEN
    d.shadowCoord = ComputeScreenPos(positionCS);
#else
    d.shadowCoord = TransformWorldToShadowCoord(Position);
#endif

    // The following URP functions and macros are all located in
    // URP/ShaderLibrary/Lighting.hlsl
    // Technically, OUTPUT_LIGHTMAP_UV, OUTPUT_SH and ComputeFogFactor
    // should be called in the vertex function of the shader. However, as of
    // 2021.1, we do not have access to custom interpolators in the shader graph.

    // The lightmap UV is usually in TEXCOORD1
    // If lightmaps are disabled, OUTPUT_LIGHTMAP_UV does nothing
    float2 lightmapUV;
    OUTPUT_LIGHTMAP_UV(LightmapUV, unity_LightmapST, lightmapUV);
    // Samples spherical harmonics, which encode light probe data
    float3 vertexSH;
    OUTPUT_SH(ShapeNormal, vertexSH);
    // This function calculates the final baked lighting from light maps or probes
    d.bakedGI = SAMPLE_GI(lightmapUV, vertexSH, ShapeNormal);
    // This function calculates the shadow mask if baked shadows are enabled
    d.shadowMask = SAMPLE_SHADOWMASK(lightmapUV);
    // This returns 0 if fog is turned off
    // It is not the same as the fog node in the shader graph
    d.fogFactor = ComputeFogFactor(positionCS.z);
#endif

    Color = CalculateFoliageLighting(d);
}

#endif