namespace Xui.GPU.Shaders.Attributes;

/// <summary>
/// Specifies the interpolation mode for a shader varying (vertex output / fragment input).
/// </summary>
public enum InterpolationMode
{
    /// <summary>
    /// Default perspective-correct interpolation.
    /// Values are interpolated with perspective correction across the triangle.
    /// </summary>
    Perspective,

    /// <summary>
    /// Linear interpolation without perspective correction.
    /// Values are interpolated linearly in screen space.
    /// </summary>
    Linear,

    /// <summary>
    /// No interpolation - flat shading.
    /// The value from the provoking vertex is used for the entire primitive.
    /// </summary>
    Flat
}
