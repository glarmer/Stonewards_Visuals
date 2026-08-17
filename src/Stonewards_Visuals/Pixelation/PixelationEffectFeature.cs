using Stonewards_Visuals;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Stonewards_Visuals.Pixelation;

public class PixelationEffectFeature : ScriptableRendererFeature
{
    private Material? _material;
    private PixelationEffectPass? _pass;

    public bool IsReady => _material != null && _pass != null;

    public override void Create()
    {
        Shader? shader = Plugin.Instance.Shaders.PSXMasterShader;
        if (shader == null)
            return;

        if (!shader.isSupported)
        {
            Plugin.Log.LogWarning("Pixelation disabled because its shader is unsupported on this graphics API.");
            return;
        }

        _material ??= CoreUtils.CreateEngineMaterial(shader);
        if (_material == null || _material.passCount == 0)
        {
            Plugin.Log.LogWarning("Pixelation disabled because Unity could not create its material.");
            return;
        }

        _pass ??= new PixelationEffectPass(_material)
        {
            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        PixelationVolume? settings = Plugin.Instance.PixelationVolume;
        if (!IsReady || !settings.Enabled || renderingData.cameraData.cameraType != CameraType.Game)
            return;

        _pass!.Setup(_material!, settings);
        renderer.EnqueuePass(_pass);
    }

    protected override void Dispose(bool disposing)
    {
        _pass?.Dispose();
        _pass = null;
        CoreUtils.Destroy(_material);
        _material = null;
        base.Dispose(disposing);
    }
}