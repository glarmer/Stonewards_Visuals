using HarmonyLib;

namespace Stonewards_Visuals.Patches;

[HarmonyPatch(typeof(ResolutionManager), "UpdateRenderScale")]
internal static class ResolutionManagerUpdateRenderScalePatch
{
    [HarmonyPostfix]
    private static void Postfix(ResolutionManager __instance)
    {
        if (Plugin.Instance == null || !Plugin.Instance.ConfigurationHandler.EnableRenderScale || ResolutionManagerStartPatch.IsApplyingTargetHeight)
        {
            return;
        }

        ResolutionManagerStartPatch.ApplyTargetHeightFromRenderScale(__instance);
    }
}