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

/// <summary>
/// Built-in shader semantics.
/// </summary>
public enum BuiltIn
{
    /// <summary>
    /// Vertex position in clip space.
    /// </summary>
    Position,

    /// <summary>
    /// Fragment depth value.
    /// </summary>
    FragDepth,

    /// <summary>
    /// Vertex instance ID.
    /// </summary>
    InstanceId,

    /// <summary>
    /// Vertex ID.
    /// </summary>
    VertexId,
}
