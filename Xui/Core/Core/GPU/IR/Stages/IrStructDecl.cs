namespace Xui.GPU.IR;

/// <summary>
/// Represents a struct declaration.
/// </summary>
public class IrStructDecl : IrNode
{
    public override IrNodeKind Kind => IrNodeKind.StructDecl;
    public IrStructType Type { get; }

    public IrStructDecl(IrStructType type)
    {
        Type = type;
    }
}
