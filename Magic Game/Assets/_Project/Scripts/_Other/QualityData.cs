using System;

[Serializable]

public class QualityData
{
    //Vsync
    public int VSYNC_COUNT = 1;

    //Anti-aliasing
    public string AA_METHOD = "fxaa";
    public int AA_QUALITY = 0;

    //Model quality
    public int MAXIMUM_LOD_LEVEL = 0;
    public float LOD_BIAS = 1.0f;
    public bool FOUR_BONE_BLENDWEIGHTS = true;

    //Texture quality
    public bool ANISOTROPIC_FILTERING = true;
    public int MAXIMUM_MIPMAP_LEVEL = 0;
    public bool TEXTURE_MIPMAP_STREAMING = true;

    //Shadows
    public string SHADOWS_RESOLUTION = "veryhigh"; //low, medium, high, veryhigh
    public string SHADOWS_REALTIME_MODE = "soft"; //disabled, hardonly, soft
    public int SHADOWS_CASCADE_AMOUNT = 4;
    public float SHADOWS_REALTIME_DISTANCE = 150.0f;
    public bool SHADOWS_STATIC_OBJECTS_CAST_REALTIME = false;
    public bool SHADOWS_STABLE_FIT = true;

    //Miscellaneous
    public int SHADER_GRAPHICS_TIER = 3;
    public bool SOFT_PARTICLES = true;
    public bool SOFT_VEGETATION = false;
    public int PIXEL_LIGHT_COUNT = 4;
    public bool REALTIME_REFLECTIONS = true;
    public bool BILLBOARDS_FACE_CAMERA_POSITION = true;
    public float UI_RESOLUTION_DPI_SCALING = 1.0f;
    public bool FULLSCREEN = true;
}
