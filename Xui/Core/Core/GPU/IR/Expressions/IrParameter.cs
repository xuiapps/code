namespace Xui.GPU.IR;

/// <summary>
/// Represents a parameter reference.
/// </summary>
public class IrParameter : IrExpression
{
    public override IrNodeKind Kind => IrNodeKind.Parameter;
    public override IrType Type { get; }
    public string Name { get; }

    public IrParameter(string name, IrType type)
    {
        Name = name;
        Type = type;
    }
}
