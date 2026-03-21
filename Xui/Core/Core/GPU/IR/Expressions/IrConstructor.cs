namespace Xui.GPU.IR;

/// <summary>
/// Represents a constructor call.
/// </summary>
public class IrConstructor : IrExpression
{
    public override IrNodeKind Kind => IrNodeKind.Constructor;
    public override IrType Type { get; }
    public List<IrExpression> Arguments { get; }

    public IrConstructor(IrType type, List<IrExpression> arguments)
    {
        Type = type;
        Arguments = arguments;
    }
}
