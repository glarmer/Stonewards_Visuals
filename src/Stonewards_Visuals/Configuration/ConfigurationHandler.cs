using System;
using BepInEx.Configuration;
using Stonewards_Visuals.Upscaling;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Stonewards_Visuals.Configuration;

public sealed class ConfigurationHandler
{
    private ConfigFile _config;
    public InputAction? MenuAction { get; set; }

    public ConfigEntry<bool> ConfigEnableRenderScale;
    public ConfigEntry<float> ConfigRenderScale;
    public ConfigEntry<int> ConfigUpscalingFilter;
    public ConfigEntry<int> ConfigDLSSMode;
    public ConfigEntry<int> ConfigDLSSPreset;
    public ConfigEntry<bool> ConfigDLSSJitter;
    public ConfigEntry<float> ConfigDLSSJitterStrength;
    public ConfigEntry<float> ConfigLODQuality;
    public ConfigEntry<string> ConfigMenuKey;
    public ConfigEntry<int> ConfigShadowDistance;
    public ConfigEntry<int> ConfigShadowCascades;
    public ConfigEntry<int> ConfigShadowmapResolution;
    public ConfigEntry<bool> ConfigSoftShadows;
    public ConfigEntry<bool> ConfigAnisotropicFiltering;
    public ConfigEntry<int> ConfigCameraAA;
    public ConfigEntry<int> ConfigMSAA;
    public ConfigEntry<bool> ConfigPixelationEnabled;
    public ConfigEntry<float> ConfigPixelationIntensity;
    public ConfigEntry<bool> ConfigPixelationColorPrecision;
    public ConfigEntry<float> ConfigPixelationColorSteps;
    public ConfigEntry<bool> ConfigPixelationDithering;
    public ConfigEntry<int> ConfigPixelationDitherPattern;
    public ConfigEntry<float> ConfigPixelationDitherStrength;

    public bool EnableRenderScale => ConfigEnableRenderScale.Value;
    public float RenderScale => ConfigRenderScale.Value;
    public int UpscalingFilter => ConfigUpscalingFilter.Value;
    internal DLSSMode DLSSMode => (DLSSMode)ConfigDLSSMode.Value;
    internal DLSSPresetMode DLSSPresetMode => (DLSSPresetMode)ConfigDLSSPreset.Value;
    public bool DLSSJitterEnabled => ConfigDLSSJitter.Value;
    public float DLSSJitterStrength => ConfigDLSSJitterStrength.Value;
    public bool DLSSEnabled => Plugin.Instance.UpscalerLibAvailable && ConfigDLSSMode.Value != (int)Stonewards_Visuals.Upscaling.DLSSMode.Off;
    public float LodQuality => ConfigLODQuality.Value;
    public int ShadowDistance => ConfigShadowDistance.Value;
    public int ShadowCascades => ConfigShadowCascades.Value;
    public int ShadowmapResolution => ConfigShadowmapResolution.Value;
    public bool SoftShadows => ConfigSoftShadows.Value;
    public bool AnisotropicFiltering => ConfigAnisotropicFiltering.Value;
    public int CameraAA => ConfigCameraAA.Value;
    public int MSAA => ConfigMSAA.Value;
    public bool PixelationEnabled => ConfigPixelationEnabled.Value;
    public float PixelationIntensity => ConfigPixelationIntensity.Value;
    public bool PixelationColorPrecisionEnabled => ConfigPixelationColorPrecision.Value;
    public float PixelationColorSteps => ConfigPixelationColorSteps.Value;
    public bool PixelationDitheringEnabled => ConfigPixelationDithering.Value;
    public int PixelationDitherPattern => ConfigPixelationDitherPattern.Value;
    public float PixelationDitherStrength => ConfigPixelationDitherStrength.Value;

    public ConfigurationHandler(ConfigFile configFile)
    {
        _config = configFile;

        Plugin.Log.LogInfo("ConfigurationHandler initialising");

        ConfigEnableRenderScale = Bind(
            "Scaling",
            "EnableRenderScale",
            true,
            "Allows Stonewards Visuals to override the game's internal render scale. When off, the game controls pixelisation and render resolution.",
            () => Plugin.Instance.Settings.SetResolutionScale()
        );

        ConfigRenderScale = Bind(
            "Scaling",
            "RenderScale",
            1f,
            "Controls the render scale of the game. Native is 1.0 (100%). Range 0.1-2.0",
            () => Plugin.Instance.Settings.SetResolutionScale(),
            v => Mathf.Clamp(v, 0.1f, 2f)
        );

        ConfigUpscalingFilter = Bind(
            "Scaling",
            "UpscalingFilter",
            2,
            "Controls what filter the game uses to scale to your monitor resolution. 0 = auto, 1 = linear, 2 = point, 3 = FSR 1.0, 4 = STP",
            () => Plugin.Instance.Settings.SetUpscaler(),
            v => Mathf.Clamp(v, 0, 4)
        );

        ConfigDLSSMode = Bind(
            "Scaling",
            "DLSSMode",
            0,
            "Controls NVIDIA DLSS, requires an Nvidia RTX GPU! 0 = Off, 1 = Ultra Quality, 2 = Quality, 3 = Balanced, 4 = Performance, 5 = Ultra Performance, 6 = DLAA.",
            () => Plugin.Instance.Settings.SetDLSS(),
            v => Mathf.Clamp(v, 0, 6)
        );

        ConfigDLSSPreset = Bind(
            "Scaling",
            "DLSSPreset",
            (int)DLSSPresetMode.PresetK,
            "Controls the DLSS render preset, experiment to your taste M and K are pretty good. 0 = Preset F, 1 = Preset J, 2 = Preset K, 3 = Preset L, 4 = Preset M.",
            () => Plugin.Instance.Settings.SetDLSS()
        );

        ConfigDLSSJitter = Bind(
            "Scaling",
            "DLSSJitter",
            true,
            "Applies sub-pixel projection jitter for DLSS temporal accumulation.",
            () => Plugin.Instance.Settings.SetDLSS()
        );

        ConfigDLSSJitterStrength = Bind(
            "Scaling",
            "DLSSJitterStrength",
            0.4f,
            "Controls the jitter for DLSS, essentially turn it up until you see shaking or a decrease in visual quality.",
            () => Plugin.Instance.Settings.SetDLSS(),
            v => Mathf.Clamp(v, 0f, 1f)
        );

        ConfigLODQuality = Bind(
            "LOD",
            "LODQuality",
            2.5f,
            "Controls the game's LOD bias. Higher values increase detail distance. Range 0.1-10.",
            () => Plugin.Instance.Settings.SetLODQuality(),
            v => Mathf.Clamp(v, 0.1f, 10f)
        );

        ConfigShadowDistance = Bind(
            "Shadows",
            "ShadowDistance",
            300,
            "Controls the maximum distance at which shadows render. Range 0-1000.",
            () => Plugin.Instance.Settings.SetShadowDistance(),
            v => Mathf.Clamp(v, 0, 1000)
        );

        ConfigShadowCascades = Bind(
            "Shadows",
            "ShadowCascades",
            4,
            "Controls directional-light shadow cascades. Stonewards URP supports 1-4.",
            () => Plugin.Instance.Settings.SetShadowCascades(),
            v => Mathf.Clamp(v, 1, 4)
        );

        ConfigShadowmapResolution = Bind(
            "Shadows",
            "ShadowmapResolution",
            4096,
            "Controls the quality of the shadows, can reduce performance so turn it down if you're having issues. Makes the trees less wobbly",
            () => Plugin.Instance.Settings.SetShadowmapResolution(),
            v => Mathf.Clamp(v, 1024, 20480)
        );

        ConfigSoftShadows = Bind(
            "Shadows",
            "SoftShadows",
            true,
            "Allows shadows to be soft, if your PC is too low spec for a high shadowmap setting this to false can stop the wobblyness of the shadows (but will result in a pixelly effect if the shadowmap res is low)",
            () => Plugin.Instance.Settings.SetSoftShadows()
        );

        ConfigAnisotropicFiltering = Bind(
            "Quality",
            "AnisotropicFiltering",
            true,
            "Helps texture sharpness at angles (set to on)",
            () => Plugin.Instance.Settings.SetAnisotropicFiltering()
        );

        ConfigCameraAA = Bind(
            "AntiAliasing",
            "CameraAA",
            0,
            "Controls camera anti-aliasing. 0 = None, 1 = FXAA, 2 = SMAA, 3 = TAA.",
            () => Plugin.Instance.Settings.SetPostProcessAA(),
            v => Mathf.Clamp(v, 0, 3)
        );

        ConfigMSAA = Bind(
            "AntiAliasing",
            "MSAA",
            0,
            "Controls multi-sample anti-aliasing. Valid values are 0, 2, 4, and 8.",
            () => Plugin.Instance.Settings.SetMSAA(),
            v => v <= 0 ? 0 : v <= 2 ? 2 : v <= 4 ? 4 : 8
        );

        ConfigPixelationEnabled = Bind(
            "Pixelation",
            "Enabled",
            true,
            "Enables the custom high-quality pixelation effect.",
            () => Plugin.Instance.PixelationVolume?.ApplyConfiguration()
        );

        ConfigPixelationIntensity = Bind(
            "Pixelation",
            "Intensity",
            0.7f,
            "Controls pixel-grid strength relative to the screen resolution. 0 is native resolution; 1 uses a 12.5% resolution grid (8x8 pixel blocks).",
            () => Plugin.Instance.PixelationVolume?.ApplyConfiguration(),
            v => Mathf.Clamp(v, 0, 1)
        );

        ConfigPixelationColorPrecision = Bind(
            "Pixelation",
            "ColorPrecision",
            true,
            "Reduces the number of colour steps for a more cohesive pixel-art appearance.",
            () => Plugin.Instance.PixelationVolume?.ApplyConfiguration()
        );

        ConfigPixelationColorSteps = Bind(
            "Pixelation",
            "ColorSteps",
            96f,
            "Number of colour steps when Color Precision is enabled. Lower values are more stylised.",
            () => Plugin.Instance.PixelationVolume?.ApplyConfiguration(),
            v => Mathf.Clamp(v, 4f, 256f)
        );

        ConfigPixelationDithering = Bind(
            "Pixelation",
            "Dithering",
            true,
            "Applies subtle pixel aligned ordered dithering to reduce colour-banding.",
            () => Plugin.Instance.PixelationVolume?.ApplyConfiguration()
        );

        ConfigPixelationDitherPattern = Bind(
            "Pixelation",
            "DitherPattern",
            1,
            "Ordered dither pattern.",
            () => Plugin.Instance.PixelationVolume?.ApplyConfiguration(),
            v => Mathf.Clamp(v, 0, 10)
        );

        ConfigPixelationDitherStrength = Bind(
            "Pixelation",
            "DitherStrength",
            0.75f,
            "Strength of the ordered dither. Lower values are subtler.",
            () => Plugin.Instance.PixelationVolume?.ApplyConfiguration(),
            v => Mathf.Clamp(v, 0, 1)
        );
        if (MSAA != 0 && CameraAA == 3)
        {
            ConfigCameraAA.Value = 2;
        }

        ConfigMenuKey = Bind(
            "General",
            "Config Menu Key",
            "<Keyboard>/f11",
            "Control path for opening the mod configuration menu (e.g. <Keyboard>/f2, <Keyboard>/space, <Keyboard>/escape)",
            SetupInputAction
        );

        SetupInputAction();

        Plugin.Log.LogInfo("ConfigurationHandler initialised");
    }

    public void ForceDLSSOff()
    {
        ConfigDLSSMode.Value = (int)DLSSMode.Off;
        ConfigDLSSJitter.Value = false;
    }

    private ConfigEntry<T> Bind<T>(string section, string key, T defaultValue, string description, Action? onChanged = null, Func<T, T>? clamp = null)
    {
        var entry = _config.Bind(section, key, defaultValue, description);

        if (clamp != null) 
            entry.Value = clamp(entry.Value);

        Plugin.Log.LogInfo($"{key} set to: {entry.Value}");

        if (onChanged != null)
        {
            entry.SettingChanged += (_, _) =>
            {
                if (clamp != null)
                    entry.Value = clamp(entry.Value);

                onChanged();
            };
        }

        return entry;
    }

    private void SetupInputAction()
    {
        MenuAction?.Dispose();

        MenuAction = new InputAction(type: InputActionType.Button);
        MenuAction.AddBinding(ConfigMenuKey.Value);
        MenuAction.Enable();
    }

    public void Dispose()
    {
        MenuAction?.Dispose();
        MenuAction = null;
    }
}