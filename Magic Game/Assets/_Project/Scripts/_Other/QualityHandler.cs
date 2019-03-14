using UnityEngine;

public class QualityHandler : MonoBehaviour
{
    void Awake()
    {
        QualityManager.LoadSettings();
    }

    public void ApplySettings()
    {
        QualityManager.SaveSettings();
        QualityManager.ApplySettings();
    }

    //Vsync
    public void VSYNC_COUNT(int i)                              { QualityManager.DATA.VSYNC_COUNT = i; }

    //Anti-aliasing
    public void AA_METHOD(string s)                             { QualityManager.DATA.AA_METHOD = s; }
    public void AA_QUALITY(int i)                               { QualityManager.DATA.AA_QUALITY = i; }

    //Model quality
    public void MAXIMUM_LOD_LEVEL(int i)                        { QualityManager.DATA.MAXIMUM_LOD_LEVEL = i; }
    public void LOD_BIAS(float f)                               { QualityManager.DATA.LOD_BIAS = f; }
    public void FOUR_BONE_BLENDWEIGHTS(bool b)                  { QualityManager.DATA.FOUR_BONE_BLENDWEIGHTS = b; }

    //Texture quality
    public void ANISOTROPIC_FILTERING(bool b)                   { QualityManager.DATA.ANISOTROPIC_FILTERING = b; }
    public void MAXIMUM_MIPMAP_LEVEL(int i)                     { QualityManager.DATA.MAXIMUM_MIPMAP_LEVEL = i; }
    public void TEXTURE_MIPMAP_STREAMING(bool b)                { QualityManager.DATA.TEXTURE_MIPMAP_STREAMING = b; }

    //Shadows
    public void SHADOWS_RESOLUTION(string s)                    { QualityManager.DATA.SHADOWS_RESOLUTION = s; }
    public void SHADOWS_REALTIME_MODE(string s)                 { QualityManager.DATA.SHADOWS_REALTIME_MODE = s; }
    public void SHADOWS_CASCADE_AMOUNT(int i)                   { QualityManager.DATA.SHADOWS_CASCADE_AMOUNT = i; }
    public void SHADOWS_REALTIME_DISTANCE(float f)              { QualityManager.DATA.SHADOWS_REALTIME_DISTANCE = f; }
    public void SHADOWS_STATIC_OBJECTS_CAST_REALTIME(bool b)    { QualityManager.DATA.SHADOWS_STATIC_OBJECTS_CAST_REALTIME = b; }
    public void SHADOWS_STABLE_FIT(bool b)                      { QualityManager.DATA.SHADOWS_STABLE_FIT = b; }

    //Miscellaneous
    public void SHADER_GRAPHICS_TIER(int i)                     { QualityManager.DATA.SHADER_GRAPHICS_TIER = i; }
    public void SOFT_PARTICLES(bool b)                          { QualityManager.DATA.SOFT_PARTICLES = b; }
    public void SOFT_VEGETATION(bool b)                         { QualityManager.DATA.SOFT_VEGETATION = b; }
    public void PIXEL_LIGHT_COUNT(int i)                        { QualityManager.DATA.PIXEL_LIGHT_COUNT = i; }
    public void REALTIME_REFLECTIONS(bool b)                    { QualityManager.DATA.REALTIME_REFLECTIONS = b; }
    public void BILLBOARDS_FACE_CAMERA_POSITION(bool b)         { QualityManager.DATA.BILLBOARDS_FACE_CAMERA_POSITION = b; }
    public void UI_RESOLUTION_DPI_SCALING(float f)              { QualityManager.DATA.UI_RESOLUTION_DPI_SCALING = f; }
}
