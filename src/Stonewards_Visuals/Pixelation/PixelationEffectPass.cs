using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Stonewards_Visuals.Pixelation;

public class PixelationEffectPass : ScriptableRenderPass
{
    private static readonly int ColorPrecision = Shader.PropertyToID("_ColorPrecision");
    private static readonly int EnableColorPrecision = Shader.PropertyToID("_EnableColorPrecision");
    private static readonly int PixelResolution = Shader.PropertyToID("_PixelResolution");
    private static readonly int EnablePixelation = Shader.PropertyToID("_EnablePixelation");
    private static readonly int EnableDither = Shader.PropertyToID("_EnableDither");
    private static readonly int DitherMode = Shader.PropertyToID("_DitherMode");
    private static readonly int DitherPattern = Shader.PropertyToID("_DitherPattern");
    private static readonly int DitherPixelPerfect = Shader.PropertyToID("_DitherPixelPerfect");
    private static readonly int DitherThreshold = Shader.PropertyToID("_DitherThreshold");
    private static readonly int EnablePalette = Shader.PropertyToID("_EnablePalette");
    private static readonly int EnableFog = Shader.PropertyToID("_EnableFog");
    
    private const string PassName = "Stonewards Pixelation";
    private Material? _material;
    private PixelationVolume? _settings;
    private RTHandle? _temporaryColor;

    public PixelationEffectPass(Material material)
    {
        _material = material;
        requiresIntermediateTexture = true;
    }

    public void Setup(Material material, PixelationVolume settings)
    {
        _material = material;
        _settings = settings;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (_material == null || _settings == null || _settings.Intensity <= 0f)
            return;

        RTHandle source = renderingData.cameraData.renderer.cameraColorTargetHandle;
        if (source == null || source.rt == null)
            return;

        RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
        descriptor.depthBufferBits = 0;
        descriptor.msaaSamples = 1;
        RenderingUtils.ReAllocateHandleIfNeeded(ref _temporaryColor, descriptor, FilterMode.Point, TextureWrapMode.Clamp, name: "_StonewardsPixelation");

        if (_temporaryColor == null)
            return;

        Vector2 screen = new(Mathf.Max(1, Screen.width), Mathf.Max(1, Screen.height));
        Vector2 grid = screen * Mathf.Lerp(1f, 0.125f, Mathf.Clamp01(_settings.Intensity));
        
        _material.SetFloat(EnablePixelation, 1f);
        _material.SetVector(PixelResolution, grid);
        _material.SetFloat(EnableColorPrecision, _settings.ColorPrecisionEnabled ? 1f : 0f);
        _material.SetFloat(ColorPrecision, _settings.ColorSteps);
        _material.SetFloat(EnableDither, _settings.DitheringEnabled ? 1f : 0f);
        _material.SetFloat(DitherMode, 2f);
        _material.SetInt(DitherPattern, _settings.DitherPattern);
        _material.SetFloat(DitherPixelPerfect, 1f);
        _material.SetFloat(DitherThreshold, 1f - _settings.DitherStrength);
        _material.SetFloat(EnablePalette, 0f);
        _material.SetFloat(EnableFog, 0f);

        CommandBuffer commandBuffer = CommandBufferPool.Get(PassName);
        Blitter.BlitCameraTexture(commandBuffer, source, _temporaryColor, _material, 0);
        Blitter.BlitCameraTexture(commandBuffer, _temporaryColor, source);
        context.ExecuteCommandBuffer(commandBuffer);
        CommandBufferPool.Release(commandBuffer);
    }

    public void Dispose()
    {
        _temporaryColor?.Release();
        _temporaryColor = null;
    }
}