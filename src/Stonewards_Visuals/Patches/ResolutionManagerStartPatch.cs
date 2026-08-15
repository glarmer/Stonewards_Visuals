using HarmonyLib;
using Stonewards_Visuals.Upscaling;
using UnityEngine;

namespace Stonewards_Visuals.Patches;

[HarmonyPatch(typeof(ResolutionManager), nameof(ResolutionManager.Start))]
internal static class ResolutionManagerStartPatch
{
    private static int _gameLastTargetHeight = 432;
    private static bool _renderScaleOverrideActive;
    private static bool _applyingTargetHeight;

    internal static bool IsApplyingTargetHeight => _applyingTargetHeight;

    [HarmonyPostfix]
    private static void Postfix(ResolutionManager __instance)
    {
        if (Plugin.Instance == null)
            return;

        Plugin.Instance.Settings.SetAllSettings();
        ApplyTargetHeightFromRenderScale(__instance);
    }

    internal static void ApplyTargetHeightFromRenderScale(ResolutionManager? instance = null)
    {
        if (Plugin.Instance == null || !Plugin.Instance.ConfigurationHandler.EnableRenderScale)
            return;

        ResolutionManager? manager = instance ?? ResolutionManager.Instance;
        if (manager == null || Screen.height <= 0)
            return;

        if (!_renderScaleOverrideActive)
        {
            _gameLastTargetHeight = ResolutionManager.CurrentTargetHeight;
            _renderScaleOverrideActive = true;
        }

        float renderScale = GetActiveRenderScale();
        int targetHeight = Mathf.Max(1, Mathf.RoundToInt(Screen.height * renderScale));
        if (ResolutionManager.CurrentTargetHeight == targetHeight)
            return;

        _applyingTargetHeight = true;
        try
        {
            ResolutionManager.CurrentTargetHeight = targetHeight;
        }
        finally
        {
            _applyingTargetHeight = false;
        }
    }

    internal static void RestoreGameTargetHeight()
    {
        if (!_renderScaleOverrideActive)
            return;

        ResolutionManager.CurrentTargetHeight = _gameLastTargetHeight;
        _renderScaleOverrideActive = false;
    }

    private static float GetActiveRenderScale()
    {
        return Plugin.Instance.ConfigurationHandler.DLSSEnabled
            ? Settings.GetDLSSRenderScale(Plugin.Instance.ConfigurationHandler.DLSSMode)
            : Plugin.Instance.ConfigurationHandler.RenderScale;
    }
}