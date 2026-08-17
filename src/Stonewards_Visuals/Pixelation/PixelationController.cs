using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Stonewards_Visuals.Pixelation;

public class PixelationController : MonoBehaviour
{
    private PixelationEffectFeature? _feature;
    private ScriptableRenderer? _renderer;
    private Camera? _camera;
    
    public void Refresh(Camera? camera = null)
    {
        if (camera != null)
            _camera = camera;
        else if (_camera == null)
            _camera = Camera.main;

        EnsureFeatureInstalled();

        if (_feature != null && _feature.IsReady)
            _feature.SetActive(true);
    }
    
    private void EnsureFeatureInstalled()
    {
        if (_camera == null)
            return;

        if (!_camera.TryGetComponent(out UniversalAdditionalCameraData cameraData))
            return;
        
        cameraData.requiresColorTexture = true;

        var renderer = cameraData.scriptableRenderer;
        if (renderer == null)
            return;

        if (_feature == null)
        {
            if (Plugin.Instance.Shaders.PSXMasterShader == null)
                return;

            _feature = ScriptableObject.CreateInstance<PixelationEffectFeature>();
            _feature.name = "Stonewards Visuals Pixelation";
            _feature.Create();
            _feature.SetActive(false);

            if (!_feature.IsReady)
            {
                Destroy(_feature);
                _feature = null;
                return;
            }
        }

        if (_renderer == renderer)
            return;

        _renderer = renderer;
    }

    internal void EnqueuePasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (_feature == null || !_feature.IsReady || renderer != _renderer)
            return;

        _feature.AddRenderPasses(renderer, ref renderingData);
    }
    
    private void OnDestroy()
    {
        if (_feature != null)
        {
            _feature.Dispose();
            Destroy(_feature);
            _feature = null;
        }
    }
}