namespace Xui.GPU.IR;

/// <summary>
/// Base class for all IR nodes in the intermediate representation.
/// </summary>
public abstract class IrNode
{
    /// <summary>
    /// Gets the node kind for pattern matching and visitors.
    /// </summary>
    public abstract IrNodeKind Kind { get; }

    /// <summary>
    /// Gets or sets the source location where this node originates from.
    /// Used to generate accurate error messages mapping back to C# source.
    /// </summary>
    public SourceLocation? SourceLocation { get; set; }
}
