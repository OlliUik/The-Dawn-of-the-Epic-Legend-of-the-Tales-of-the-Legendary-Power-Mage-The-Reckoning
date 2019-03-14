using System.IO;
using UnityEngine;

public static class QualityManager
{
    private static string PATH = Path.Combine(Application.persistentDataPath, "QualitySettings.ini");
    public static QualityData DATA = new QualityData();

    public static void LoadSettings()
    {
        if (File.Exists(PATH))
        {
            using (StreamReader reader = File.OpenText(PATH))
            {
                string jsonString = reader.ReadToEnd();
                DATA = JsonUtility.FromJson<QualityData>(jsonString);
                Debug.Log(PATH + " loaded successfully.");
            }
        }
        else
        {
            Debug.Log(PATH + " not found, using default quality settings...");
            switch (Graphics.activeTier)
            {
                case UnityEngine.Rendering.GraphicsTier.Tier1: DATA.SHADER_GRAPHICS_TIER = 1; break;
                case UnityEngine.Rendering.GraphicsTier.Tier2: DATA.SHADER_GRAPHICS_TIER = 2; break;
                case UnityEngine.Rendering.GraphicsTier.Tier3: DATA.SHADER_GRAPHICS_TIER = 3; break;
            }
            SaveSettings();
        }
        ApplySettings();
    }

    public static void SaveSettings()
    {
        string jsonString = JsonUtility.ToJson(DATA);

        using (StreamWriter writer = File.CreateText(PATH))
        {
            writer.Write(jsonString);
        }
    }

    public static void ApplySettings()
    {
        //Vsync
        QualitySettings.vSyncCount = DATA.VSYNC_COUNT;

        //Anti-aliasing
        switch (DATA.AA_METHOD)
        {
            case "msaa": QualitySettings.antiAliasing = DATA.AA_QUALITY; break;
            default: QualitySettings.antiAliasing = 0; break;
        }

        //Model quality
        QualitySettings.maximumLODLevel = DATA.MAXIMUM_LOD_LEVEL;
        QualitySettings.lodBias = DATA.LOD_BIAS;
        QualitySettings.blendWeights = DATA.FOUR_BONE_BLENDWEIGHTS ?
            BlendWeights.FourBones
            : BlendWeights.TwoBones;

        //Texture quality
        QualitySettings.anisotropicFiltering = DATA.ANISOTROPIC_FILTERING ?
            AnisotropicFiltering.Enable
            : AnisotropicFiltering.Disable;
        QualitySettings.masterTextureLimit = DATA.MAXIMUM_MIPMAP_LEVEL;
        QualitySettings.streamingMipmapsActive = DATA.TEXTURE_MIPMAP_STREAMING;

        //Shadows
        switch (DATA.SHADOWS_RESOLUTION)
        {
            case "low": QualitySettings.shadowResolution = ShadowResolution.Low; break;
            case "medium": QualitySettings.shadowResolution = ShadowResolution.Medium ; break;
            case "high": QualitySettings.shadowResolution = ShadowResolution.High; break;
            case "veryhigh": QualitySettings.shadowResolution = ShadowResolution.VeryHigh; break;
            default: QualitySettings.shadowResolution = ShadowResolution.Low; break;
        }
        switch (DATA.SHADOWS_REALTIME_MODE)
        {
            case "soft": QualitySettings.shadows = ShadowQuality.All; break;
            case "hardonly": QualitySettings.shadows = ShadowQuality.HardOnly; break;
            default: QualitySettings.shadows = ShadowQuality.Disable; break;
        }
        QualitySettings.shadowCascades = DATA.SHADOWS_CASCADE_AMOUNT;
        QualitySettings.shadowDistance = DATA.SHADOWS_REALTIME_DISTANCE;
        QualitySettings.shadowmaskMode = DATA.SHADOWS_STATIC_OBJECTS_CAST_REALTIME ?
            ShadowmaskMode.DistanceShadowmask
            : ShadowmaskMode.Shadowmask;
        QualitySettings.shadowProjection = DATA.SHADOWS_STABLE_FIT ?
            ShadowProjection.StableFit
            : ShadowProjection.CloseFit;

        //Miscellaneous
        switch (DATA.SHADER_GRAPHICS_TIER)
        {
            case 2: Graphics.activeTier = UnityEngine.Rendering.GraphicsTier.Tier2; break;
            case 3: Graphics.activeTier = UnityEngine.Rendering.GraphicsTier.Tier3; break;
            default: Graphics.activeTier = UnityEngine.Rendering.GraphicsTier.Tier1; break;
        }
        QualitySettings.softParticles = DATA.SOFT_PARTICLES;
        QualitySettings.softVegetation = DATA.SOFT_VEGETATION;
        QualitySettings.pixelLightCount = DATA.PIXEL_LIGHT_COUNT;
        QualitySettings.realtimeReflectionProbes = DATA.REALTIME_REFLECTIONS;
        QualitySettings.billboardsFaceCameraPosition = DATA.BILLBOARDS_FACE_CAMERA_POSITION;
        QualitySettings.resolutionScalingFixedDPIFactor = DATA.UI_RESOLUTION_DPI_SCALING;

        if (DATA.SHADER_GRAPHICS_TIER == 3 && Camera.main.actualRenderingPath != RenderingPath.DeferredShading)
        {
            Debug.LogWarning("Failed to activate Deferred rendering! Switching back to medium shading...");
            DATA.SHADER_GRAPHICS_TIER = 2;
            Graphics.activeTier = UnityEngine.Rendering.GraphicsTier.Tier2;
        }
        Debug.Log("Quality settings applied successfully.");
    }
}
