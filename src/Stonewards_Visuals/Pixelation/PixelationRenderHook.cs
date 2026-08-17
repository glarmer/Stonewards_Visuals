using HarmonyLib;
using UnityEngine.Rendering.Universal;

namespace Stonewards_Visuals.Pixelation;

[HarmonyPatch(typeof(ScriptableRenderer), "AddRenderPasses")]
public static class PixelationRenderHook
{
    [HarmonyPostfix]
    private static void AfterAddRenderPasses(ScriptableRenderer __instance, ref RenderingData renderingData)
    {
        Plugin.Instance?.PixelationController?.EnqueuePasses(__instance, ref renderingData);
    }
}