using UnityEngine;

namespace Stonewards_Visuals.Pixelation;

public sealed class PixelationVolume : MonoBehaviour
{
    public bool Enabled { get; private set; }
    public float Intensity { get; private set; }
    public bool ColorPrecisionEnabled { get; private set; }
    public float ColorSteps { get; private set; }
    public bool DitheringEnabled { get; private set; }
    public int DitherPattern { get; private set; }
    public float DitherStrength { get; private set; }

    private void Awake() => ApplyConfiguration();

    internal void ApplyConfiguration()
    {
        var config = Plugin.Instance.ConfigurationHandler;
        Enabled = config.PixelationEnabled;
        Intensity = config.PixelationIntensity;
        ColorPrecisionEnabled = config.PixelationColorPrecisionEnabled;
        ColorSteps = config.PixelationColorSteps;
        DitheringEnabled = config.PixelationDitheringEnabled;
        DitherPattern = config.PixelationDitherPattern;
        DitherStrength = config.PixelationDitherStrength;
    }
}