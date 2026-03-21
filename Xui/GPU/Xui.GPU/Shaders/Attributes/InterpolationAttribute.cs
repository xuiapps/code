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

/// <summary>
/// Attribute to control how a varying (vertex shader output / fragment shader input) is interpolated.
/// </summary>
/// <remarks>
/// This attribute maps to HLSL interpolation modifiers (linear, noperspective, nointerpolation),
/// Metal's [[flat]], [[center_perspective]], etc., and Vulkan's interpolation decorations.
/// </remarks>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class InterpolationAttribute : Attribute
{
    /// <summary>
    /// Gets the interpolation mode.
    /// </summary>
    public InterpolationMode Mode { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InterpolationAttribute"/> class.
    /// </summary>
    /// <param name="mode">The interpolation mode to use for this varying.</param>
    public InterpolationAttribute(InterpolationMode mode)
    {
        Mode = mode;
    }
}
