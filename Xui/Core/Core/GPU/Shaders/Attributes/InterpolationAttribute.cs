namespace Xui.GPU.Shaders.Attributes;

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
