using System;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using UnityEngine;

namespace Stonewards_Visuals.Pixelation;

public class StonewardsShaderBundle : IDisposable
{
    private static string BundleFileName = "stonewards_shaders";
    private string PSXMasterShaderName = "PSXMaster_URP";
    private AssetBundle _bundle;

    public Shader PSXMasterShader { get; private set; }

    public bool Load()
    {
        if (_bundle != null)
            return true;

        string? path = GetCandidatePaths().FirstOrDefault(File.Exists);
        if (path == null)
        {
            Plugin.Log.LogWarning($"Shader bundle {BundleFileName} was not found.");
            return false;
        }

        _bundle = AssetBundle.LoadFromFile(path);
        if (_bundle == null)
        {
            Plugin.Log.LogWarning($"Failed to load shader bundle from {path}");
            return false;
        }

        PSXMasterShader = _bundle.LoadAsset<Shader>(PSXMasterShaderName);
        if (PSXMasterShader == null)
            Plugin.Log.LogWarning($"Shader {PSXMasterShaderName} was not found {path}.");

        return PSXMasterShader != null;
    }

    public void Dispose()
    {
        PSXMasterShader = null;
        _bundle.Unload(false);
    }

    private static string[] GetCandidatePaths()
    {
        string assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
        return
        [
            Path.Combine(assemblyDirectory, "AssetBundles", BundleFileName),
            Path.Combine(assemblyDirectory, BundleFileName),
            Path.Combine(Paths.PluginPath, "Stonewards_Visuals", "AssetBundles", BundleFileName),
            Path.Combine(Paths.PluginPath, "Stonewards_Visuals", BundleFileName),
            Path.Combine(Paths.PluginPath, "AssetBundles", BundleFileName)
        ];
    }
}