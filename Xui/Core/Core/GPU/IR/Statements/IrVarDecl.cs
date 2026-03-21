namespace Xui.GPU.IR;

/// <summary>
/// Represents a variable declaration.
/// </summary>
public class IrVarDecl : IrStatement
{
    public override IrNodeKind Kind => IrNodeKind.VarDecl;
    public string Name { get; }
    public IrType Type { get; }
    public IrExpression? Initializer { get; }

    public IrVarDecl(string name, IrType type, IrExpression? initializer = null)
    {
        Name = name;
        Type = type;
        Initializer = initializer;
    }
}
