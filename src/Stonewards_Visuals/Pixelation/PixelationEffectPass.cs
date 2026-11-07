using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
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

    private sealed class PassData
    {
        public TextureHandle Source;
        public Material Material = null!;
    }

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

        ConfigureMaterial(new Vector2(
            Mathf.Max(1, renderingData.cameraData.cameraTargetDescriptor.width),
            Mathf.Max(1, renderingData.cameraData.cameraTargetDescriptor.height)));

        CommandBuffer commandBuffer = CommandBufferPool.Get(PassName);
        Blitter.BlitCameraTexture(commandBuffer, source, _temporaryColor, _material, 0);
        Blitter.BlitCameraTexture(commandBuffer, _temporaryColor, source);
        context.ExecuteCommandBuffer(commandBuffer);
        CommandBufferPool.Release(commandBuffer);
    }

    public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
    {
        if (_material == null || _settings == null || _settings.Intensity <= 0f)
            return;

        UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
        if (resourceData.isActiveTargetBackBuffer)
            return;

        TextureHandle source = resourceData.cameraColor;
        if (!source.IsValid())
            return;

        TextureDesc destinationDescriptor = source.GetDescriptor(renderGraph);
        destinationDescriptor.name = "_StonewardsPixelation";
        destinationDescriptor.clearBuffer = false;
        destinationDescriptor.msaaSamples = MSAASamples.None;
        TextureHandle destination = renderGraph.CreateTexture(destinationDescriptor);

        ConfigureMaterial(new Vector2(
            Mathf.Max(1, destinationDescriptor.width),
            Mathf.Max(1, destinationDescriptor.height)));

        using (IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass<PassData>(PassName, out PassData passData))
        {
            passData.Source = source;
            passData.Material = _material;

            builder.UseTexture(source, AccessFlags.Read);
            builder.SetRenderAttachment(destination, 0, AccessFlags.Write);
            builder.AllowPassCulling(false);
            builder.SetRenderFunc((PassData data, RasterGraphContext context) =>
            {
                Blitter.BlitTexture(context.cmd, data.Source, new Vector4(1f, 1f, 0f, 0f), data.Material, 0);
            });
        }

        resourceData.cameraColor = destination;
    }

    private void ConfigureMaterial(Vector2 screen)
    {
        if (_material == null || _settings == null)
            return;

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
    }

    public void Dispose()
    {
        _temporaryColor?.Release();
        _temporaryColor = null;
    }
}