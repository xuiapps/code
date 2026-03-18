namespace Xui.GPU.Shaders.Attributes;

/// <summary>
/// Specifies the location index for a vertex input or fragment output field.
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public sealed class LocationAttribute : Attribute
{
    /// <summary>
    /// Gets the location index.
    /// </summary>
    public int Index { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LocationAttribute"/> class.
    /// </summary>
    /// <param name="index">The location index.</param>
    public LocationAttribute(int index)
    {
        Index = index;
    }
}
