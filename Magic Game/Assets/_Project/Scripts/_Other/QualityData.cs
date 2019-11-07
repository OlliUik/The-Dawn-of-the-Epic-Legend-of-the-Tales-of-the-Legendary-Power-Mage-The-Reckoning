using System;
using UnityEngine;

[Serializable]
public class QualityData
{
    //Keybinds and sensitivity
    [Header("Keybinds & sensitivity")]
    public Vector2 SENSITIVITY = Vector2.one;

    //Vsync
    [Header("Graphics quality")]
    [Tooltip("When full sync is enabled, the game is rendered at the refresh rate of the monitor.\n\nWhen half sync is enabled, the game is rendered at half of the refresh rate (60Hz = 30FPS).\n\n0 = Don't sync, 1 = Full sync, 2 = Half sync.")]
    [Range(0, 2)]public int VSYNC_COUNT = 1;

    //Anti-aliasing
    [Tooltip("Affects how smooth the game looks.\n\nHaving no anti-aliasing leads to jagged edges.\n\nAccepted values: 'none', 'msaa', 'fxaa', 'smaa', 'taa'.")]
    public string AA_METHOD = "fxaa";
    [Tooltip("Affects the quality of smoothing provided by anti-aliasing.\n\nOnly affects MSAA.")]
    public int AA_QUALITY = 0;

    //Model quality
    [Tooltip("Affects the quality of models.\n\nThis feature depends upon if artists have created proper 'level of detail' models.\n\nIf unsure, leave this at 0.")]
    public int MAXIMUM_LOD_LEVEL = 0;
    [Tooltip("Affects how fast lower quality models are used the further they are from the camera.\n\nThis feature depends upon if artists have created proper 'level of detail' models.")]
    public float LOD_BIAS = 1.0f;
    [Tooltip("Disabling four bone blendweights can improve performance, but will most likely lead to animation errors.\n\nLeaving this enabled is recommended.")]
    public bool FOUR_BONE_BLENDWEIGHTS = true;

    //Texture quality
    [Tooltip("Affects if textures should be smoothed the further away they are from the camera.\n\nHaving this disabled can improve performance, but will introduce 'shimmering' on distant textures.")]
    public bool ANISOTROPIC_FILTERING = true;
    [Tooltip("Affects the quality of textures.\n\n0 = Full resolution textures, 1 = Half resolution textures, 2 = Quarter resolution textures.")]
    [Range(0, 3)]public int MAXIMUM_MIPMAP_LEVEL = 0;
    [Tooltip("Having this enabled streams the textures into memory in a smoother way, stabilising framerate when new textures are loaded.")]
    public bool TEXTURE_MIPMAP_STREAMING = true;

    //Shadows
    [Tooltip("Affects shadow resolution and quality.\n\nAccepted values: 'veryhigh', 'high', 'medium', 'low'.")]
    public string SHADOWS_RESOLUTION = "veryhigh";
    [Tooltip("Affects the sharpness of shadows.\n\nAccepted values: 'soft', 'hardonly', 'disabled'.")]
    public string SHADOWS_REALTIME_MODE = "soft";
    [Tooltip("Affects the shadow quality at different distances from the camera.\n\nHaving more cascades improves shadows close to the camera, but can affect performance.\n\nShadow cascade amount is rounded to 0, 2 or 4.")]
    [Range(0, 4)] public int SHADOWS_CASCADE_AMOUNT = 4;
    [Tooltip("Affects how far the shadows are rendered, until pre-baked shadows are used (if provided by the scene).\n\nHaving shadows rendered further can alleviate the problems with having the shadows 'disappear' at a distance, but leads to poorer shadow quality closer to the camera.")]
    public float SHADOWS_REALTIME_DISTANCE = 150.0f;
    [Tooltip("Affects if static objects should use realtime shadows instead of pre-baked ones.\n\nDoesn't do anything if there are no static objects or no baked lightmaps.")]
    public bool SHADOWS_STATIC_OBJECTS_CAST_REALTIME = false;
    [Tooltip("Affects shadow quality.\n\nStable fit works most of the time.\n\nClose fit works best with a static camera angle, but can break when the camera is moved a lot.")]
    public bool SHADOWS_STABLE_FIT = true;

    //Miscellaneous
    [Tooltip("Affects the shading tier defined in 'Project Settings -> Graphics'.\n\nHaving a lower tier can improve performance, but leads to worse overall quality.")]
    [Range(1, 3)] public int SHADER_GRAPHICS_TIER = 3;
    [Tooltip("Affects if particles should 'fade' into walls instead of suddenly clipping into them.")]
    public bool SOFT_PARTICLES = true;
    public bool SOFT_VEGETATION = false;
    [Tooltip("Affects the amount of pixel lights available. After the maximum amount of pixel lights have been used, vertex lights are used instead.\n\nOnly affects Forward rendering (lowest graphics tier).")]
    [Range(0, 32)] public int PIXEL_LIGHT_COUNT = 4;
    [Tooltip("Enables/disables realtime reflections.\n\nDisabling reflections can improve performance, but can make materials look unrealistic.")]
    public bool REALTIME_REFLECTIONS = true;
    [Tooltip("If enabled, billboards (particles) face the camera position instead of the camera's rendering plane.\n\nHaving this enabled can make billboards look better when the camera is rotated, but is more expensive to render.")]
    public bool BILLBOARDS_FACE_CAMERA_POSITION = true;
    public float UI_RESOLUTION_DPI_SCALING = 1.0f;
    public bool FULLSCREEN = true;
}
