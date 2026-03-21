namespace Xui.GPU.Shaders.Attributes;

/// <summary>
/// Specifies that a field represents a built-in shader semantic.
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public sealed class BuiltInAttribute : Attribute
{
    /// <summary>
    /// Gets the built-in semantic.
    /// </summary>
    public BuiltIn Semantic { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BuiltInAttribute"/> class.
    /// </summary>
    /// <param name="semantic">The built-in semantic.</param>
    public BuiltInAttribute(BuiltIn semantic)
    {
        Semantic = semantic;
    }
}
