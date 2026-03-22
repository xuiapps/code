namespace Xui.GPU.IR;

/// <summary>
/// Interpolation mode for varyings.
/// </summary>
public enum InterpolationMode
{
    /// <summary>Linear interpolation without perspective correction.</summary>
    Linear,
    /// <summary>No interpolation; value is constant across the primitive.</summary>
    Flat,
    /// <summary>Perspective-correct interpolation (default).</summary>
    Perspective
}
