namespace Xui.GPU.IR;

/// <summary>
/// Represents a struct declaration.
/// </summary>
public class IrStructDecl : IrNode
{
    /// <summary>Gets the IR node kind for this declaration.</summary>
    public override IrNodeKind Kind => IrNodeKind.StructDecl;
    /// <summary>Gets the struct type being declared.</summary>
    public IrStructType Type { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="IrStructDecl"/> with the specified struct type.
    /// </summary>
    public IrStructDecl(IrStructType type)
    {
        Type = type;
    }
}
